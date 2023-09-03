using RidgeRacerVArchiveTool.RR5_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace RidgeRacerVArchiveTool
{
    public partial class Form1 : Form
    {
        public bool result = true;
        public bool close = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (bgWorkerPack.IsBusy)
            {
                return;
            }

            string[] path = Environment.GetCommandLineArgs();
            // not set files
            if (path.Count() == 1)
            {
                return;
            }


            progressBar1.Minimum = 0;
            progressBar1.Value = 0;

            label1.Text = "Execeution..."; //

            radioJP.Enabled = false; //
            radioUS.Enabled = false; //
            radioPAL.Enabled = false; //
            radioACV3A.Enabled = false; //

            string region = "";
            if (radioJP.Checked == true)
            {
                region = "JP";
            }
            else if (radioUS.Checked == true)
            {
                region = "US";
            }
            else if (radioPAL.Checked == true)
            {
                region = "PAL";
            }
            else if (radioACV3A.Checked == true)
            {
                region = "AC_RRV3_A";
            }

            if (region.Length < 1)
            {
                return;
            }

            // Get Values from Master Table
            RR5_TOC_Table tocTabl = new RR5_TOC_Table();
            DataRow[] dataRows = tocTabl.tocTable.Select($"region = '{region}'");
            DataRow row = dataRows[0];
            string elfName = row[tocTabl.COL_NAME_ELF].ToString();
            int tocAddress;
            int.TryParse(row[tocTabl.COL_NAME_TOC_ADR].ToString(), out tocAddress);
            string arcName = row[tocTabl.COL_NAME_ARC].ToString();

            int fileCount;
            int.TryParse(row[tocTabl.COL_NAME_MAX_TOC].ToString(), out fileCount);
            progressBar1.Maximum = fileCount;

            for (int i = 1; i < 2/*path.Count()*/; i++)
            {
                //string fileExtension = Path.GetExtension(path[i]);


                string fileDirectory = Path.GetDirectoryName(path[i]);
                string fileName = Path.GetFileName(path[i]);

                bool isDirectory = (File.GetAttributes(path[i]) & FileAttributes.Directory) == FileAttributes.Directory;
                if (!arcName.Equals(fileName) && !isDirectory)
                {
                    MessageBox.Show($"Please Drag & Drop R5.ALL ({region} region).");
                    close = true;
                    return;
                }

                string elfPath = $@"{fileDirectory}\{elfName}";
                if (!File.Exists(elfPath))
                {
                    MessageBox.Show($"Faild: Couldn't find {elfName}.");
                    close = true;
                    return;
                }
                string arcPath = $@"{fileDirectory}\{arcName}";

                Dictionary<string, string> argments = new Dictionary<string, string>();
                argments.Add("elfPath", elfPath);
                argments.Add("arcPath", arcPath);
                argments.Add("tocAddress", tocAddress.ToString());
                argments.Add("fileCount", fileCount.ToString());

                // Unpack
                if (arcName.Equals(fileName))
                {
                    string destPath = $@"{fileDirectory}\{fileName.Replace('.', '_')}_extract";
                    argments.Add("destPath", destPath);

                    bgWorkerUnpack.WorkerReportsProgress = true;
                    bgWorkerUnpack.RunWorkerAsync(argments);

                // Pack
                }
                else if (isDirectory)
                {
                    string srcPath = path[i];
                    argments.Add("srcPath", srcPath);

                    bgWorkerPack.WorkerReportsProgress = true;
                    bgWorkerPack.RunWorkerAsync(argments);

                }
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void radioJP_MouseClick(object sender, MouseEventArgs e)
        {
            radioJP.Checked = true;
            radioUS.Checked = false;
            radioPAL.Checked = false;
            radioACV3A.Checked = false;
        }

        private void radioUS_MouseClick(object sender, MouseEventArgs e)
        {
            radioJP.Checked = false;
            radioUS.Checked = true;
            radioPAL.Checked = false;
            radioACV3A.Checked = false;
        }

        private void radioPAL_MouseClick(object sender, MouseEventArgs e)
        {
            radioJP.Checked = false;
            radioUS.Checked = false;
            radioPAL.Checked = true;
            radioACV3A.Checked = false;
        }


        private void radioACV3A_MouseClick(object sender, MouseEventArgs e)
        {
            radioJP.Checked = false;
            radioUS.Checked = false;
            radioPAL.Checked = false;
            radioACV3A.Checked = true;
        }


        private void bgWorkerUnpack_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker)sender;

            Dictionary<string, string> argments = e.Argument as Dictionary<string, string>;
            string fileName;
            argments.TryGetValue("fileName", out fileName);
            string elfPath;
            argments.TryGetValue("elfPath", out elfPath);
            string arcPath;
            argments.TryGetValue("arcPath", out arcPath);
            string _strTocAddress;
            argments.TryGetValue("tocAddress", out _strTocAddress);
            int tocAddress;
            int.TryParse(_strTocAddress, out tocAddress);
            string destPath;
            argments.TryGetValue("destPath", out destPath);

            Directory.CreateDirectory(destPath);

            try
            {
                // elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.Read))
                // arc file
                using (FileStream arcFileStream = new FileStream(arcPath, FileMode.Open, FileAccess.Read))
                {
                    elfFileStream.Seek((long)tocAddress, SeekOrigin.Begin);

                    int i = 0;
                    while (true)
                    {
                        TOC toc = new TOC();
                        bool result = toc.Unpack(elfFileStream);
                        // get Terminator
                        if (result)
                        {
                            break;
                        }

                        string extention = RR5_Archive.EXT_RR5_RAW;
                        if (toc.compressedSize < toc.uncompressedSize)
                        {
                            extention = RR5_Archive.EXT_RR5_LZ; // Needs decompress by LZSS(RRV Format).
                        }

                        string destFileName = string.Format("{0:D8}.{1}", i, extention);
                        string fullpath = $@"{destPath}\{destFileName}";
                        using (FileStream destFileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
                        {
                            byte[] destBytes = new byte[toc.compressedSize];
                            arcFileStream.Seek(toc.blockOffset * 0x800, SeekOrigin.Begin);
                            arcFileStream.Read(destBytes, 0x00, toc.compressedSize);
                            destFileStream.Write(destBytes, 0x00, toc.compressedSize);
                        }

                        i++;
                        bgWorker.ReportProgress(i); // update progress var
                    }
                }
            }
            catch
            {
                MessageBox.Show("Faild: Unpack."); //
                close = true;
                return;
            }

            MessageBox.Show("Success: Unpack Complete."); //

        }

        private void bgWorkerUnpack_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = $@"{e.ProgressPercentage.ToString()}/{progressBar1.Maximum.ToString()}";
        }

        private void bgWorkerUnpack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Done.";

            if (close == true)
                Close();
        }

        private void bgWorkerPack_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker)sender;

            Dictionary<string, string> argments = e.Argument as Dictionary<string, string>;
            string fileName;
            argments.TryGetValue("fileName", out fileName);
            string elfPath;
            argments.TryGetValue("elfPath", out elfPath);
            string arcPath;
            argments.TryGetValue("arcPath", out arcPath);
            string _str;
            argments.TryGetValue("tocAddress", out _str);
            int tocAddress;
            int.TryParse(_str, out tocAddress);
            int fileCount;
            argments.TryGetValue("fileCount", out _str);
            int.TryParse(_str, out fileCount);

            string srcPath;
            argments.TryGetValue("srcPath", out srcPath);

            // Experimentally features.
            // TODO: Implement "RollBack" features when Pack is faild. 
            List<string> fileSortList = new List<string>(Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories));
            if (fileSortList.Count != fileCount)
            {
                MessageBox.Show($"Faild: Unmaching region and file counts. Detected: {fileSortList.Count} files. Except: {fileCount} files."); //
                close = true;
                return;
            }

            try
            {
                // elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.ReadWrite))
                // arc file
                using (FileStream arcFileStream = new FileStream(arcPath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[1];

                    int i = 0;
                    // Goto TOC Address
                    elfFileStream.Seek(tocAddress, SeekOrigin.Begin);
                    foreach (string srcFilePath in fileSortList)
                    {
                        TOC toc = new TOC();
                        using (FileStream srcFileStream = new FileStream(srcFilePath, FileMode.Open, FileAccess.Read))
                        {
                            if (arcFileStream.Position > 1)
                            {
                                toc.blockOffset = (int)arcFileStream.Position / 0x800;
                            }

                            // Write TOC
                            toc.Pack(srcFileStream, elfFileStream);

                            bytes = new byte[toc.compressedSize];
                            srcFileStream.Read(bytes, 0x00, toc.compressedSize);
                        }
                        arcFileStream.Write(bytes, 0x00, toc.compressedSize);

                        // padding
                        bytes = new byte[1];
                        bytes[0] = 0x00;
                        int padding = (toc.blockSize * 0x800) - (toc.compressedSize);
                        for (int j = 0; j < padding; j++)
                        {
                            arcFileStream.Write(bytes, 0x00, bytes.Length);
                        }

                        i++;
                        bgWorker.ReportProgress(i); // update progress var
                    }

                    // check teminator of TOC
                    bytes = new byte[4];
                    elfFileStream.Read(bytes, 0x00, bytes.Length);
                    int terminator = BitConverter.ToInt32(bytes, 0x00);
                    if (terminator != 0xCC0000)
                    {
                        MessageBox.Show("Faild: Pack."); //
                        close = true;
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Faild: Pack."); //
                close = true;
                return;
            }

            MessageBox.Show("Success: Pack Complete."); //

        }

        private void bgWorkerPack_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = $@"{e.ProgressPercentage.ToString()}/{progressBar1.Maximum.ToString()}"; 
        }

        private void bgWorkerPack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Done.";

            if (close == true)
                Close();
        }

    }
}
