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
            this.chkHalfExpGoldReq.TabIndex = 6;
            this.chkHalfExpGoldReq.Text = "Increase experience gain by 133% and half gold requirements for items";
            this.chkHalfExpGoldReq.UseVisualStyleBackColor = true;
            // 
            // radSlightIntensity
            // 
            this.radSlightIntensity.AutoSize = true;
            this.radSlightIntensity.Location = new System.Drawing.Point(70, 219);
            this.radSlightIntensity.Name = "radSlightIntensity";
            this.radSlightIntensity.Size = new System.Drawing.Size(51, 17);
            this.radSlightIntensity.TabIndex = 11;
            this.radSlightIntensity.Text = "Slight";
            this.radSlightIntensity.UseVisualStyleBackColor = true;
            this.radSlightIntensity.CheckedChanged += new System.EventHandler(this.radSlightIntensity_CheckedChanged);
            // 
            // radModerateIntensity
            // 
            this.radModerateIntensity.AutoSize = true;
            this.radModerateIntensity.Location = new System.Drawing.Point(128, 219);
            this.radModerateIntensity.Name = "radModerateIntensity";
            this.radModerateIntensity.Size = new System.Drawing.Size(70, 17);
            this.radModerateIntensity.TabIndex = 12;
            this.radModerateIntensity.Text = "Moderate";
            this.radModerateIntensity.UseVisualStyleBackColor = true;
            this.radModerateIntensity.CheckedChanged += new System.EventHandler(this.radModerateIntensity_CheckedChanged);
            // 
            // radHeavyIntensity
            // 
            this.radHeavyIntensity.AutoSize = true;
            this.radHeavyIntensity.Checked = true;
            this.radHeavyIntensity.Location = new System.Drawing.Point(205, 219);
            this.radHeavyIntensity.Name = "radHeavyIntensity";
            this.radHeavyIntensity.Size = new System.Drawing.Size(56, 17);
            this.radHeavyIntensity.TabIndex = 13;
            this.radHeavyIntensity.TabStop = true;
            this.radHeavyIntensity.Text = "Heavy";
            this.radHeavyIntensity.UseVisualStyleBackColor = true;
            this.radHeavyIntensity.CheckedChanged += new System.EventHandler(this.radHeavyIntensity_CheckedChanged);
            // 
            // radInsaneIntensity
            // 
            this.radInsaneIntensity.AutoSize = true;
            this.radInsaneIntensity.Location = new System.Drawing.Point(268, 219);
            this.radInsaneIntensity.Name = "radInsaneIntensity";
            this.radInsaneIntensity.Size = new System.Drawing.Size(68, 17);
            this.radInsaneIntensity.TabIndex = 14;
            this.radInsaneIntensity.Text = "INSANE!";
            this.radInsaneIntensity.UseVisualStyleBackColor = true;
            this.radInsaneIntensity.CheckedChanged += new System.EventHandler(this.radInsaneIntensity_CheckedChanged);
            // 
            // btnRandomize
            // 
            this.btnRandomize.Location = new System.Drawing.Point(448, 219);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(75, 23);
            this.btnRandomize.TabIndex = 15;
            this.btnRandomize.Text = "Randomize!";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
            // 
            // lblIntensityDesc
            // 
            this.lblIntensityDesc.AutoSize = true;
            this.lblIntensityDesc.Location = new System.Drawing.Point(11, 250);
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
            this.txtSeed.TabIndex = 8;
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
            this.chkDoubleXP.TabIndex = 7;
            this.chkDoubleXP.Text = "XP +100% for all monsters EXCEPT metal slime/babble, less encounters";
            this.chkDoubleXP.UseVisualStyleBackColor = true;
            // 
            // optNoIntensity
            // 
            this.optNoIntensity.AutoSize = true;
            this.optNoIntensity.Location = new System.Drawing.Point(12, 219);
            this.optNoIntensity.Name = "optNoIntensity";
            this.optNoIntensity.Size = new System.Drawing.Size(51, 17);
            this.optNoIntensity.TabIndex = 10;
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
            this.btnNewSeed.TabIndex = 9;
            this.btnNewSeed.Text = "New Seed";
            this.btnNewSeed.UseVisualStyleBackColor = true;
            this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(533, 397);
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
    }
}

