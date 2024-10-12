using RRV_Lib;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace RRVDecompress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string objpath;
        public static string filepath;
        public static string fpposition;
        public static bool decompResult = false;
        public static string[] path = new string[] { "" };
        public static bool close = false;

        private void Form1_Shown(object sender, EventArgs e)
        {
            ProgressBar1.Minimum = 0;
            ProgressBar1.Value = 0;
            BackgroundWorker1.WorkerReportsProgress = true;
            BackgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressBar1.Value = e.ProgressPercentage;
            ProgressBar1.Maximum = Environment.GetCommandLineArgs().Length;
            Label1.Text =  string.Format("{0}/{1}", e.ProgressPercentage.ToString(), Environment.GetCommandLineArgs().Length);
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
            // if files not set
            if (path.Count() == 1)
            {
                close = true;
                return;
            }

            Label1.Text = "Exceution...";

            for (int i = 1; i < path.Count(); i++)
            {

                RRV_LZSS decompressor = new RRV_LZSS();
                decompResult = decompressor.Decompress(path[i]);

                if (decompResult == true)
                {
                    MessageBox.Show("Faild: Decompression is Faild");
                }



                Label1.Update();

            }

            close = true;
            return;
        }

    }
}