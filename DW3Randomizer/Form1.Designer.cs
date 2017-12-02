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
            this.components = new System.ComponentModel.Container();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSHAChecksum = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblReqChecksum = new System.Windows.Forms.Label();
            this.btnRandomize = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.txtSeed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnCompareBrowse = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCompare = new System.Windows.Forms.TextBox();
            this.btnNewSeed = new System.Windows.Forms.Button();
            this.lblIntensityDesc = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.grpMonsterStat = new System.Windows.Forms.GroupBox();
            this.optMonsterSilly = new System.Windows.Forms.RadioButton();
            this.optMonsterHeavy = new System.Windows.Forms.RadioButton();
            this.optMonsterMedium = new System.Windows.Forms.RadioButton();
            this.optMonsterLight = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkFourJobFiesta = new System.Windows.Forms.CheckBox();
            this.chkRandomizeGP = new System.Windows.Forms.CheckBox();
            this.chkRandomizeXP = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cboEncounterRate = new System.Windows.Forms.ComboBox();
            this.cboGoldReq = new System.Windows.Forms.ComboBox();
            this.cboExpGains = new System.Windows.Forms.ComboBox();
            this.chkSpeedText = new System.Windows.Forms.CheckBox();
            this.chkFasterBattles = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkRandomizeMap = new System.Windows.Forms.CheckBox();
            this.chkRandWhoCanEquip = new System.Windows.Forms.CheckBox();
            this.chkRandItemEffects = new System.Windows.Forms.CheckBox();
            this.chkSmallMap = new System.Windows.Forms.CheckBox();
            this.chkRandStatGains = new System.Windows.Forms.CheckBox();
            this.chkRandTreasures = new System.Windows.Forms.CheckBox();
            this.chkRandSpellStrength = new System.Windows.Forms.CheckBox();
            this.chkRandSpellLearning = new System.Windows.Forms.CheckBox();
            this.chkRandEquip = new System.Windows.Forms.CheckBox();
            this.chkRandMonsterZones = new System.Windows.Forms.CheckBox();
            this.chkRandEnemyPatterns = new System.Windows.Forms.CheckBox();
            this.chkRandStores = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.txtDefault2 = new System.Windows.Forms.TextBox();
            this.txtDefault3 = new System.Windows.Forms.TextBox();
            this.txtDefault10 = new System.Windows.Forms.TextBox();
            this.txtDefault11 = new System.Windows.Forms.TextBox();
            this.txtDefault12 = new System.Windows.Forms.TextBox();
            this.txtDefault9 = new System.Windows.Forms.TextBox();
            this.txtDefault8 = new System.Windows.Forms.TextBox();
            this.txtDefault7 = new System.Windows.Forms.TextBox();
            this.txtDefault6 = new System.Windows.Forms.TextBox();
            this.txtDefault5 = new System.Windows.Forms.TextBox();
            this.txtDefault4 = new System.Windows.Forms.TextBox();
            this.txtDefault1 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtFlags = new System.Windows.Forms.TextBox();
            this.adjustments = new System.Windows.Forms.ToolTip(this.components);
            this.chkRemoveParryFight = new System.Windows.Forms.CheckBox();
            this.grpMonsterStat.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(119, 23);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(320, 20);
            this.txtFileName.TabIndex = 0;
            this.txtFileName.Leave += new System.EventHandler(this.txtFileName_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
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
            this.label2.Location = new System.Drawing.Point(17, 78);
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
            this.label4.Location = new System.Drawing.Point(17, 102);
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
            // btnRandomize
            // 
            this.btnRandomize.Location = new System.Drawing.Point(449, 369);
            this.btnRandomize.Name = "btnRandomize";
            this.btnRandomize.Size = new System.Drawing.Size(75, 23);
            this.btnRandomize.TabIndex = 26;
            this.btnRandomize.Text = "Randomize!";
            this.btnRandomize.UseVisualStyleBackColor = true;
            this.btnRandomize.Click += new System.EventHandler(this.btnRandomize_Click);
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
            this.txtSeed.Location = new System.Drawing.Point(119, 155);
            this.txtSeed.Name = "txtSeed";
            this.txtSeed.Size = new System.Drawing.Size(100, 20);
            this.txtSeed.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 13);
            this.label3.TabIndex = 20;
            this.label3.Text = "Seed";
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
            this.label5.Location = new System.Drawing.Point(17, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Comparison Image";
            // 
            // txtCompare
            // 
            this.txtCompare.Location = new System.Drawing.Point(119, 49);
            this.txtCompare.Name = "txtCompare";
            this.txtCompare.Size = new System.Drawing.Size(320, 20);
            this.txtCompare.TabIndex = 2;
            // 
            // btnNewSeed
            // 
            this.btnNewSeed.Location = new System.Drawing.Point(231, 153);
            this.btnNewSeed.Name = "btnNewSeed";
            this.btnNewSeed.Size = new System.Drawing.Size(75, 23);
            this.btnNewSeed.TabIndex = 8;
            this.btnNewSeed.Text = "New Seed";
            this.btnNewSeed.UseVisualStyleBackColor = true;
            this.btnNewSeed.Click += new System.EventHandler(this.btnNewSeed_Click);
            // 
            // lblIntensityDesc
            // 
            this.lblIntensityDesc.Location = new System.Drawing.Point(13, 374);
            this.lblIntensityDesc.Name = "lblIntensityDesc";
            this.lblIntensityDesc.Size = new System.Drawing.Size(400, 48);
            this.lblIntensityDesc.TabIndex = 35;
            this.lblIntensityDesc.Text = "a867549bad1cba4cd6f6dd51743e78596b982bd8";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 129);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Random Level";
            // 
            // grpMonsterStat
            // 
            this.grpMonsterStat.Controls.Add(this.optMonsterSilly);
            this.grpMonsterStat.Controls.Add(this.optMonsterHeavy);
            this.grpMonsterStat.Controls.Add(this.optMonsterMedium);
            this.grpMonsterStat.Controls.Add(this.optMonsterLight);
            this.grpMonsterStat.Location = new System.Drawing.Point(119, 118);
            this.grpMonsterStat.Name = "grpMonsterStat";
            this.grpMonsterStat.Size = new System.Drawing.Size(271, 30);
            this.grpMonsterStat.TabIndex = 38;
            this.grpMonsterStat.TabStop = false;
            // 
            // optMonsterSilly
            // 
            this.optMonsterSilly.AutoSize = true;
            this.optMonsterSilly.Location = new System.Drawing.Point(63, 8);
            this.optMonsterSilly.Name = "optMonsterSilly";
            this.optMonsterSilly.Size = new System.Drawing.Size(43, 17);
            this.optMonsterSilly.TabIndex = 20;
            this.optMonsterSilly.Text = "Silly";
            this.optMonsterSilly.UseVisualStyleBackColor = true;
            this.optMonsterSilly.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // optMonsterHeavy
            // 
            this.optMonsterHeavy.AutoSize = true;
            this.optMonsterHeavy.Location = new System.Drawing.Point(192, 8);
            this.optMonsterHeavy.Name = "optMonsterHeavy";
            this.optMonsterHeavy.Size = new System.Drawing.Size(71, 17);
            this.optMonsterHeavy.TabIndex = 19;
            this.optMonsterHeavy.Text = "Ludicrous";
            this.optMonsterHeavy.UseVisualStyleBackColor = true;
            this.optMonsterHeavy.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // optMonsterMedium
            // 
            this.optMonsterMedium.AutoSize = true;
            this.optMonsterMedium.Location = new System.Drawing.Point(112, 8);
            this.optMonsterMedium.Name = "optMonsterMedium";
            this.optMonsterMedium.Size = new System.Drawing.Size(74, 17);
            this.optMonsterMedium.TabIndex = 18;
            this.optMonsterMedium.Text = "Ridiculous";
            this.optMonsterMedium.UseVisualStyleBackColor = true;
            this.optMonsterMedium.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // optMonsterLight
            // 
            this.optMonsterLight.AutoSize = true;
            this.optMonsterLight.Checked = true;
            this.optMonsterLight.Location = new System.Drawing.Point(9, 8);
            this.optMonsterLight.Name = "optMonsterLight";
            this.optMonsterLight.Size = new System.Drawing.Size(48, 17);
            this.optMonsterLight.TabIndex = 17;
            this.optMonsterLight.TabStop = true;
            this.optMonsterLight.Text = "Light";
            this.optMonsterLight.UseVisualStyleBackColor = true;
            this.optMonsterLight.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(16, 181);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(541, 164);
            this.tabControl1.TabIndex = 41;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkRemoveParryFight);
            this.tabPage1.Controls.Add(this.chkFourJobFiesta);
            this.tabPage1.Controls.Add(this.chkRandomizeGP);
            this.tabPage1.Controls.Add(this.chkRandomizeXP);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.cboEncounterRate);
            this.tabPage1.Controls.Add(this.cboGoldReq);
            this.tabPage1.Controls.Add(this.cboExpGains);
            this.tabPage1.Controls.Add(this.chkSpeedText);
            this.tabPage1.Controls.Add(this.chkFasterBattles);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(533, 138);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Adjustments";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkFourJobFiesta
            // 
            this.chkFourJobFiesta.AutoSize = true;
            this.chkFourJobFiesta.Location = new System.Drawing.Point(370, 52);
            this.chkFourJobFiesta.Name = "chkFourJobFiesta";
            this.chkFourJobFiesta.Size = new System.Drawing.Size(157, 17);
            this.chkFourJobFiesta.TabIndex = 19;
            this.chkFourJobFiesta.Text = "Four Job Fiesta adjustments";
            this.adjustments.SetToolTip(this.chkFourJobFiesta, "Allows the hero to be removed from the party, the hero to change classes, and any" +
        " hero to become a sage.");
            this.chkFourJobFiesta.UseVisualStyleBackColor = true;
            this.chkFourJobFiesta.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandomizeGP
            // 
            this.chkRandomizeGP.AutoSize = true;
            this.chkRandomizeGP.Checked = true;
            this.chkRandomizeGP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandomizeGP.Location = new System.Drawing.Point(211, 29);
            this.chkRandomizeGP.Name = "chkRandomizeGP";
            this.chkRandomizeGP.Size = new System.Drawing.Size(97, 17);
            this.chkRandomizeGP.TabIndex = 18;
            this.chkRandomizeGP.Text = "Randomize GP";
            this.chkRandomizeGP.UseVisualStyleBackColor = true;
            this.chkRandomizeGP.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandomizeXP
            // 
            this.chkRandomizeXP.AutoSize = true;
            this.chkRandomizeXP.Checked = true;
            this.chkRandomizeXP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandomizeXP.Location = new System.Drawing.Point(211, 6);
            this.chkRandomizeXP.Name = "chkRandomizeXP";
            this.chkRandomizeXP.Size = new System.Drawing.Size(96, 17);
            this.chkRandomizeXP.TabIndex = 17;
            this.chkRandomizeXP.Text = "Randomize XP";
            this.chkRandomizeXP.UseVisualStyleBackColor = true;
            this.chkRandomizeXP.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 60);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(82, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Encounter Rate";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 35);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Gold Gains";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Experience Gains";
            // 
            // cboEncounterRate
            // 
            this.cboEncounterRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEncounterRate.FormattingEnabled = true;
            this.cboEncounterRate.Items.AddRange(new object[] {
            "400%",
            "300%",
            "200%",
            "150%",
            "100%",
            "75%",
            "50%",
            "25%"});
            this.cboEncounterRate.Location = new System.Drawing.Point(109, 55);
            this.cboEncounterRate.Margin = new System.Windows.Forms.Padding(2);
            this.cboEncounterRate.Name = "cboEncounterRate";
            this.cboEncounterRate.Size = new System.Drawing.Size(82, 21);
            this.cboEncounterRate.TabIndex = 13;
            this.cboEncounterRate.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
            // 
            // cboGoldReq
            // 
            this.cboGoldReq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboGoldReq.FormattingEnabled = true;
            this.cboGoldReq.Items.AddRange(new object[] {
            "200%",
            "150%",
            "100%",
            "50%"});
            this.cboGoldReq.Location = new System.Drawing.Point(109, 30);
            this.cboGoldReq.Margin = new System.Windows.Forms.Padding(2);
            this.cboGoldReq.Name = "cboGoldReq";
            this.cboGoldReq.Size = new System.Drawing.Size(82, 21);
            this.cboGoldReq.TabIndex = 12;
            this.cboGoldReq.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
            // 
            // cboExpGains
            // 
            this.cboExpGains.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExpGains.FormattingEnabled = true;
            this.cboExpGains.Items.AddRange(new object[] {
            "500%",
            "400%",
            "300%",
            "200%",
            "150%",
            "100%",
            "50%",
            "25%"});
            this.cboExpGains.Location = new System.Drawing.Point(109, 5);
            this.cboExpGains.Margin = new System.Windows.Forms.Padding(2);
            this.cboExpGains.Name = "cboExpGains";
            this.cboExpGains.Size = new System.Drawing.Size(82, 21);
            this.cboExpGains.TabIndex = 11;
            this.cboExpGains.SelectedIndexChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkSpeedText
            // 
            this.chkSpeedText.AutoSize = true;
            this.chkSpeedText.Checked = true;
            this.chkSpeedText.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSpeedText.Location = new System.Drawing.Point(370, 29);
            this.chkSpeedText.Name = "chkSpeedText";
            this.chkSpeedText.Size = new System.Drawing.Size(115, 17);
            this.chkSpeedText.TabIndex = 10;
            this.chkSpeedText.Text = "Speedy Text Hack";
            this.chkSpeedText.UseVisualStyleBackColor = true;
            this.chkSpeedText.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkFasterBattles
            // 
            this.chkFasterBattles.AutoSize = true;
            this.chkFasterBattles.Checked = true;
            this.chkFasterBattles.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFasterBattles.Location = new System.Drawing.Point(370, 6);
            this.chkFasterBattles.Name = "chkFasterBattles";
            this.chkFasterBattles.Size = new System.Drawing.Size(137, 17);
            this.chkFasterBattles.TabIndex = 9;
            this.chkFasterBattles.Text = "Increased Battle Speed";
            this.chkFasterBattles.UseVisualStyleBackColor = true;
            this.chkFasterBattles.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkRandomizeMap);
            this.tabPage2.Controls.Add(this.chkRandWhoCanEquip);
            this.tabPage2.Controls.Add(this.chkRandItemEffects);
            this.tabPage2.Controls.Add(this.chkSmallMap);
            this.tabPage2.Controls.Add(this.chkRandStatGains);
            this.tabPage2.Controls.Add(this.chkRandTreasures);
            this.tabPage2.Controls.Add(this.chkRandSpellStrength);
            this.tabPage2.Controls.Add(this.chkRandSpellLearning);
            this.tabPage2.Controls.Add(this.chkRandEquip);
            this.tabPage2.Controls.Add(this.chkRandMonsterZones);
            this.tabPage2.Controls.Add(this.chkRandEnemyPatterns);
            this.tabPage2.Controls.Add(this.chkRandStores);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(533, 138);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Randomization";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // chkRandomizeMap
            // 
            this.chkRandomizeMap.AutoSize = true;
            this.chkRandomizeMap.Enabled = false;
            this.chkRandomizeMap.Location = new System.Drawing.Point(178, 96);
            this.chkRandomizeMap.Name = "chkRandomizeMap";
            this.chkRandomizeMap.Size = new System.Drawing.Size(102, 17);
            this.chkRandomizeMap.TabIndex = 52;
            this.chkRandomizeMap.Text = "Randomize map";
            this.chkRandomizeMap.UseVisualStyleBackColor = true;
            this.chkRandomizeMap.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandWhoCanEquip
            // 
            this.chkRandWhoCanEquip.AutoSize = true;
            this.chkRandWhoCanEquip.Checked = true;
            this.chkRandWhoCanEquip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandWhoCanEquip.Location = new System.Drawing.Point(179, 50);
            this.chkRandWhoCanEquip.Name = "chkRandWhoCanEquip";
            this.chkRandWhoCanEquip.Size = new System.Drawing.Size(152, 17);
            this.chkRandWhoCanEquip.TabIndex = 51;
            this.chkRandWhoCanEquip.Text = "Randomize who can equip";
            this.chkRandWhoCanEquip.UseVisualStyleBackColor = true;
            this.chkRandWhoCanEquip.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandItemEffects
            // 
            this.chkRandItemEffects.AutoSize = true;
            this.chkRandItemEffects.Checked = true;
            this.chkRandItemEffects.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandItemEffects.Location = new System.Drawing.Point(179, 28);
            this.chkRandItemEffects.Name = "chkRandItemEffects";
            this.chkRandItemEffects.Size = new System.Drawing.Size(136, 17);
            this.chkRandItemEffects.TabIndex = 50;
            this.chkRandItemEffects.Text = "Randomize item effects";
            this.chkRandItemEffects.UseVisualStyleBackColor = true;
            this.chkRandItemEffects.Visible = false;
            this.chkRandItemEffects.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkSmallMap
            // 
            this.chkSmallMap.AutoSize = true;
            this.chkSmallMap.Checked = true;
            this.chkSmallMap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSmallMap.Enabled = false;
            this.chkSmallMap.Location = new System.Drawing.Point(179, 120);
            this.chkSmallMap.Name = "chkSmallMap";
            this.chkSmallMap.Size = new System.Drawing.Size(125, 17);
            this.chkSmallMap.TabIndex = 49;
            this.chkSmallMap.Text = "Small Map (128x128)";
            this.chkSmallMap.UseVisualStyleBackColor = true;
            this.chkSmallMap.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandStatGains
            // 
            this.chkRandStatGains.AutoSize = true;
            this.chkRandStatGains.Checked = true;
            this.chkRandStatGains.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandStatGains.Location = new System.Drawing.Point(5, 97);
            this.chkRandStatGains.Name = "chkRandStatGains";
            this.chkRandStatGains.Size = new System.Drawing.Size(127, 17);
            this.chkRandStatGains.TabIndex = 48;
            this.chkRandStatGains.Text = "Randomize stat gains";
            this.chkRandStatGains.UseVisualStyleBackColor = true;
            this.chkRandStatGains.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandTreasures
            // 
            this.chkRandTreasures.AutoSize = true;
            this.chkRandTreasures.Checked = true;
            this.chkRandTreasures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandTreasures.Location = new System.Drawing.Point(5, 120);
            this.chkRandTreasures.Name = "chkRandTreasures";
            this.chkRandTreasures.Size = new System.Drawing.Size(125, 17);
            this.chkRandTreasures.TabIndex = 47;
            this.chkRandTreasures.Text = "Randomize treasures";
            this.chkRandTreasures.UseVisualStyleBackColor = true;
            this.chkRandTreasures.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandSpellStrength
            // 
            this.chkRandSpellStrength.AutoSize = true;
            this.chkRandSpellStrength.Checked = true;
            this.chkRandSpellStrength.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandSpellStrength.Location = new System.Drawing.Point(179, 74);
            this.chkRandSpellStrength.Name = "chkRandSpellStrength";
            this.chkRandSpellStrength.Size = new System.Drawing.Size(149, 17);
            this.chkRandSpellStrength.TabIndex = 46;
            this.chkRandSpellStrength.Text = "Randomize spell strengths";
            this.chkRandSpellStrength.UseVisualStyleBackColor = true;
            this.chkRandSpellStrength.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandSpellLearning
            // 
            this.chkRandSpellLearning.AutoSize = true;
            this.chkRandSpellLearning.Checked = true;
            this.chkRandSpellLearning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandSpellLearning.Location = new System.Drawing.Point(5, 74);
            this.chkRandSpellLearning.Name = "chkRandSpellLearning";
            this.chkRandSpellLearning.Size = new System.Drawing.Size(143, 17);
            this.chkRandSpellLearning.TabIndex = 45;
            this.chkRandSpellLearning.Text = "Randomize spell learning";
            this.chkRandSpellLearning.UseVisualStyleBackColor = true;
            this.chkRandSpellLearning.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandEquip
            // 
            this.chkRandEquip.AutoSize = true;
            this.chkRandEquip.Checked = true;
            this.chkRandEquip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandEquip.Location = new System.Drawing.Point(179, 5);
            this.chkRandEquip.Name = "chkRandEquip";
            this.chkRandEquip.Size = new System.Drawing.Size(163, 17);
            this.chkRandEquip.TabIndex = 44;
            this.chkRandEquip.Text = "Randomize equipment power";
            this.chkRandEquip.UseVisualStyleBackColor = true;
            this.chkRandEquip.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandMonsterZones
            // 
            this.chkRandMonsterZones.AutoSize = true;
            this.chkRandMonsterZones.Checked = true;
            this.chkRandMonsterZones.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandMonsterZones.Location = new System.Drawing.Point(5, 51);
            this.chkRandMonsterZones.Name = "chkRandMonsterZones";
            this.chkRandMonsterZones.Size = new System.Drawing.Size(150, 17);
            this.chkRandMonsterZones.TabIndex = 43;
            this.chkRandMonsterZones.Text = "Randomize monster zones";
            this.chkRandMonsterZones.UseVisualStyleBackColor = true;
            this.chkRandMonsterZones.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandEnemyPatterns
            // 
            this.chkRandEnemyPatterns.AutoSize = true;
            this.chkRandEnemyPatterns.Checked = true;
            this.chkRandEnemyPatterns.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandEnemyPatterns.Location = new System.Drawing.Point(5, 28);
            this.chkRandEnemyPatterns.Name = "chkRandEnemyPatterns";
            this.chkRandEnemyPatterns.Size = new System.Drawing.Size(154, 17);
            this.chkRandEnemyPatterns.TabIndex = 42;
            this.chkRandEnemyPatterns.Text = "Randomize enemy patterns";
            this.chkRandEnemyPatterns.UseVisualStyleBackColor = true;
            this.chkRandEnemyPatterns.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // chkRandStores
            // 
            this.chkRandStores.AutoSize = true;
            this.chkRandStores.Checked = true;
            this.chkRandStores.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRandStores.Location = new System.Drawing.Point(5, 5);
            this.chkRandStores.Name = "chkRandStores";
            this.chkRandStores.Size = new System.Drawing.Size(123, 17);
            this.chkRandStores.TabIndex = 41;
            this.chkRandStores.Text = "Randomize all stores";
            this.chkRandStores.UseVisualStyleBackColor = true;
            this.chkRandStores.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.txtDefault2);
            this.tabPage3.Controls.Add(this.txtDefault3);
            this.tabPage3.Controls.Add(this.txtDefault10);
            this.tabPage3.Controls.Add(this.txtDefault11);
            this.tabPage3.Controls.Add(this.txtDefault12);
            this.tabPage3.Controls.Add(this.txtDefault9);
            this.tabPage3.Controls.Add(this.txtDefault8);
            this.tabPage3.Controls.Add(this.txtDefault7);
            this.tabPage3.Controls.Add(this.txtDefault6);
            this.tabPage3.Controls.Add(this.txtDefault5);
            this.tabPage3.Controls.Add(this.txtDefault4);
            this.tabPage3.Controls.Add(this.txtDefault1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(533, 138);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Default Names";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // txtDefault2
            // 
            this.txtDefault2.Location = new System.Drawing.Point(13, 38);
            this.txtDefault2.MaxLength = 8;
            this.txtDefault2.Name = "txtDefault2";
            this.txtDefault2.Size = new System.Drawing.Size(81, 20);
            this.txtDefault2.TabIndex = 39;
            // 
            // txtDefault3
            // 
            this.txtDefault3.Location = new System.Drawing.Point(13, 65);
            this.txtDefault3.MaxLength = 8;
            this.txtDefault3.Name = "txtDefault3";
            this.txtDefault3.Size = new System.Drawing.Size(81, 20);
            this.txtDefault3.TabIndex = 40;
            // 
            // txtDefault10
            // 
            this.txtDefault10.Location = new System.Drawing.Point(325, 12);
            this.txtDefault10.MaxLength = 8;
            this.txtDefault10.Name = "txtDefault10";
            this.txtDefault10.Size = new System.Drawing.Size(81, 20);
            this.txtDefault10.TabIndex = 47;
            // 
            // txtDefault11
            // 
            this.txtDefault11.Location = new System.Drawing.Point(325, 38);
            this.txtDefault11.MaxLength = 8;
            this.txtDefault11.Name = "txtDefault11";
            this.txtDefault11.Size = new System.Drawing.Size(81, 20);
            this.txtDefault11.TabIndex = 48;
            // 
            // txtDefault12
            // 
            this.txtDefault12.Location = new System.Drawing.Point(325, 65);
            this.txtDefault12.MaxLength = 8;
            this.txtDefault12.Name = "txtDefault12";
            this.txtDefault12.Size = new System.Drawing.Size(81, 20);
            this.txtDefault12.TabIndex = 49;
            // 
            // txtDefault9
            // 
            this.txtDefault9.Location = new System.Drawing.Point(219, 65);
            this.txtDefault9.MaxLength = 8;
            this.txtDefault9.Name = "txtDefault9";
            this.txtDefault9.Size = new System.Drawing.Size(81, 20);
            this.txtDefault9.TabIndex = 46;
            // 
            // txtDefault8
            // 
            this.txtDefault8.Location = new System.Drawing.Point(220, 38);
            this.txtDefault8.MaxLength = 8;
            this.txtDefault8.Name = "txtDefault8";
            this.txtDefault8.Size = new System.Drawing.Size(81, 20);
            this.txtDefault8.TabIndex = 45;
            // 
            // txtDefault7
            // 
            this.txtDefault7.Location = new System.Drawing.Point(220, 12);
            this.txtDefault7.MaxLength = 8;
            this.txtDefault7.Name = "txtDefault7";
            this.txtDefault7.Size = new System.Drawing.Size(81, 20);
            this.txtDefault7.TabIndex = 44;
            // 
            // txtDefault6
            // 
            this.txtDefault6.Location = new System.Drawing.Point(117, 65);
            this.txtDefault6.MaxLength = 8;
            this.txtDefault6.Name = "txtDefault6";
            this.txtDefault6.Size = new System.Drawing.Size(81, 20);
            this.txtDefault6.TabIndex = 43;
            // 
            // txtDefault5
            // 
            this.txtDefault5.Location = new System.Drawing.Point(117, 38);
            this.txtDefault5.MaxLength = 8;
            this.txtDefault5.Name = "txtDefault5";
            this.txtDefault5.Size = new System.Drawing.Size(81, 20);
            this.txtDefault5.TabIndex = 42;
            // 
            // txtDefault4
            // 
            this.txtDefault4.Location = new System.Drawing.Point(117, 12);
            this.txtDefault4.MaxLength = 8;
            this.txtDefault4.Name = "txtDefault4";
            this.txtDefault4.Size = new System.Drawing.Size(81, 20);
            this.txtDefault4.TabIndex = 41;
            // 
            // txtDefault1
            // 
            this.txtDefault1.Location = new System.Drawing.Point(13, 12);
            this.txtDefault1.MaxLength = 8;
            this.txtDefault1.Name = "txtDefault1";
            this.txtDefault1.Size = new System.Drawing.Size(81, 20);
            this.txtDefault1.TabIndex = 38;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(361, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Flags";
            // 
            // txtFlags
            // 
            this.txtFlags.Location = new System.Drawing.Point(401, 157);
            this.txtFlags.Name = "txtFlags";
            this.txtFlags.Size = new System.Drawing.Size(155, 20);
            this.txtFlags.TabIndex = 42;
            this.txtFlags.Leave += new System.EventHandler(this.determineChecks);
            // 
            // chkRemoveParryFight
            // 
            this.chkRemoveParryFight.AutoSize = true;
            this.chkRemoveParryFight.Location = new System.Drawing.Point(370, 75);
            this.chkRemoveParryFight.Name = "chkRemoveParryFight";
            this.chkRemoveParryFight.Size = new System.Drawing.Size(143, 17);
            this.chkRemoveParryFight.TabIndex = 20;
            this.chkRemoveParryFight.Text = "Remove Parry/Fight Bug";
            this.adjustments.SetToolTip(this.chkRemoveParryFight, "Allows the hero to be removed from the party, the hero to change classes, and any" +
        " hero to become a sage.");
            this.chkRemoveParryFight.UseVisualStyleBackColor = true;
            this.chkRemoveParryFight.CheckedChanged += new System.EventHandler(this.determineFlags);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 428);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtFlags);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.grpMonsterStat);
            this.Controls.Add(this.lblIntensityDesc);
            this.Controls.Add(this.btnNewSeed);
            this.Controls.Add(this.btnCompareBrowse);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCompare);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeed);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnRandomize);
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
            this.grpMonsterStat.ResumeLayout(false);
            this.grpMonsterStat.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
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
        private System.Windows.Forms.Button btnRandomize;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.TextBox txtSeed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnNewSeed;
        private System.Windows.Forms.TextBox txtCompare;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCompareBrowse;
        private System.Windows.Forms.Label lblIntensityDesc;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox grpMonsterStat;
        private System.Windows.Forms.RadioButton optMonsterSilly;
        private System.Windows.Forms.RadioButton optMonsterHeavy;
        private System.Windows.Forms.RadioButton optMonsterMedium;
        private System.Windows.Forms.RadioButton optMonsterLight;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkSmallMap;
        private System.Windows.Forms.CheckBox chkRandStatGains;
        private System.Windows.Forms.CheckBox chkRandTreasures;
        private System.Windows.Forms.CheckBox chkRandSpellStrength;
        private System.Windows.Forms.CheckBox chkRandSpellLearning;
        private System.Windows.Forms.CheckBox chkRandEquip;
        private System.Windows.Forms.CheckBox chkRandMonsterZones;
        private System.Windows.Forms.CheckBox chkRandEnemyPatterns;
        private System.Windows.Forms.CheckBox chkRandStores;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtDefault2;
        private System.Windows.Forms.TextBox txtDefault3;
        private System.Windows.Forms.TextBox txtDefault10;
        private System.Windows.Forms.TextBox txtDefault11;
        private System.Windows.Forms.TextBox txtDefault12;
        private System.Windows.Forms.TextBox txtDefault9;
        private System.Windows.Forms.TextBox txtDefault8;
        private System.Windows.Forms.TextBox txtDefault7;
        private System.Windows.Forms.TextBox txtDefault6;
        private System.Windows.Forms.TextBox txtDefault5;
        private System.Windows.Forms.TextBox txtDefault4;
        private System.Windows.Forms.TextBox txtDefault1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtFlags;
        private System.Windows.Forms.CheckBox chkSpeedText;
        private System.Windows.Forms.CheckBox chkFasterBattles;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cboEncounterRate;
        private System.Windows.Forms.ComboBox cboGoldReq;
        private System.Windows.Forms.ComboBox cboExpGains;
        private System.Windows.Forms.CheckBox chkRandomizeGP;
        private System.Windows.Forms.CheckBox chkRandomizeXP;
        private System.Windows.Forms.CheckBox chkRandWhoCanEquip;
        private System.Windows.Forms.CheckBox chkRandItemEffects;
        private System.Windows.Forms.CheckBox chkRandomizeMap;
        private System.Windows.Forms.CheckBox chkFourJobFiesta;
        private System.Windows.Forms.ToolTip adjustments;
        private System.Windows.Forms.CheckBox chkRemoveParryFight;
    }
}

