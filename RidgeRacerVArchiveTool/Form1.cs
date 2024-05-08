using RidgeRacerVArchiveTool.RR5_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RidgeRacerVArchiveTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (bgWorkerPack.IsBusy || bgWorkerUnpack.IsBusy)
            {
                return;
            }

            string[] path = Environment.GetCommandLineArgs();
            // not set files or folder of extracted archive file
            if (path.Count() == 1)
            {
                // works as "Region Select" mode.
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

            for (int i = 1; i < 2; i++)
            {
                string fileDirectory = Path.GetDirectoryName(path[i]);
                string fileName = Path.GetFileName(path[i]);

                bool isDirectory = (File.GetAttributes(path[i]) & FileAttributes.Directory) == FileAttributes.Directory;
                if (!arcName.Equals(fileName) && !isDirectory)
                {
                    MessageBox.Show($"Please Drag & Drop R5.ALL ({region} region).");
                    return;
                }

                string elfPath = $@"{fileDirectory}\{elfName}";
                if (!File.Exists(elfPath))
                {
                    MessageBox.Show($"Faild: Couldn't find {elfName}.");
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
                    string destPath = $@"{fileDirectory}\{fileName.Replace('.', '_')}_unpack";
                    argments.Add("destPath", destPath);

                    bgWorkerUnpack.WorkerReportsProgress = true;
                    bgWorkerUnpack.RunWorkerAsync(argments);

                // Pack
                }
                else if (isDirectory)
                {
                    string destPath = $@"{fileDirectory}\pack";
                    argments.Add("destPath", destPath);
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
            string _str;
            argments.TryGetValue("tocAddress", out _str);
            int tocAddress;
            int.TryParse(_str, out tocAddress);
            int fileCount;
            argments.TryGetValue("fileCount", out _str);
            int.TryParse(_str, out fileCount);
            string destPath;
            argments.TryGetValue("destPath", out destPath);

            try
            {
                Directory.CreateDirectory(destPath);
    
                // elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.Read))
                // arc file
                using (FileStream arcFileStream = new FileStream(arcPath, FileMode.Open, FileAccess.Read))
                // csv file
                using (StreamWriter streamWriterHashList = new StreamWriter($@"{destPath}\hashList.csv"))
                {
                    streamWriterHashList.WriteLine("Id, Id (Hex), Hash (SHA1), Compress");

                    elfFileStream.Seek((long)tocAddress, SeekOrigin.Begin);

                    for (int i = 0; i < fileCount; i++)
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
                        using (SHA1 sha1 = SHA1.Create())
                        {
                            byte[] destBytes = new byte[toc.compressedSize];
                            arcFileStream.Seek(toc.blockOffset * 0x800, SeekOrigin.Begin);
                            arcFileStream.Read(destBytes, 0x00, toc.compressedSize);
                            destFileStream.Write(destBytes, 0x00, toc.compressedSize);

                            byte[] hash = sha1.ComputeHash(destBytes);
                            string row = String.Format("{0:D8}, {0:X8}, {1}, {2}", i, BitConverter.ToString(hash).Replace("-", ""), (toc.compressedSize < toc.uncompressedSize ? "Yes" : "No"));
                            streamWriterHashList.WriteLine(row);

                        }

                        bgWorker.ReportProgress(i+1); // update progress var
                    }


                }
            }
            catch
            {
                MessageBox.Show("Faild: Unpack."); //
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

            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            string destPath;
            argments.TryGetValue("destPath", out destPath);
            string srcPath;
            argments.TryGetValue("srcPath", out srcPath);

            List<string> fileSortList = new List<string>(Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories));
            if (fileSortList.Count != fileCount)
            {
                MessageBox.Show($"Faild: Unmaching region and file counts. Detected: {fileSortList.Count} files. Except: {fileCount} files."); //
                return;
            }

            string elfPathMod = $@"{destPath}\{Path.GetFileName(elfPath)}";
            string arcPathMod = $@"{destPath}\{Path.GetFileName(arcPath)}";

            try
            {
                Directory.CreateDirectory(destPath);

                // duplicate elf file
                using (FileStream elfFileStream = new FileStream(elfPath, FileMode.Open, FileAccess.Read))
                using (FileStream elfFileStreamMod = new FileStream(elfPathMod, FileMode.Create, FileAccess.Write))
                {
                    elfFileStream.CopyTo(elfFileStreamMod);
                }

                // mod elf file
                using (FileStream elfFileStreamMod = new FileStream(elfPathMod, FileMode.Open, FileAccess.ReadWrite))
                // mof arc file
                using (FileStream arcFileStreamMod = new FileStream(arcPathMod, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = new byte[1];

                    int i = 0;
                    // Goto TOC Address
                    elfFileStreamMod.Seek(tocAddress, SeekOrigin.Begin);
                    foreach (string srcFilePath in fileSortList)
                    {
                        TOC toc = new TOC();
                        using (FileStream srcFileStream = new FileStream(srcFilePath, FileMode.Open, FileAccess.Read))
                        {
                            if (arcFileStreamMod.Position > 1)
                            {
                                toc.blockOffset = (int)arcFileStreamMod.Position / 0x800;
                            }

                            // Write TOC
                            toc.Pack(srcFileStream, elfFileStreamMod);

                            bytes = new byte[toc.compressedSize];
                            srcFileStream.Read(bytes, 0x00, toc.compressedSize);
                        }
                        arcFileStreamMod.Write(bytes, 0x00, toc.compressedSize);

                        // padding
                        bytes = new byte[1];
                        bytes[0] = 0x00;
                        int padding = (toc.blockSize * 0x800) - (toc.compressedSize);
                        for (int j = 0; j < padding; j++)
                        {
                            arcFileStreamMod.Write(bytes, 0x00, bytes.Length);
                        }

                        bgWorker.ReportProgress(i+1); // update progress var
                        i++;
                    }

                    // check teminator of TOC
                    bytes = new byte[4];
                    elfFileStreamMod.Read(bytes, 0x00, bytes.Length);
                    int terminator = BitConverter.ToInt32(bytes, 0x00);
                    if (terminator != 0xCC0000)
                    {
                        MessageBox.Show("Faild: Pack. unexpected terminator");
                        return;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Faild: Pack.");
                return;
            }

            MessageBox.Show("Success: Pack Complete.");

        }

        private void bgWorkerPack_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = $@"{e.ProgressPercentage.ToString()}/{progressBar1.Maximum.ToString()}"; 
        }

        private void bgWorkerPack_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label1.Text = "Done.";

            Close();
        }

    }
}
