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
        public Form1()
        {
            InitializeComponent();
        }

        public bool result = true;
        public bool close = false;
        public int process = 0;
        public int fileCount = 0;

        private void Form1_Shown(object sender, EventArgs e)
        {
            ProgressBar1.Minimum = 0;
            ProgressBar1.Value = 0;
            BackgroundWorker1.WorkerReportsProgress = true;
            BackgroundWorker1.RunWorkerAsync();
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

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = process;
            ProgressBar1.Maximum = fileCount;
            Label1.Text = process.ToString() + "/" + fileCount.ToString();
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (close == true)
                Close();
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bgWorker = (BackgroundWorker)sender;
            string[] path = Environment.GetCommandLineArgs();

            // not set files
            if (path.Count() == 1)
            {
                return;
            }

            Label1.Text = "Execeution...";


            radioJP.Enabled = false;
            radioUS.Enabled = false;
            radioPAL.Enabled = false;
            radioACV3A.Enabled = false;
            string region = "";
            if (radioJP.Checked == true)
            {
                region = "JP";
            }
            else if (radioUS.Checked == true)
            {
                region = "US";
            }
            else if(radioPAL.Checked == true)
            {
                region = "PAL";
            }
            else if (radioACV3A.Checked == true)
            {
                region = "AC_RRV3_A";
            }

            for (int i = 1; i < path.Count(); i++)
            {
                //string fileExtension = Path.GetExtension(path[i]);

                RR5_TOC_Table tocTabl = new RR5_TOC_Table();
                DataRow[] dataRows = tocTabl.tocTable.Select($"region = '{region}'");
                DataRow row = dataRows[0];
                string fileDirectory = Path.GetDirectoryName(path[i]);
                string elfName = row[tocTabl.COL_NAME_ELF].ToString();
                int tocAddress;
                int.TryParse(row[tocTabl.COL_NAME_TOC_ADR].ToString(), out tocAddress);
                string arcName = row[tocTabl.COL_NAME_ARC].ToString();

                fileCount = int.Parse(row[tocTabl.COL_NAME_MAX_TOC].ToString());

                RR5_Archive arc = new RR5_Archive();
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

                // Unpack
                if (arcName.Equals(fileName)) {

                    string extractPath = $@"{fileDirectory}\{fileName.Replace('.', '_')}_extract";
                    Directory.CreateDirectory(extractPath);


                    result = arc.Unpack(extractPath, elfPath, arcPath, tocAddress);

                    if (result == true)
                    {
                        MessageBox.Show("Faild: Unpack.");
                        close = true;
                        return;
                    }

                    MessageBox.Show("Success: Unpack Complete.");
                    // Pack
                } else if (isDirectory)
                {
                    // Experimentally features.
                    // TODO: Implement "RollBack" features when Pack is faild. 
                    List<string> fileSortList = new List<string>(Directory.GetFiles(path[i], "*", SearchOption.AllDirectories));
                    if (fileSortList.Count != fileCount)
                    {
                        MessageBox.Show($"Faild: Unmaching region and file counts. Detected: {fileSortList.Count} files. Except: {fileCount} files.");
                        close = true;
                        return;
                    }

                    result = arc.Pack(fileSortList, elfPath, arcPath, tocAddress);

                    if (result == true)
                    {
                        MessageBox.Show("Faild: Pack.");
                        close = true;
                        return;
                    }

                    MessageBox.Show("Success: Pack Complete.");
                }
            }

            Label1.Text = "Done.";
            return;
        }
    }
}
