namespace RidgeRacerVArchiveTool
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.bgWorkerPack = new System.ComponentModel.BackgroundWorker();
            this.bgWorkerUnpack = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioJP = new System.Windows.Forms.RadioButton();
            this.radioACV3A = new System.Windows.Forms.RadioButton();
            this.radioUS = new System.Windows.Forms.RadioButton();
            this.radioPAL = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please Drag&Drop R5.ALL.";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 12);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(300, 18);
            this.progressBar1.TabIndex = 1;
            // 
            // bgWorkerUnpack
            // 
            this.bgWorkerUnpack.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerUnpack_DoWork);
            this.bgWorkerUnpack.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerUnpack_ProgressChanged);
            this.bgWorkerUnpack.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerUnpack_RunWorkerCompleted);
            // 
            // bgWorkerPack
            // 
            this.bgWorkerPack.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorkerPack_DoWork);
            this.bgWorkerPack.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorkerPack_ProgressChanged);
            this.bgWorkerPack.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorkerPack_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioJP);
            this.groupBox1.Controls.Add(this.radioACV3A);
            this.groupBox1.Controls.Add(this.radioUS);
            this.groupBox1.Controls.Add(this.radioPAL);
            this.groupBox1.Location = new System.Drawing.Point(12, 58);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(300, 40);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Region of Ridge Racer V";
            // 
            // radioJP
            // 
            this.radioJP.AutoSize = true;
            this.radioJP.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.JPchecked;
            this.radioJP.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "JPchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioJP.Location = new System.Drawing.Point(6, 18);
            this.radioJP.Name = "radioJP";
            this.radioJP.Size = new System.Drawing.Size(37, 16);
            this.radioJP.TabIndex = 3;
            this.radioJP.TabStop = true;
            this.radioJP.Text = "JP";
            this.radioJP.UseVisualStyleBackColor = true;
            this.radioJP.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioJP_MouseClick);
            // 
            // radioACV3A
            // 
            this.radioACV3A.AutoSize = true;
            this.radioACV3A.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.ACV3Achecked;
            this.radioACV3A.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "ACV3Achecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioACV3A.Location = new System.Drawing.Point(143, 18);
            this.radioACV3A.Name = "radioACV3A";
            this.radioACV3A.Size = new System.Drawing.Size(65, 16);
            this.radioACV3A.TabIndex = 6;
            this.radioACV3A.Text = "AC_V3A";
            this.radioACV3A.UseVisualStyleBackColor = true;
            this.radioACV3A.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioACV3A_MouseClick);
            // 
            // radioUS
            // 
            this.radioUS.AutoSize = true;
            this.radioUS.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.USchecked;
            this.radioUS.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "USchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioUS.Location = new System.Drawing.Point(49, 18);
            this.radioUS.Name = "radioUS";
            this.radioUS.Size = new System.Drawing.Size(38, 16);
            this.radioUS.TabIndex = 4;
            this.radioUS.Text = "US";
            this.radioUS.UseVisualStyleBackColor = true;
            this.radioUS.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioUS_MouseClick);
            // 
            // radioPAL
            // 
            this.radioPAL.AutoSize = true;
            this.radioPAL.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.PALchecked;
            this.radioPAL.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "PALchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioPAL.Location = new System.Drawing.Point(93, 18);
            this.radioPAL.Name = "radioPAL";
            this.radioPAL.Size = new System.Drawing.Size(44, 16);
            this.radioPAL.TabIndex = 5;
            this.radioPAL.Text = "PAL";
            this.radioPAL.UseVisualStyleBackColor = true;
            this.radioPAL.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioPAL_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 110);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Ridge Racer V Archive Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker bgWorkerUnpack;
        private System.ComponentModel.BackgroundWorker bgWorkerPack;
        private System.Windows.Forms.RadioButton radioJP;
        private System.Windows.Forms.RadioButton radioUS;
        private System.Windows.Forms.RadioButton radioPAL;
        private System.Windows.Forms.RadioButton radioACV3A;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

