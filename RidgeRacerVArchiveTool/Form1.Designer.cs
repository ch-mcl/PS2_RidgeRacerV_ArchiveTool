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
            this.Label1 = new System.Windows.Forms.Label();
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label2 = new System.Windows.Forms.Label();
            this.radioPAL = new System.Windows.Forms.RadioButton();
            this.radioUS = new System.Windows.Forms.RadioButton();
            this.radioJP = new System.Windows.Forms.RadioButton();
            this.radioACV3A = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(10, 38);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(133, 12);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Please Drag&Drop R5.ALL.";
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(12, 12);
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(300, 18);
            this.ProgressBar1.TabIndex = 1;
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            this.BackgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker1_ProgressChanged);
            this.BackgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(164, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Set your Ridge Racer V region.";
            // 
            // radioPAL
            // 
            this.radioPAL.AutoSize = true;
            this.radioPAL.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.PALchecked;
            this.radioPAL.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "PALchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioPAL.Location = new System.Drawing.Point(104, 85);
            this.radioPAL.Name = "radioPAL";
            this.radioPAL.Size = new System.Drawing.Size(44, 16);
            this.radioPAL.TabIndex = 5;
            this.radioPAL.Text = "PAL";
            this.radioPAL.UseVisualStyleBackColor = true;
            this.radioPAL.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioPAL_MouseClick);
            // 
            // radioUS
            // 
            this.radioUS.AutoSize = true;
            this.radioUS.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.USchecked;
            this.radioUS.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "USchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioUS.Location = new System.Drawing.Point(58, 85);
            this.radioUS.Name = "radioUS";
            this.radioUS.Size = new System.Drawing.Size(38, 16);
            this.radioUS.TabIndex = 4;
            this.radioUS.Text = "US";
            this.radioUS.UseVisualStyleBackColor = true;
            this.radioUS.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioUS_MouseClick);
            // 
            // radioJP
            // 
            this.radioJP.AutoSize = true;
            this.radioJP.Checked = global::RidgeRacerVArchiveTool.Properties.Settings.Default.JPchecked;
            this.radioJP.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::RidgeRacerVArchiveTool.Properties.Settings.Default, "JPchecked", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.radioJP.Location = new System.Drawing.Point(12, 85);
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
            this.radioACV3A.Location = new System.Drawing.Point(154, 85);
            this.radioACV3A.Name = "radioACV3A";
            this.radioACV3A.Size = new System.Drawing.Size(65, 16);
            this.radioACV3A.TabIndex = 6;
            this.radioACV3A.Text = "AC_V3A";
            this.radioACV3A.UseVisualStyleBackColor = true;
            this.radioACV3A.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioACV3A_MouseClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 110);
            this.Controls.Add(this.radioACV3A);
            this.Controls.Add(this.radioPAL);
            this.Controls.Add(this.radioUS);
            this.Controls.Add(this.radioJP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.Label1);
            this.Name = "Form1";
            this.Text = "Ridge Racer V Archive Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ProgressBar ProgressBar1;
        private System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioJP;
        private System.Windows.Forms.RadioButton radioUS;
        private System.Windows.Forms.RadioButton radioPAL;
        private System.Windows.Forms.RadioButton radioACV3A;
    }
}

