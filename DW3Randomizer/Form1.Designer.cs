namespace DW3Randomizer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSHAChecksum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblReqChecksum = new System.Windows.Forms.Label();
            this.chkHalfExpGoldReq = new System.Windows.Forms.CheckBox();
            this.radSlightIntensity = new System.Windows.Forms.RadioButton();
            this.radModerateIntensity = new System.Windows.Forms.RadioButton();
            this.radHeavyIntensity = new System.Windows.Forms.RadioButton();
            this.radInsaneIntensity = new System.Windows.Forms.RadioButton();
            this.btnRandomize = new System.Windows.Forms.Button();
            this.lblIntensityDesc = new System.Windows.Forms.Label();
            this.btnCompare = new System.Windows.Forms.Button();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkDoubleXP = new System.Windows.Forms.CheckBox();
            this.optNoIntensity = new System.Windows.Forms.RadioButton();
            this.btnCompareBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompare = new System.Windows.Forms.TextBox();
            this.btnNewSeed = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDefault1 = new System.Windows.Forms.TextBox();
            this.txtDefault4 = new System.Windows.Forms.TextBox();
            this.txtDefault5 = new System.Windows.Forms.TextBox();
            this.txtDefault6 = new System.Windows.Forms.TextBox();
            this.txtDefault7 = new System.Windows.Forms.TextBox();
            this.txtDefault8 = new System.Windows.Forms.TextBox();
            this.txtDefault9 = new System.Windows.Forms.TextBox();
            this.txtDefault12 = new System.Windows.Forms.TextBox();
            this.txtDefault11 = new System.Windows.Forms.TextBox();
            this.txtDefault10 = new System.Windows.Forms.TextBox();
            this.txtDefault3 = new System.Windows.Forms.TextBox();
            this.txtDefault2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(122, 23);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.Leave += new System.EventHandler(this.txtFileName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "DW3 ROM Image";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(448, 21);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "SHA1 Checksum";
            // 
            // lblSHAChecksum
            // 
            this.lblSHAChecksum.AutoSize = true;
            this.lblSHAChecksum.Location = new System.Drawing.Point(119, 78);
            this.lblSHAChecksum.Name = "lblSHAChecksum";
            this.lblSHAChecksum.Size = new System.Drawing.Size(247, 13);
            this.lblSHAChecksum.TabIndex = 4;
            this.lblSHAChecksum.Text = "????????????????????????????????????????";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Required";
            // 
            // lblReqChecksum
            // 
            this.lblReqChecksum.AutoSize = true;
            this.lblReqChecksum.Location = new System.Drawing.Point(119, 102);
            this.lblReqChecksum.Name = "lblReqChecksum";
            this.lblReqChecksum.Size = new System.Drawing.Size(244, 13);
            this.lblReqChecksum.TabIndex = 6;
            this.lblReqChecksum.Text = "a867549bad1cba4cd6f6dd51743e78596b982bd8";
            // 
            // chkHalfExpGoldReq
            // 
            this.chkHalfExpGoldReq.AutoSize = true;
            this.chkHalfExpGoldReq.Checked = true;
            this.chkHalfExpGoldReq.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHalfExpGoldReq.Location = new System.Drawing.Point(12, 129);
            this.chkHalfExpGoldReq.Name = "chkHalfExpGoldReq";
            this.chkHalfExpGoldReq.Size = new System.Drawing.Size(357, 17);
            this.chkHalfExpGoldReq.TabIndex = 5;
            this.chkHalfExpGoldReq.Text = "Increase experience gain by 133% and half gold requirements for items";
            this.chkHalfExpGoldReq.UseVisualStyleBackColor = true;
            // 
            // radSlightIntensity
            // 
            this.radSlightIntensity.AutoSize = true;
            this.radSlightIntensity.Enabled = false;
            this.radSlightIntensity.Location = new System.Drawing.Point(70, 306);
            this.radSlightIntensity.Name = "radSlightIntensity";
            this.radSlightIntensity.Size = new System.Drawing.Size(51, 17);
            this.radSlightIntensity.TabIndex = 22;
            this.radSlightIntensity.Text = "Slight";
            this.radSlightIntensity.UseVisualStyleBackColor = true;
            this.radSlightIntensity.CheckedChanged += new System.EventHandler(this.radSlightIntensity_CheckedChanged);
            // 
            // radModerateIntensity
            // 
            this.radModerateIntensity.AutoSize = true;
            this.radModerateIntensity.Enabled = false;
            this.radModerateIntensity.Location = new System.Drawing.Point(128, 306);
            this.radModerateIntensity.Name = "radModerateIntensity";
            this.radModerateIntensity.Size = new System.Drawing.Size(70, 17);
            this.radModerateIntensity.TabIndex = 23;
            this.radModerateIntensity.Text = "Moderate";
            this.radModerateIntensity.UseVisualStyleBackColor = true;
            this.radModerateIntensity.CheckedChanged += new System.EventHandler(this.radModerateIntensity_CheckedChanged);
            // 
            // radHeavyIntensity
            // 
            this.radHeavyIntensity.AutoSize = true;
            this.radHeavyIntensity.Enabled = false;
            this.radHeavyIntensity.Location = new System.Drawing.Point(205, 306);
            this.radHeavyIntensity.Name = "radHeavyIntensity";
            this.radHeavyIntensity.Size = new System.Drawing.Size(56, 17);
            this.radHeavyIntensity.TabIndex = 24;
            this.radHeavyIntensity.Text = "Heavy";
            this.radHeavyIntensity.UseVisualStyleBackColor = true;
            this.radHeavyIntensity.CheckedChanged += new System.EventHandler(this.radHeavyIntensity_CheckedChanged);
            // 
            // radInsaneIntensity
            // 
            this.radInsaneIntensity.AutoSize = true;
            this.radInsaneIntensity.Checked = true;
            this.radInsaneIntensity.Location = new System.Drawing.Point(268, 306);
            this.radInsaneIntensity.Name = "radInsaneIntensity";
            this.radInsaneIntensity.Size = new System.Drawing.Size(68, 17);
            this.radInsaneIntensity.TabIndex = 25;
            this.radInsaneIntensity.TabStop = true;
            this.radInsaneIntensity.Text = "INSANE!";
            this.radInsaneIntensity.UseVisualStyleBackColor = true;
            this.radInsaneIntensity.CheckedChanged += new System.EventHandler(this.radInsaneIntensity_CheckedChanged);
            // 
            // btnRandomize
            // 
            this.btnRandomize.Location = new System.Drawing.Point(448, 306);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(75, 23);
            this.btnRandomize.TabIndex = 26;
            this.btnRandomize.Text = "Randomize!";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // lblIntensityDesc
            // 
            this.lblIntensityDesc.AutoSize = true;
            this.lblIntensityDesc.Location = new System.Drawing.Point(11, 337);
            this.lblIntensityDesc.MaximumSize = new System.Drawing.Size(500, 0);
            this.lblIntensityDesc.Name = "lblIntensityDesc";
            this.lblIntensityDesc.Size = new System.Drawing.Size(84, 13);
            this.lblIntensityDesc.TabIndex = 15;
            this.lblIntensityDesc.Text = "Description here";
            // 
            // btnCompare
            // 
            this.btnCompare.Location = new System.Drawing.Point(448, 76);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // txtSeed
            // 
            this.txtSeed.Location = new System.Drawing.Point(69, 186);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(100, 20);
            this.txtSeed.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Seed";
            // 
            // chkDoubleXP
            // 
            this.chkDoubleXP.AutoSize = true;
            this.chkDoubleXP.Checked = true;
            this.chkDoubleXP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDoubleXP.Location = new System.Drawing.Point(12, 152);
            this.chkDoubleXP.Name = "chkDoubleXP";
            this.chkDoubleXP.Size = new System.Drawing.Size(364, 17);
            this.chkDoubleXP.TabIndex = 6;
            this.chkDoubleXP.Text = "XP +100% for all monsters EXCEPT metal slime/babble, less encounters";
            this.chkDoubleXP.UseVisualStyleBackColor = true;
            // 
            // optNoIntensity
            // 
            this.optNoIntensity.AutoSize = true;
            this.optNoIntensity.Location = new System.Drawing.Point(12, 306);
            this.optNoIntensity.Name = "optNoIntensity";
            this.optNoIntensity.Size = new System.Drawing.Size(51, 17);
            this.optNoIntensity.TabIndex = 21;
            this.optNoIntensity.Text = "None";
            this.optNoIntensity.UseVisualStyleBackColor = true;
            // 
            // btnCompareBrowse
            // 
            this.btnCompareBrowse.Location = new System.Drawing.Point(448, 47);
            this.btnCompareBrowse.Name = "btnCompareBrowse";
            this.btnCompareBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnCompareBrowse.TabIndex = 3;
            this.btnCompareBrowse.Text = "Browse";
            this.btnCompareBrowse.UseVisualStyleBackColor = true;
            this.btnCompareBrowse.Click += new System.EventHandler(this.btnCompareBrowse_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Comparison Image";
            // 
            // txtCompare
            // 
            this.txtCompare.Location = new System.Drawing.Point(122, 49);
            this.txtCompare.Name = "txtCompare";
            this.txtCompare.Size = new System.Drawing.Size(320, 20);
            this.txtCompare.TabIndex = 2;
            // 
            // btnNewSeed
            // 
            this.btnNewSeed.Location = new System.Drawing.Point(186, 184);
            this.btnNewSeed.Name = "btnNewSeed";
            this.btnNewSeed.Size = new System.Drawing.Size(75, 23);
            this.btnNewSeed.TabIndex = 8;
            this.btnNewSeed.Text = "New Seed";
            this.btnNewSeed.UseVisualStyleBackColor = true;
            this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 217);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "Default Names";
            // 
            // txtDefault1
            // 
            this.txtDefault1.Location = new System.Drawing.Point(95, 216);
            this.txtDefault1.MaxLength = 8;
            this.txtDefault1.Name = "txtDefault1";
            this.txtDefault1.Size = new System.Drawing.Size(81, 20);
            this.txtDefault1.TabIndex = 9;
            // 
            // txtDefault4
            // 
            this.txtDefault4.Location = new System.Drawing.Point(198, 216);
            this.txtDefault4.MaxLength = 8;
            this.txtDefault4.Name = "txtDefault4";
            this.txtDefault4.Size = new System.Drawing.Size(81, 20);
            this.txtDefault4.TabIndex = 12;
            // 
            // txtDefault5
            // 
            this.txtDefault5.Location = new System.Drawing.Point(198, 242);
            this.txtDefault5.MaxLength = 8;
            this.txtDefault5.Name = "txtDefault5";
            this.txtDefault5.Size = new System.Drawing.Size(81, 20);
            this.txtDefault5.TabIndex = 13;
            // 
            // txtDefault6
            // 
            this.txtDefault6.Location = new System.Drawing.Point(198, 269);
            this.txtDefault6.MaxLength = 8;
            this.txtDefault6.Name = "txtDefault6";
            this.txtDefault6.Size = new System.Drawing.Size(81, 20);
            this.txtDefault6.TabIndex = 14;
            // 
            // txtDefault7
            // 
            this.txtDefault7.Location = new System.Drawing.Point(301, 216);
            this.txtDefault7.MaxLength = 8;
            this.txtDefault7.Name = "txtDefault7";
            this.txtDefault7.Size = new System.Drawing.Size(81, 20);
            this.txtDefault7.TabIndex = 15;
            // 
            // txtDefault8
            // 
            this.txtDefault8.Location = new System.Drawing.Point(301, 242);
            this.txtDefault8.MaxLength = 8;
            this.txtDefault8.Name = "txtDefault8";
            this.txtDefault8.Size = new System.Drawing.Size(81, 20);
            this.txtDefault8.TabIndex = 16;
            // 
            // txtDefault9
            // 
            this.txtDefault9.Location = new System.Drawing.Point(300, 269);
            this.txtDefault9.MaxLength = 8;
            this.txtDefault9.Name = "txtDefault9";
            this.txtDefault9.Size = new System.Drawing.Size(81, 20);
            this.txtDefault9.TabIndex = 17;
            // 
            // txtDefault12
            // 
            this.txtDefault12.Location = new System.Drawing.Point(407, 269);
            this.txtDefault12.MaxLength = 8;
            this.txtDefault12.Name = "txtDefault12";
            this.txtDefault12.Size = new System.Drawing.Size(81, 20);
            this.txtDefault12.TabIndex = 20;
            // 
            // txtDefault11
            // 
            this.txtDefault11.Location = new System.Drawing.Point(407, 242);
            this.txtDefault11.MaxLength = 8;
            this.txtDefault11.Name = "txtDefault11";
            this.txtDefault11.Size = new System.Drawing.Size(81, 20);
            this.txtDefault11.TabIndex = 19;
            // 
            // txtDefault10
            // 
            this.txtDefault10.Location = new System.Drawing.Point(407, 216);
            this.txtDefault10.MaxLength = 8;
            this.txtDefault10.Name = "txtDefault10";
            this.txtDefault10.Size = new System.Drawing.Size(81, 20);
            this.txtDefault10.TabIndex = 18;
            // 
            // txtDefault3
            // 
            this.txtDefault3.Location = new System.Drawing.Point(95, 269);
            this.txtDefault3.MaxLength = 8;
            this.txtDefault3.Name = "txtDefault3";
            this.txtDefault3.Size = new System.Drawing.Size(81, 20);
            this.txtDefault3.TabIndex = 11;
            // 
            // txtDefault2
            // 
            this.txtDefault2.Location = new System.Drawing.Point(95, 242);
            this.txtDefault2.MaxLength = 8;
            this.txtDefault2.Name = "txtDefault2";
            this.txtDefault2.Size = new System.Drawing.Size(81, 20);
            this.txtDefault2.TabIndex = 10;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(537, 450);
            this.Controls.Add(this.txtDefault2);
            this.Controls.Add(this.txtDefault3);
            this.Controls.Add(this.txtDefault10);
            this.Controls.Add(this.txtDefault11);
            this.Controls.Add(this.txtDefault12);
            this.Controls.Add(this.txtDefault9);
            this.Controls.Add(this.txtDefault8);
            this.Controls.Add(this.txtDefault7);
            this.Controls.Add(this.txtDefault6);
            this.Controls.Add(this.txtDefault5);
            this.Controls.Add(this.txtDefault4);
            this.Controls.Add(this.txtDefault1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnNewSeed);
            this.Controls.Add(this.btnCompareBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCompare);
            this.Controls.Add(this.optNoIntensity);
            this.Controls.Add(this.chkDoubleXP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.lblIntensityDesc);
            this.Controls.Add(this.btnRandomize);
            this.Controls.Add(this.radInsaneIntensity);
            this.Controls.Add(this.radHeavyIntensity);
            this.Controls.Add(this.radModerateIntensity);
            this.Controls.Add(this.radSlightIntensity);
            this.Controls.Add(this.chkHalfExpGoldReq);
            this.Controls.Add(this.lblReqChecksum);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblSHAChecksum);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtFileName);
            this.Name = "Form1";
            this.Text = "Dragon Warrior 3 Randomizer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSHAChecksum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblReqChecksum;
        private System.Windows.Forms.CheckBox chkHalfExpGoldReq;
        private System.Windows.Forms.RadioButton radSlightIntensity;
        private System.Windows.Forms.RadioButton radModerateIntensity;
        private System.Windows.Forms.RadioButton radHeavyIntensity;
        private System.Windows.Forms.RadioButton radInsaneIntensity;
        private System.Windows.Forms.Button btnRandomize;
        private System.Windows.Forms.Label lblIntensityDesc;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkDoubleXP;
        private System.Windows.Forms.RadioButton optNoIntensity;
        private System.Windows.Forms.Button btnNewSeed;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCompareBrowse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtDefault1;
        private System.Windows.Forms.TextBox txtDefault4;
        private System.Windows.Forms.TextBox txtDefault5;
        private System.Windows.Forms.TextBox txtDefault6;
        private System.Windows.Forms.TextBox txtDefault7;
        private System.Windows.Forms.TextBox txtDefault8;
        private System.Windows.Forms.TextBox txtDefault9;
        private System.Windows.Forms.TextBox txtDefault12;
        private System.Windows.Forms.TextBox txtDefault11;
        private System.Windows.Forms.TextBox txtDefault10;
        private System.Windows.Forms.TextBox txtDefault3;
        private System.Windows.Forms.TextBox txtDefault2;
    }
}

