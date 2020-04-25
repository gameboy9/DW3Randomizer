using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace DW3Randomizer
{
	public partial class Form1 : Form
	{
		bool loading = true;
		byte[] romData;
		byte[] romData2;
		byte[] monsterOrder = { 0x00, 0x01, 0x68, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e,
								0x0f, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e,
								0x1f, 0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x8a, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d,
								0x2e, 0x2f, 0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d,
								0x3e, 0x3f, 0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47, 0x48, 0x49, 0x4a, 0x4b, 0x4c, 0x4d,
								0x4e, 0x4f, 0x50, 0x51, 0x52, 0x54, 0x55, 0x56, 0x57, 0x58, 0x59, 0x5a, 0x5b, 0x5c, 0x5d, 0x5e,
								0x5f, 0x60, 0x61, 0x62, 0x63, 0x64, 0x66, 0x67, 0x53, 0x69, 0x6a, 0x6b, 0x6c, 0x6d, 0x6e, 0x6f,
								0x70, 0x71, 0x72, 0x73, 0x74, 0x75, 0x76, 0x77, 0x78, 0x79, 0x7a, 0x7b, 0x7c, 0x7d, 0x7e, 0x7f,
								0x80, 0x88, 0x89, 0x65, 0x84, 0x81, 0x82, 0x83 }; // 129 normal monsters, 7 bosses.  Skip Zoma, "frozen" Zoma, Ortega
		int[,] map = new int[256, 256];
		int[,] map2 = new int[139, 158];
		int[,] island = new int[256, 256];
		int[,] island2 = new int[139, 158];
		int[,] zone = new int[16, 16];
		int[] maxIsland = new int[4];
		List<int> islands = new List<int>();

		public Form1()
		{
			InitializeComponent();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				txtFileName.Text = openFileDialog1.FileName;
				runChecksum();
			}
		}

		private void runChecksum()
		{
			try
			{
				using (var md5 = SHA1.Create())
				{
					using (var stream = File.OpenRead(txtFileName.Text))
					{
						lblSHAChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
					}
				}
			}
			catch
			{
				lblSHAChecksum.Text = "????????????????????????????????????????";
			}
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();

			try
			{
				using (TextReader reader = File.OpenText("lastFile.txt"))
				{
					txtFileName.Text = reader.ReadLine();
					txtFlags.Text = reader.ReadLine();
					determineChecks(null, null);
					txtDefault1.Text = reader.ReadLine();
					txtDefault2.Text = reader.ReadLine();
					txtDefault3.Text = reader.ReadLine();
					cboClass1.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					cboClass2.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					cboClass3.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					cboGender1.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					cboGender2.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					cboGender3.SelectedIndex = Convert.ToInt32(reader.ReadLine());
					runChecksum();
				}
			}
			catch
			{
				// ignore error
				txtDefault1.Text = "Ragnar";
				txtDefault2.Text = "Cristo";
				txtDefault3.Text = "Mara";
				cboClass1.SelectedIndex = 0;
				cboClass2.SelectedIndex = 1;
				cboClass3.SelectedIndex = 2;
				cboGender1.SelectedIndex = 0;
				cboGender2.SelectedIndex = 0;
				cboGender3.SelectedIndex = 1;
				cboEncounterRate.SelectedIndex = 4;
				cboExpGains.SelectedIndex = 5;
				cboGoldReq.SelectedIndex = 0;
			}
			btnNewSeed_Click(null, null);
			loading = false;
			determineFlags(null, null);
		}

		private void btnNewSeed_Click(object sender, EventArgs e)
		{
			txtSeed.Text = (DateTime.Now.Ticks % 2147483647).ToString();
		}

		private void btnRandomize_Click(object sender, EventArgs e)
		{
			if (lblSHAChecksum.Text != lblReqChecksum.Text)
			{
				if (MessageBox.Show("The checksum of the ROM does not match the required checksum.  Patch anyway?", "Checksum mismatch", MessageBoxButtons.YesNo) == DialogResult.No)
					return;
			}

			if (!loadRom())
				return;
			if (txtSeed.Text == "output")
			{
				textOutput();
				return;
			}
			else if (txtSeed.Text == "textGet")
			{
				textGet();
				return;
			}
			else
			{
				if (chkRandStores.Checked) forceItemSell();
				boostGP();
				superRandomize();

				boostXP();
				adjustEncounters();

				if (chkSpeedText.Checked) speedText();
				if (chkFasterBattles.Checked) battleSpeed();
				if (chkFourJobFiesta.Checked) fourJobFiesta();
			}

			if (chkNoLamiaOrbs.Checked)
			{
				romData[0x3794b] = 0xea;
				romData[0x3794c] = 0xea;
			}

			// Implement DW4 RNG so any currently known manipulations won't work.
			romData[0x3c351] = romData[0x7c351] = 0xAD;
			romData[0x3c352] = romData[0x7c352] = 0xd2;
			romData[0x3c353] = romData[0x7c353] = 0x06;
			romData[0x3c354] = romData[0x7c354] = 0x4c;
			romData[0x3c355] = romData[0x7c355] = 0x53;
			romData[0x3c356] = romData[0x7c356] = 0xc3;
			romData[0x3c357] = romData[0x7c357] = 0xf0;
			romData[0x3c358] = romData[0x7c358] = 0xfb;
			romData[0x3c359] = romData[0x7c359] = 0x4c;
			romData[0x3c35a] = romData[0x7c35a] = 0x0a;
			romData[0x3c35b] = romData[0x7c35b] = 0xc9;
			romData[0x3c35c] = romData[0x7c35c] = 0x20;
			romData[0x3c35d] = romData[0x7c35d] = 0x41;
			romData[0x3c35e] = romData[0x7c35e] = 0xc3;
			romData[0x3c35f] = romData[0x7c35f] = 0xca;
			romData[0x3c360] = romData[0x7c360] = 0xd0;
			romData[0x3c361] = romData[0x7c361] = 0xfa;
			romData[0x3c362] = romData[0x7c362] = 0x60;
			romData[0x3c363] = romData[0x7c363] = 0xe6;
			romData[0x3c364] = romData[0x7c364] = 0x1c;
			romData[0x3c365] = romData[0x7c365] = 0xcd;
			romData[0x3c366] = romData[0x7c366] = 0xd2;
			romData[0x3c367] = romData[0x7c367] = 0x06;
			romData[0x3c368] = romData[0x7c368] = 0x4c;
			romData[0x3c369] = romData[0x7c369] = 0x47;
			romData[0x3c36a] = romData[0x7c36a] = 0xc3;

			// Speed up item menu loading
			romData[0x2b0d] = 0x00;
			romData[0x2b0e] = 0xf0;
			romData[0x2b0f] = 0x01;
			romData[0x2b10] = 0x00;

			// All ROM hacks will revive ALL characters on a ColdAsACod.
			// There will be a temporary graphical error if you use less than four characters, but I'm going to leave it be.
			byte[] codData1 = { 0xa0, 0x00, // Make sure Y is 0 first.
                0xb9, 0x3c, 0x07,
				0xc9, 0x80,
				0x90, 0x03, // If less than 0x80, skip.
                0x20, 0xb2, 0xbf, // JSR to a bunch of unused code, which will have the "revive one character code" that I'm replacing.
                0xc8, 0xc8, // Increment Y twice (Y is used to revive the characters)
                0xc0, 0x08, // Compare Y with 08
                0xd0, 0xf0, // If not equal, go back to the JSR mentioned above
                0xa0, 0x00, // Set Y back to 0 to make sure the game doesn't think something is up
                0xea, 0xea, 0xea, 0xea, 0xea,
				0xea, 0xea, 0xea, 0xea, 0xea,
				0xea, 0xea }; // 12 NOPs, since I have nothing else to do.
			byte[] codData2 = { 0xa9, 0x80, // Load 80, the status for alive
                0x99, 0x3c, 0x07, // store to two status bytes
                0x99, 0x3d, 0x07,
				0xb9, 0x24, 0x07, // Load max HP
                0x99, 0x1c, 0x07, // save max HP
                0xb9, 0x25, 0x07, // second byte
                0x99, 0x1d, 0x07,
				0xb9, 0x34, 0x07, // Load max MP
                0x99, 0x2c, 0x07, // save max MP
                0xb9, 0x35, 0x07, // second byte
                0x99, 0x2d, 0x07,
				0x60 }; // end JSR

			for (int lnI = 0; lnI < codData1.Length; lnI++)
				romData[0x22b3 + lnI] = codData1[lnI];
			for (int lnI = 0; lnI < codData2.Length; lnI++)
				romData[0x3fc2 + lnI] = codData2[lnI];

			// Fix the "parry/fight" bug(as determined via gamefaqs board), via Zombero's DW3 Hardtype IPS patch.
			if (chkRemoveParryFight.Checked)
			{
				byte[] parryFightFix1 = { 0xbd, 0x9b, 0x6a, 0x29, 0xdf, 0x9d, 0x9b, 0x6a, 0x60 };
				byte[] parryFightFix2 = { 0x20, 0x70, 0xbb };
				for (int lnI = 0; lnI < parryFightFix1.Length; lnI++)
					romData[0xbb80 + lnI] = parryFightFix1[lnI];
				for (int lnI = 0; lnI < parryFightFix2.Length; lnI++)
					romData[0xa402 + lnI] = parryFightFix2[lnI];
			}

			romData[0x3cc6a] = 0x4c; // Forces a jump out of the king scolding routine, saving at least 13 seconds / party wipe.  There are graphical errors, but I'll take it!

			// Force the same three names to be selected in the opening Lucia's Eatery
			romData[0x1e9f8] = 0x29;
			romData[0x1e9f9] = 0x00;

			saveRom(true);

			// Rename the starting characters.
			for (int lnI = 0; lnI < 3; lnI++)
			{
				byte value = (byte)(lnI == 0 ? cboClass1.SelectedIndex : lnI == 1 ? cboClass2.SelectedIndex : cboClass3.SelectedIndex);
				byte gender = (byte)(lnI == 0 ? cboGender1.SelectedIndex : lnI == 1 ? cboGender2.SelectedIndex : cboGender3.SelectedIndex);
				byte intValue = (byte)(value == 0 ? 4 : value == 1 ? 2 : value == 2 ? 1 : value == 3 ? 6 : value == 4 ? 5 : value == 5 ? 7 : value == 6 ? 3 : 0);

				romData[0x1ed4f + lnI] = (byte)(intValue + (gender == 0 ? 0 : 8));
			}

			for (int lnI = 0; lnI < 3; lnI++)
			{
				string name = (lnI == 0 ? txtDefault1.Text : lnI == 1 ? txtDefault2.Text : txtDefault3.Text);
				for (int lnJ = 0; lnJ < 8; lnJ++)
				{
					romData[0x1ed52 + (8 * lnI * 4) + lnJ] = romData[0x1ed52 + (8 * lnI * 4) + 8 + lnJ] = romData[0x1ed52 + (8 * lnI * 4) + 16 + lnJ] = romData[0x1ed52 + (8 * lnI * 4) + 24 + lnJ] = 0;
					try
					{
						char character = Convert.ToChar(name.Substring(lnJ, 1));
						if (character >= 0x30 && character <= 0x39)
							romData[0x1ed52 + (8 * lnI * 4) + lnJ] = (byte)(character - 47);
						if (character >= 0x41 && character <= 0x5a)
							romData[0x1ed52 + (8 * lnI * 4) + lnJ] = (byte)(character - 28);
						if (character >= 0x61 && character <= 0x7a)
							romData[0x1ed52 + (8 * lnI * 4) + lnJ] = (byte)(character - 86);
					}
					catch
					{
						romData[0x1ed52 + (8 * lnI * 4) + lnJ] = 0; // no more characters to process - make the rest of the characters blank
					}
				}
			}

			// Remove the golden claw 100/256 encounter rate - Can't because the king won't check if you have the black pepper.
			//romData[0x185c] = 0x4c;
			//romData[0x185d] = 0x5b;
			//romData[0x185e] = 0x98;
			saveRom(false);
		}

		private void fourJobFiesta()
		{
			// Allow hero to leave the party
			romData[0x36dcf] = 0x4c;
			romData[0x36dd0] = 0xd0;
			romData[0x36dd1] = 0xad;

			// Allow hero to change classes
			romData[0x36c4c] = 0x4c;
			romData[0x36c4d] = 0x44;
			romData[0x36c4e] = 0xac;

			// Allow all heroes to change into a sage
			romData[0x36ccd] = 0x4c;
			romData[0x36cce] = 0xd8;
			romData[0x36ccf] = 0xac;

		}

		private void battleSpeed()
		{
			romData[0x13a65] = 0x01;
			romData[0x13a66] = 0x04;
			romData[0x13a67] = 0x08;
			romData[0x13a68] = 0x0c;
			romData[0x13a69] = 0x10;
			romData[0x13a6a] = 0x18;
			romData[0x13a6b] = 0x20;
			romData[0x852] = 2; // instead of 16 - animation of transition into battle removed, saving 14 frames / start of battle.
			romData[0x8ce] = 1; // instead of 12 - flashes to start a battle, saving 11 frames / start of battle.
			romData[0x980d] = 1; // instead of 8 - Magic spell flashing, saving 7 or 14 frames / spell casted
			romData[0x9827] = 0xea; // NEXT 3 LINES:  1 flash -> 0 flashes
			romData[0x9828] = 0xea;
			romData[0x9829] = 0xea;
			romData[0x9882] = 2; // instead of 12 - Frames of shaking when YOU are hit... saving 10 frames / hit
			romData[0x9957] = 1; // Instead of 4 enemy flashes, saving at least 6 frames / hit... probably 12 or even 24 frames / hit.
		}

		private void speedText()
		{
			romData[0x3a783] = 0x20;
			romData[0x3a784] = 0xbd;
			romData[0x3a785] = 0xbf;

			romData[0x3a9c7] = 0x90;
			romData[0x3a9c8] = 0x1d;
			romData[0x3a9c9] = 0xa6;
			romData[0x3a9ca] = 0x78;
			romData[0x3a9cb] = 0xf0;
			romData[0x3a9cc] = 0x06;

			byte[] speedText = { 0xad, 0xd0, 0x6a, 0xf0, 0x03, 0x00, 0x96, 0x2f, 0x20, 0xba, 0xc2, 0xa9, 0x02, 0x8d, 0xd6, 0x06, 0x20, 0x41, 0xc3, 0xa9, 0x00, 0x8d, 0xd6, 0x06, 0x4c, 0x5f, 0xaa };
			for (int i = 0; i < speedText.Length; i++)
				romData[0x3bfcd + i] = speedText[i];
		}

		private void textGet()
		{
			List<string> txtStrings = new List<string>();
			string tempWord = "";
			for (int lnI = 0; lnI < 1913; lnI++)
			{
				int starter = 0x1b2da;
				if (romData[starter + lnI] == 255)
				{
					txtStrings.Add(tempWord);
					tempWord = "";
				}
				else if (romData[starter + lnI] >= 0 && romData[starter + lnI] <= 9)
				{
					tempWord += (char)(romData[starter + lnI] + 39);
				}
				else if (romData[starter + lnI] >= 10 && romData[starter + lnI] <= 35)
				{
					tempWord += (char)(romData[starter + lnI] + 87);
				}
				else if (romData[starter + lnI] >= 36 && romData[starter + lnI] <= 61)
				{
					tempWord += (char)(romData[starter + lnI] + 29);
				}
			}
			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW3Strings.txt")))
			{
				int lnJ = 1;
				foreach (string word in txtStrings)
				{
					writer.WriteLine(lnJ.ToString("X3") + "-" + word);
					lnJ++;
				}
			}
		}

		private bool loadRom(bool extra = false)
		{
			try
			{
				romData = File.ReadAllBytes(txtFileName.Text);
				if (extra)
					romData2 = File.ReadAllBytes(txtCompare.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Empty file name(s) or unable to open files.  Please verify the files exist.");
				return false;
			}
			return true;
		}

		private void saveRom(bool calcChecksum)
		{
			string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW3Random_" + txtSeed.Text + "_" + txtFlags.Text + ".nes");
			File.WriteAllBytes(finalFile, romData);
			lblIntensityDesc.Text = "ROM hacking complete!  (" + finalFile + ")";
			txtCompare.Text = finalFile;

			if (calcChecksum)
			{
				try
				{
					using (var md5 = SHA1.Create())
					{
						using (var stream = File.OpenRead(finalFile))
						{
							lblNewChecksum.Text = BitConverter.ToString(md5.ComputeHash(stream)).ToLower().Replace("-", "");
						}
					}
				}
				catch
				{
					lblNewChecksum.Text = "????????????????????????????????????????";
				}
			}
		}

		private bool randomizeMapv5(Random r1)
		{
			for (int lnI = 0; lnI < 256; lnI++)
				for (int lnJ = 0; lnJ < 256; lnJ++)
				{
					if (chkSmallMap.Checked && (lnI >= 128 || lnJ >= 128))
					{
						map[lnI, lnJ] = 0x06;
						island[lnI, lnJ] = 200;
					}
					else
					{
						map[lnI, lnJ] = 0x00;
						island[lnI, lnJ] = -1;
					}
				}

			for (int lnI = 0; lnI < 139; lnI++)
				for (int lnJ = 0; lnJ < 158; lnJ++)
				{
					map2[lnI, lnJ] = 0x00;
					island2[lnI, lnJ] = -1;
				}

			for (int lnI = 0; lnI < 132; lnI++)
				for (int lnJ = 0; lnJ < 156; lnJ++)
					map2[lnI, lnJ] = 0x00;


			int smallIslandSize = (r1.Next() % 24000) + 21000; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
			int bigIslandSize = (r1.Next() % 12000) + 33000; // (lnI == 0 ? 1500 : lnI == 1 ? 2500 : lnI == 2 ? 1500 : lnI == 3 ? 1500 : lnI == 4 ? 5000 : 5000);
			int islandSize2 = (chkSmallMap.Checked ? (r1.Next() % 1000) + 2800 : (r1.Next() % 3000) + 11000); // For Tantegel
			smallIslandSize /= (chkSmallMap.Checked ? 4 : 1);
			bigIslandSize /= (chkSmallMap.Checked ? 4 : 1);

			// Set up three special zones.  Zone 1000 = 20 squares and has Thief key stuff.  Zone 2000 = 40 squares and has Magic Key stuff.
			// Zone 3000 = 1 square and has Baramos stuff and end of Necrogund stuff.  It will be surrounded by one tile of mountains.
			// This takes up 94 / 256 of the total squares available.

			bool zonesCreated = false;
			List<int> noradLink = new List<int>();

			while (!zonesCreated)
			{
				zone = new int[16, 16];
				if (createZone(1000, 25, false, r1) && createZone(2000, 50, false, r1))
					zonesCreated = true;
			}

			markZoneSides();
			generateZoneMap(1000, bigIslandSize * 25 / 256, r1); // Aliahan Castle is here.
			generateZoneMap(2000, bigIslandSize * 50 / 256, r1); // Romaly Castle is here.
			generateZoneMap(0, smallIslandSize * 180 / 256, r1); // Norud Cave East is here.
			generateZoneMap(-1000, islandSize2, r1); // About 31% of the regular map

			smoothMap();
			smoothMap2();

			createBridges(r1);
			resetIslands();

			// We should mark islands and inaccessible land...
			int lakeNumber = 256;

			int maxPlots = 0;
			int maxLake = 0;
			int lastValidIsland = -1;

			for (int lnI = 0; lnI < 256; lnI++)
				for (int lnJ = 0; lnJ < 256; lnJ++)
				{
					if (island[lnI, lnJ] == -1)
					{
						int plots = lakePlot(lakeNumber, lnI, lnJ);
						if (plots > maxPlots)
						{
							maxPlots = plots;
							maxLake = lakeNumber;
						}
						lakeNumber++;
					}
					else
					{
						lastValidIsland = island[lnI, lnJ];
					}
				}

			lakeNumber = 4256;
			maxPlots = 0;
			int maxLake2 = 0;
			lastValidIsland = -1;

			for (int lnI = 0; lnI < 139; lnI++)
				for (int lnJ = 0; lnJ < 158; lnJ++)
				{
					if (island2[lnI, lnJ] == -1)
					{
						int plots = lakePlot2(lakeNumber, lnI, lnJ);
						if (plots > maxPlots)
						{
							maxPlots = plots;
							maxLake2 = lakeNumber;
						}
						lakeNumber++;
					}
					else
					{
						lastValidIsland = island[lnI, lnJ];
					}
				}

			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "island.txt")))
			{
				for (int lnY = 0; lnY < 139; lnY++)
				{
					string output = "";
					for (int lnX = 0; lnX < 158; lnX++)
						output += island2[lnY, lnX].ToString().PadLeft(5);
					writer.WriteLine(output);
				}
			}

			// Establish Aliahan location
			bool midenOK = false;
			int[] midenX = new int[7];
			int[] midenY = new int[7];
			while (!midenOK)
			{
				midenX[1] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				midenY[1] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				if (validPlot(midenY[1], midenX[1], 2, 4, new int[] { maxIsland[1] }))
					midenOK = true;
			}

			// Shrine South Of Romaly
			midenOK = false;
			while (!midenOK)
			{
				midenX[2] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				midenY[2] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				if (validPlot(midenY[2], midenX[2], 1, 1, new int[] { maxIsland[2] }))
					midenOK = true;
			}

			// Norud Cave
			midenOK = false;
			while (!midenOK)
			{
				midenX[0] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				midenY[0] = 6 + (r1.Next() % (chkSmallMap.Checked ? 116 : 244));
				if (validPlot(midenY[0], midenX[0], 1, 1, new int[] { maxIsland[0] }))
					midenOK = true;
			}

			// Tantegel
			midenOK = false;
			while (!midenOK)
			{
				midenX[6] = r1.Next() % 132;
				midenY[6] = r1.Next() % 132;
				if (validPlot(midenY[6], midenX[6], 2, 4, new int[] { 60000 }))
					midenOK = true;
			}

			int charlockX = -255;
			int charlockY = -255;

			// Relocate opening Tantegel scene to 1, 1
			romData[0x3ceb4] = 0x01;
			romData[0x3cebf] = 0x01;
			romData[0x1b3eb] = 0x01;
			romData[0x1b3ec] = 0x01;

			// Don't include Romaly, Aliahan, or Portuga islands in future location hunting.
			islands.Remove(maxIsland[1]);
			islands.Remove(maxIsland[2]);
			islands.Remove(maxIsland[3]);


			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "island.txt")))
			{
				for (int lnY = 0; lnY < 256; lnY++)
				{
					string output = "";
					for (int lnX = 0; lnX < 256; lnX++)
						output += island[lnY, lnX].ToString().PadLeft(5) + " ";
					writer.WriteLine(output);
				}
			}

			string[] locTypes = { "C", "C", "C", "?", "S", "X", "T", "T", "?", "T", "?", "T", "T", "X", "T", "?", // Aliahan, Romaly, Eginbear, Baramos, Drought Shrine, XXXXXX, Samanao Town, Brecconary, Charlock, Reeve, Portuga, Noaniels, Assaram, XXXXXX, Baharata, Lancel
                                  // (16) Cantlin, Rimuldar, Hauksness, Luzami, Kanave, Tedanki, Moor, Jipang, Pirate's Den, Soo, Kol, Shrine before Enticement, Shrine S. of Portuga, Sword Of Gaia Shrine, Desert Shrine, Shrine south of Isis
                                  "T", "T", "T", "V", "V", "V", "V", "V", "V", "V", "V", "S", "S", "?", "S", "S",
                                  // (32) Silver Orb Shrine, Olivia Promenade, Olivia Canal Shrine, Dragon Queen Castle, Jipang Shrine, Liamland, Samanao Shrine, Shrine North of Soo, Garinham, Staff of rain shrine, Rainbow Drop Shrine, Portuga Shrine East, West, Promontory Cave, Ruby Cave, Norud Cave West
                                  "?", "S", "S", "C", "S", "S", "S", "S", "S", "S", "S", "?", "?", "E", "E", "E",
                                  // (48) Norud Cave East, Necrogund F5, Necrogund F1, Dhama, Kidnapper's Cave, Jipang Cave, Lancel Cave, Samanao Cave, Erdrick's Cave, Mountain Cave B1, Swamp Cave, Pyramid, Najimi Tower, Garuna Tower, Tower Of Arp, Champange Tower, Tower of Kol
                                  "E", "?", "?", "C", "E", "E", "?", "E", "E", "E", "E", "P", "W", "W", "W", "W", "W",
                                  // (65) Grass tile S of Reeve, Isis, Enticement Cave entrance, Shrine south of Romaly, Pirate Ship, Greenland house, New Town
                                  "?", "?", "?", "?", "?", "?", "?", "X", "?", "X", "?", "?", "?", "?", "?", "?" };

			int[] locIslands = { 1, 2, 9, 9, 9, -100, 9, 6, -2, 1, 3, 2, 2, -100, 0, 4,
								 6, 6, 6, 9, 2, 9, 9, 9, 9, 9, 6, 1, 9, 9, -100, 4,
								 10, 9, 9, 9, 9, 9, 9, 9, 6, 6, 6, 2, 3, 1, 2, 2,
								 0, 9, 10, 9, 0, 9, 9, 9, 6, 6, 6, 2, 1, 0, 9, 2, 6,
								 1, 2, 1, 2, 9, 9, 9, -100, 2, -100, 9, 2, 1, 1, 6, 9, 4 };

			int[] landLocs = { 0, 1, 9, 10, 11, 12, 14, 20, 27, 30, 43, 44, 45, 46, 47, 48, 58, 59, 60, 61, 63, 65, 66, 67 };

			int[] returnLocs = { 0, 1, 2, 6, 7, 9, 10, 11, 12, 14,
								 15, 16, 17, 18, 20, 23, 25, 26, 51, 65 };

			int[] returnPoints = { 0, 2, 12, -1, -1, -1, 13, 15, -1, 1, 7, 4, 5, -1, 8, 10,
								 17, 19, 16, -1, 3, -1, -1, 11, -1, 14, 18, -1, -1, -1, -1, -1,
								 -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
								 -1, -1, -1, 9, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
								 -1, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };

			for (int lnI = 0; lnI < locTypes.Length; lnI++)
			{
				//if (locIslands[lnI] < 0) continue;
				int x = 300;
				int y = 300;
				if (lnI == 0) { x = midenX[1]; y = midenY[1]; }
				else if (lnI == 48) { x = midenX[0]; y = midenY[0]; } // Norud Cave East
				else if (lnI == 77) { x = midenX[2]; y = midenY[2]; } // Shrine South Of Romaly
				else if (lnI == 7) { x = midenX[6]; y = midenY[6]; } // Brecconary/Tantegel
				else if (locIslands[lnI] == -1 || locIslands[lnI] == -2)
				{
					// Subtract 3 for room
					x = 4 + r1.Next() % (chkSmallMap.Checked ? 80 - 4 - 4 : 132 - 4 - 4);
					y = 4 + r1.Next() % (chkSmallMap.Checked ? 80 - 4 - 4 : 132 - 4 - 4);
				}
				else if (locIslands[lnI] == -100)
				{
					continue;
				}
				else
				{
					// Subtract 6 for room
					x = 6 + r1.Next() % (chkSmallMap.Checked ? 128 - 6 - 6 : 256 - 6 - 6);
					y = 6 + r1.Next() % (chkSmallMap.Checked ? 128 - 6 - 6 : 256 - 6 - 6);
				}

				if (locIslands[lnI] == 6)
				{
					if (Math.Abs(y - charlockY) < 5 || Math.Abs(x - charlockX) < 5)
					{
						lnI--;
						continue;
					}
				}

				// TODO:  Ship return points, human return points, bird return points
				// If branches on locTypes, possibly a case.
				switch (locTypes[lnI])
				{
					case "C":
						if (validPlot(y, x, 2, 4, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
							{
								map2[y + 0, x + 1] = 0xe8;
								map2[y + 0, x + 2] = 0xe9;
								map2[y + 1, x + 1] = 0xec;
								map2[y + 1, x + 2] = 0xed;
							}
							else
							{
								map[y + 0, x + 1] = 0xe8;
								map[y + 0, x + 2] = 0xe9;
								map[y + 1, x + 1] = 0xec;
								map[y + 1, x + 2] = 0xed;
							}

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x + 1);
							romData[byteToUse + 1] = (byte)(y + 1);

							if (lnI == 0) // Aliahan Castle
							{
								romData[0x18535] = (byte)(x + 1);
								romData[0x18536] = (byte)(y + 1);
							}

							if (returnPoints[lnI] != -1)
							{
								int byteToUseReturn = 0x1b61c + (4 * returnPoints[lnI]);
								romData[byteToUseReturn] = (byte)(x + 1);
								if (locIslands[lnI] == 6)
								{
									if (map2[y + 2, x] == 0x00 || map2[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 2);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 1);
								}
								else
								{
									if (map[y + 2, x] == 0x00 || map[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 2);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 1);
								}
								if (locIslands[lnI] != 6)
									shipPlacement(byteToUseReturn + 2, y + 1, x + 1, maxLake);
								else
									shipPlacement2(byteToUseReturn + 2, y + 1, x + 1, maxLake2);
							}
						}
						else
							lnI--;

						break;
					case "T": // Town
						if (validPlot(y, x, 1, 4, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
							{
								map2[y + 0, x + 1] = 0xea;
								map2[y + 0, x + 2] = 0xeb;
							}
							else
							{
								map[y + 0, x + 1] = 0xea;
								map[y + 0, x + 2] = 0xeb;
							}

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x + 1);
							romData[byteToUse + 1] = (byte)(y);

							if (returnPoints[lnI] != -1)
							{
								int byteToUseReturn = 0x1b61c + (4 * returnPoints[lnI]);
								romData[byteToUseReturn] = (byte)(x + 1);
								if (locIslands[lnI] == 6)
								{
									if (map2[y + 2, x] == 0x00 || map2[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 1);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 0);
								}
								else
								{
									if (map[y + 2, x] == 0x00 || map[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 1);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 0);
								}
								if (locIslands[lnI] != 6)
									shipPlacement(byteToUseReturn + 2, y, x + 1, maxLake);
								else
									shipPlacement2(byteToUseReturn + 2, y, x + 1, maxLake2);

								if (lnI == 10) // Portuga - set originating ship location to the return point
								{
									romData[0x3d126] = romData[0x7d126] = romData[byteToUseReturn + 2];
									romData[0x3d12a] = romData[0x7d12a] = romData[byteToUseReturn + 3];
								}
							}

						}
						else
							lnI--;

						break;
					case "S": // Shrine
						if (validPlot(y, x, 1, 1, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
								map2[y + 0, x + 0] = 0xf5;
							else
								map[y + 0, x + 0] = 0xf5;

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)x;
							romData[byteToUse + 1] = (byte)y;

							if (lnI == 28)
							{
								romData[0x1851a] = (byte)x;
								romData[0x1851b] = (byte)y;
							}
							else if (lnI == 31)
							{
								romData[0x184ff] = (byte)x;
								romData[0x18500] = (byte)y;
							}
							else if (lnI == 33)
							{
								romData[0x18505] = romData[0x18508] = (byte)x;
								romData[0x18506] = romData[0x18509] = (byte)y;
							}
							else if (lnI == 36)
							{
								romData[0x184f6] = (byte)x;
								romData[0x184f7] = (byte)y;
							}
							else if (lnI == 37)
							{
								romData[0x3255d] = (byte)x;
								romData[0x32561] = (byte)(y - 1);
							}
							else if (lnI == 38)
							{
								romData[0x184f9] = romData[0x1850e] = (byte)x;
								romData[0x184fa] = romData[0x1850f] = (byte)y;
							}
							else if (lnI == 39)
							{
								romData[0x184fc] = romData[0x18502] = romData[0x18517] = (byte)x;
								romData[0x184fd] = romData[0x18503] = romData[0x18518] = (byte)y;
							}
						}
						else
							lnI--;

						break;
					case "V": // Village
						if (validPlot(y, x, 1, 3, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
								map2[y + 0, x + 1] = 0xf1;
							else
								map[y + 0, x + 1] = 0xf1;

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x + 1);
							romData[byteToUse + 1] = (byte)y;

							if (returnPoints[lnI] != -1)
							{
								int byteToUseReturn = 0x1b61c + (4 * returnPoints[lnI]);
								romData[byteToUseReturn] = (byte)(x + 1);
								if (locIslands[lnI] == 6)
								{
									if (map2[y + 2, x] == 0x00 || map2[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 1);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 0);
								}
								else
								{
									if (map[y + 2, x] == 0x00 || map[y + 2, x] == 0x06)
										romData[byteToUseReturn + 1] = (byte)(y + 1);
									else
										romData[byteToUseReturn + 1] = (byte)(y + 0);
								}
								if (locIslands[lnI] != 6)
									shipPlacement(byteToUseReturn + 2, y, x + 1, maxLake);
								else
									shipPlacement2(byteToUseReturn + 2, y, x + 1, maxLake2);
							}

							if (lnI == 23)
							{
								romData[0x311c4] = (byte)(x + 1);
								romData[0x311c8] = (byte)y;
							}
						}
						else
							lnI--;

						break;
					case "P": // Pyramid
						if (validPlot(y, x, 3, 1, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							map[y + 2, x] = 0xf3;

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x);
							romData[byteToUse + 1] = (byte)(y + 2);
						}
						else
							lnI--;

						break;
					case "E": // Cave
						if (validPlot(y, x, 1, 1, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
								map2[y + 0, x + 0] = 0xef;
							else
								map[y + 0, x + 0] = 0xef;

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x);
							romData[byteToUse + 1] = (byte)(y);

							if (lnI == 45)
							{
								romData[0x18533] = (byte)(x);
								romData[0x18534] = (byte)(y);
							}
							else if (lnI == 47)
							{
								romData[0x1852b] = (byte)(x);
								romData[0x1852c] = (byte)(y);
							}
							else if (lnI == 48)
							{
								romData[0x1852d] = (byte)(x);
								romData[0x1852e] = (byte)(y);
							}
							else if (lnI == 49)
							{
								romData[0x1853d] = (byte)(x);
								romData[0x1853e] = (byte)(y);
							}
							else if (lnI == 56)
							{
								romData[0x30edb] = (byte)(x);
								romData[0x30edf] = (byte)(y);
							}
						}
						else
							lnI--;

						break;
					case "W": // Tower
						if (validPlot(y, x, 3, 3, (locIslands[lnI] <= 3 ? new int[] { maxIsland[locIslands[lnI]] } : locIslands[lnI] <= 6 ? new int[] { 60000 } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
							locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], locIslands[lnI] == 6 ? maxLake2 : maxLake, locIslands[lnI] == 6))
						{
							if (locIslands[lnI] == 6)
							{
								map2[y + 0, x + 0] = 0xf2;
								map2[y + 1, x + 0] = 0xee;
							}
							else
							{
								map[y + 0, x + 0] = 0xf2;
								map[y + 1, x + 0] = 0xee;
							}

							int byteToUse = 0x1b252 + (5 * lnI);
							romData[byteToUse] = (byte)(x);
							romData[byteToUse + 1] = (byte)(y + 1);

							if (lnI == 60) // Najimi Tower
							{
								romData[0x18537] = (byte)(x);
								romData[0x18538] = (byte)(y + 1);

								// Direct LDA of Tower fall point
								romData[0x3d401] = (byte)(x);
								romData[0x3d405] = (byte)(y + 1);

								romData[0x7d401] = (byte)(x);
								romData[0x7d405] = (byte)(y + 1);
							}
							else if (lnI == 61) // Garuna Tower
							{
								romData[0x1851d] = romData[0x18520] = romData[0x18526] = romData[0x18529] = (byte)x;
								romData[0x1851e] = romData[0x18521] = romData[0x18527] = romData[0x1852a] = (byte)(y + 1);
							}
						}
						else
							lnI--;

						break;
					case "?":
						if (lnI == 3) // Baramos Castle
						{
							bool baramosLegal = true;
							for (int lnJ = x - 4; lnJ < x + 4; lnJ++)
								for (int lnK = y - 4; lnK < y + 4; lnK++)
								{
									if (map[lnK, lnJ] > 0x07)
										baramosLegal = false;
									if (lnK == midenY[0] && lnJ == midenX[0])
										baramosLegal = false;
									if (lnK == midenY[1] && lnJ == midenX[1])
										baramosLegal = false;
									if (lnK == midenY[2] && lnJ == midenX[2])
										baramosLegal = false;
								}

							if (baramosLegal)
							{
								for (int lnJ = -3; lnJ < 3; lnJ++)
									for (int lnK = -3; lnK < 3; lnK++)
										island[y + lnJ, x + lnK] = 4001;

								for (int lnJ = -3; lnJ < 3; lnJ++)
								{
									map[y + lnJ, x - 3] = 0x06;
									map[y + lnJ, x + 2] = 0x06;
									map[y - 3, x + lnJ] = 0x06;
									map[y + 2, x + lnJ] = 0x06;
								}
								for (int lnJ = -2; lnJ < 2; lnJ++)
								{
									map[y + lnJ, x - 2] = 0x07;
									map[y + lnJ, x + 1] = 0x07;
									map[y - 2, x + lnJ] = 0x07;
									map[y + 1, x + lnJ] = 0x07;
								}

								map[y - 1, x - 1] = 0xe8;
								map[y - 1, x + 0] = 0xe9;
								map[y + 0, x - 1] = 0xec;
								map[y + 0, x + 0] = 0xed;

								// Let's also get the Pit Of Giaga!
								map[y + 1, x + 1] = 0xf4;
								romData[0x1b3f1] = (byte)(x + 1);
								romData[0x1b3f2] = (byte)(y + 1);

								int byteToUse = 0x1b252 + (5 * 3);
								romData[byteToUse] = (byte)(x - 1);
								romData[byteToUse + 1] = (byte)y;
							}
							else
								lnI--;
						}
						else if (lnI == 8) // Charlock Castle
						{
							bool baramosLegal = true;
							if (x < 10 || y < 10 || x > 150 || y > 130)
								baramosLegal = false;

							if (baramosLegal)
							{
								for (int lnJ = x - 4; lnJ < x + 4; lnJ++)
									for (int lnK = y - 4; lnK < y + 4; lnK++)
									{
										if (map2[lnK, lnJ] > 0x07)
											baramosLegal = false;
									}
							}

							if (baramosLegal)
							{
								charlockX = x;
								charlockY = y;

								for (int lnJ = -4; lnJ < 4; lnJ++)
									for (int lnK = -4; lnK < 4; lnK++)
										map2[y + lnJ, x + lnK] = 0x06;

								for (int lnJ = -3; lnJ < 3; lnJ++)
									for (int lnK = -3; lnK < 3; lnK++)
										map2[y + lnJ, x + lnK] = 0x00;


								for (int lnJ = -2; lnJ < 2; lnJ++)
									for (int lnK = -2; lnK < 2; lnK++)
										map2[y + lnJ, x + lnK] = 0x07;

								map2[y - 1, x - 1] = 0xe8;
								map2[y - 1, x] = 0xe9;
								map2[y, x - 1] = 0xec;
								map2[y, x] = 0xed;
								map2[y, x + 3] = 0x01;
								int lnL = x + 4;
								while (map2[y, lnL] == 0x00 && lnL < 132)
								{
									map2[y, lnL] = 0x01;
									lnL++;
								}

								// Rainbow Drop
								romData[0x1bfc6] = (byte)(x + 3);
								romData[0x1bfcc] = (byte)y;
								romData[0x3f023] = (byte)(x + 1);
								romData[0x3f027] = (byte)(x + 4);
								romData[0x3f019] = (byte)y;

								int byteToUse = 0x1b252 + (5 * 8);
								romData[byteToUse] = (byte)(x - 1);
								romData[byteToUse + 1] = (byte)y;
							}
							else
								lnI--;
						}
						else if (lnI == 15) // Lancel / Lancel Cave
						{
							bool baramosLegal = true;
							for (int lnJ = x - 2; lnJ <= x + 2; lnJ++)
								for (int lnK = y - 1; lnK <= y + 1; lnK++)
								{
									if (map[lnK, lnJ] > 0x07)
										baramosLegal = false;
								}

							if (baramosLegal)
							{
								for (int lnJ = -2; lnJ < 2; lnJ++)
								{
									map[y - 1, x + lnJ] = 0x06;
									map[y + 1, x + lnJ] = 0x06;
									island[y - 1, x + lnJ] = 6000;
									island[y, x + lnJ] = 6000;
									island[y + 1, x + lnJ] = 6000;
								}

								map[y, x - 2] = 0x06;
								map[y, x - 1] = 0xef;
								map[y, x] = 0x01;
								map[y, x + 1] = 0xea;
								map[y, x + 2] = 0xeb;
								map[y - 1, x + 1] = 0x01;
								map[y - 1, x + 2] = 0x01;
								map[y + 1, x + 1] = 0x01;
								map[y + 1, x + 2] = 0x01;

								romData[0x1b360] = (byte)(x - 1);
								romData[0x1b361] = (byte)y;

								romData[0x1b29d] = (byte)(x + 1);
								romData[0x1b29e] = (byte)y;

								romData[0x3d16f] = (byte)x;

								romData[0x32736] = (byte)(x + 1);
								romData[0x3273a] = (byte)y;

								// Return point
								int byteToUseReturn = 0x1b61c + (4 * 10);
								romData[byteToUseReturn] = (byte)(x + 2);
								romData[byteToUseReturn + 1] = (byte)(y + 1);
								shipPlacement(byteToUseReturn + 2, y + 1, x + 2, maxLake);
							}
							else
								lnI--;
						}
						//else if (lnI == 32) // Silver Orb Shrine; skip, addressing that in Necrogund.
						else if (lnI == 29) // Olivia Canal Shrine
						{
							bool baramosLegal = true;
							for (int lnJ = x - 4; lnJ < x + 4; lnJ++)
								for (int lnK = y - 1; lnK < y + 1; lnK++)
								{
									if (map[lnK, lnJ] != 0x00 || island[lnK, lnJ] != maxLake)
										baramosLegal = false;
								}

							if (baramosLegal)
							{
								// Create line of mountains
								for (int lnJ = -3; lnJ < 1; lnJ++)
								{
									map[y + 1, x + lnJ] = 0x06;
									map[y - 1, x + lnJ] = 0x06;
								}
								// Make the rest water
								for (int lnJ = 1; lnJ < 4; lnJ++)
								{
									map[y + 1, x + lnJ] = 0x00;
									map[y - 1, x + lnJ] = 0x00;
								}
								map[y, x - 4] = 0x06;
								map[y, x - 3] = 0xf5; // Shrine Placement
								map[y, x + 3] = 0xf7; // Shoal Placement

								romData[0x1b2e3] = (byte)(x - 3);
								romData[0x1b2e4] = (byte)y;
								// Olivia bad news spot
								romData[0x3313e] = (byte)(x - 2);
								romData[0x33144] = (byte)y;
							}
							else
								lnI--;
						}
						else if (lnI == 43)
						{
							if (validPlot(y, x, 1, 1, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								bool baramosLegal = true;
								int x2 = 8 + r1.Next() % (chkSmallMap.Checked ? 128 - 8 - 4 : 256 - 8 - 8);

								if (chkSmallMap.Checked)
								{
									for (int lnJ = x2 - 3; lnJ < x2 + 2; lnJ++)
										for (int lnK = y; lnK < y + 2; lnK++)
										{
											if (map[lnK, lnJ] != 0x00 || island[lnK, lnJ] != maxLake)
												baramosLegal = false;
										}

									if (baramosLegal)
									{
										map[y + 0, x + 0] = 0xf5;

										// Map Portuga Shrine East to the ROM
										int byteToUse = 0x1b252 + (5 * lnI);
										romData[byteToUse] = (byte)x;
										romData[byteToUse + 1] = (byte)y;

										map[y + 0, x2 - 1] = 0x05;
										map[y + 0, x2 - 3] = 0xea;
										map[y + 0, x2 - 2] = 0xeb;
										map[y + 1, x2 - 1] = 0x05;
										map[y + 1, x2 - 2] = 0x05;
										map[y + 1, x2 - 3] = 0x05;

										// Map Portuga Castle to the ROM
										byteToUse = 0x1b252 + (5 * 10);
										romData[byteToUse] = (byte)(x2 - 3);
										romData[byteToUse + 1] = (byte)y;

										map[y + 0, x2 + 0] = 0xf5;
										// Map Portuga Shrine West to the ROM
										byteToUse = 0x1b252 + (5 * 44);
										romData[byteToUse] = (byte)x2;
										romData[byteToUse + 1] = (byte)y;

										int byteToUseReturn = 0x1b61c + (4 * returnPoints[10]);
										romData[byteToUseReturn] = (byte)(x2 - 4);
										romData[byteToUseReturn + 1] = (byte)y;
										shipPlacement(byteToUseReturn + 2, y - 1, x2 - 4, maxLake);

										romData[0x3d126] = romData[0x7d126] = romData[byteToUseReturn + 2];
										romData[0x3d12a] = romData[0x7d12a] = romData[byteToUseReturn + 3];

										romData[0x1850b] = romData[0x3d18b] = romData[0x7d18b] = (byte)x;
										romData[0x1850c] = romData[0x3d181] = romData[0x7d181] = (byte)y;

										romData[0x3d192] = romData[0x7d192] = (byte)x2;
									}
									else
										lnI--;
								}
								else
								{
									for (int lnJ = x2 - 5; lnJ < x2 + 5; lnJ++)
										for (int lnK = y - 3; lnK < y + 3; lnK++)
										{
											if (map[lnK, lnJ] != 0x00 || island[lnK, lnJ] != maxLake)
												baramosLegal = false;
										}

									if (baramosLegal)
									{
										map[y + 0, x + 0] = 0xf5;

										// Map Portuga Shrine East to the ROM
										int byteToUse = 0x1b252 + (5 * lnI);
										romData[byteToUse] = (byte)x;
										romData[byteToUse + 1] = (byte)y;

										for (int lnJ = -4; lnJ < 4; lnJ++)
										{
											if (lnJ == -4 || lnJ == -3 || lnJ == -1 || lnJ == 1 || lnJ == 3)
											{
												map[y - 2, x2 + lnJ] = map[y - 1, x2 + lnJ] = map[y, x2 + lnJ] = map[y + 1, x2 + lnJ] = map[y + 2, x2 + lnJ] = 0x05;
											}
											else if (lnJ == -2 || lnJ == 2)
											{
												map[y - 2, x2 + lnJ] = map[y - 1, x2 + lnJ] = map[y, x2 + lnJ] = map[y + 1, x2 + lnJ] = 0x06;
												map[y + 2, x2 + lnJ] = 0x05;
											}
											else if (lnJ == 0)
											{
												map[y - 1, x2 + lnJ] = map[y, x2 + lnJ] = map[y + 1, x2 + lnJ] = map[y + 2, x2 + lnJ] = 0x06;
												map[y - 2, x2 + lnJ] = 0x05;
											}
											island[y - 2, x2 + lnJ] = island[y - 1, x2 + lnJ] = island[y, x2 + lnJ] = island[y + 1, x2 + lnJ] = island[y + 2, x2 + lnJ] = 3000;
										}

										map[y - 2, x2 - 4] = 0xea;
										map[y - 2, x2 - 3] = 0xeb;

										// Map Portuga Castle to the ROM
										byteToUse = 0x1b252 + (5 * 10);
										romData[byteToUse] = (byte)(x2 - 4);
										romData[byteToUse + 1] = (byte)(y - 2);

										map[y + 0, x2 + 3] = 0xf5;
										// Map Portuga Shrine West to the ROM
										byteToUse = 0x1b252 + (5 * 44);
										romData[byteToUse] = (byte)(x2 + 3);
										romData[byteToUse + 1] = (byte)y;

										int byteToUseReturn = 0x1b61c + (4 * returnPoints[10]);
										romData[byteToUseReturn] = (byte)(x2 - 4);
										romData[byteToUseReturn + 1] = (byte)(y - 1);
										shipPlacement(byteToUseReturn + 2, y - 1, x2 - 4, maxLake);

										romData[0x3d126] = romData[0x7d126] = romData[byteToUseReturn + 2];
										romData[0x3d12a] = romData[0x7d12a] = romData[byteToUseReturn + 3];

										romData[0x1850b] = romData[0x3d18b] = romData[0x7d18b] = (byte)x;
										romData[0x1850c] = romData[0x3d181] = romData[0x7d181] = (byte)y;

										romData[0x3d192] = romData[0x7d192] = (byte)(x2 + 3);
									}
									else
										lnI--;
								}

							}
							else
								lnI--;
						}
						else if (lnI == 10 || lnI == 44)
							continue;
						else if (lnI == 49) // Necrogund F1
						{
							bool baramosLegal = true;
							for (int lnJ = x - 5; lnJ <= x + 4; lnJ++)
								for (int lnK = y - 3; lnK <= y + 2; lnK++)
								{
									if (map[lnK, lnJ] > 0x07)
										baramosLegal = false;
								}

							if (baramosLegal)
							{
								for (int lnJ = x - 5; lnJ <= x + 4; lnJ++)
									for (int lnK = y - 3; lnK <= y + 2; lnK++)
									{
										island[lnK, lnJ] = 5001;
									}

								for (int lnJ = y - 3; lnJ <= y + 2; lnJ++)
								{
									map[lnJ, x - 5] = 0x06;
									map[lnJ, x + 2] = 0x06;
									map[lnJ, x + 4] = 0x06;
								}
								for (int lnJ = x - 5; lnJ <= x + 4; lnJ++)
									map[y + 2, lnJ] = 0x06;
								for (int lnJ = x - 5; lnJ <= x + 4; lnJ++)
									map[y - 3, lnJ] = 0x05;

								// Silver Orb Shrine/Necrogund F5
								map[y - 2, x + 3] = 0x06;
								map[y - 1, x + 3] = 0xf5;
								map[y + 0, x + 3] = 0x05;
								map[y + 1, x + 3] = 0xef;

								// The rest
								map[y - 2, x - 4] = 0x05;
								map[y - 2, x - 3] = 0x05;
								map[y - 2, x - 2] = 0x05;
								map[y - 2, x - 1] = 0x06;
								map[y - 2, x + 0] = 0x05;
								map[y - 2, x + 1] = 0x06;

								map[y - 1, x - 4] = 0x05;
								map[y - 1, x - 3] = 0x05;
								map[y - 1, x - 2] = 0x00;
								map[y - 1, x - 1] = 0x05;
								map[y - 1, x + 0] = 0xf0;
								map[y - 1, x + 1] = 0x05;

								map[y - 0, x - 4] = 0x05;
								map[y - 0, x - 3] = 0x00;
								map[y - 0, x - 2] = 0x00;
								map[y - 0, x - 1] = 0x05;
								map[y - 0, x + 0] = 0x05;
								map[y - 0, x + 1] = 0x05;

								map[y + 1, x - 4] = 0x00;
								map[y + 1, x - 3] = 0x00;
								map[y + 1, x - 2] = 0x05;
								map[y + 1, x - 1] = 0x05;
								map[y + 1, x + 0] = 0x05;
								map[y + 1, x + 1] = 0xef;

								// Volcano stuff
								// First, Sword of Gaia
								romData[0x2e7f] = romData[0x32a93] = (byte)x;
								romData[0x2e85] = romData[0x32a99] = (byte)(y - 2);
								// Second, mapping stuff
								romData[0x3f09b] = (byte)(x - 4);
								romData[0x3f09f] = (byte)(x - 1);
								romData[0x3f0a5] = (byte)(y - 1);
								romData[0x3f0a9] = (byte)(y + 3);
								// Map link to cave now though!
								// Beginning
								romData[0x18531] = romData[0x1b34c] = (byte)(x + 1);
								romData[0x18532] = romData[0x1b34d] = (byte)(y + 1);
								// End
								romData[0x1852f] = romData[0x1b347] = (byte)(x + 3);
								romData[0x18530] = romData[0x1b348] = (byte)(y + 1);
								// Shrine
								romData[0x1b2f2] = (byte)(x + 3);
								romData[0x1b2f3] = (byte)(y - 1);
							}
							else
								lnI--;
						}
						//else if (lnI == 50) // Necrogrund F5/Silver Orb Shrine - skip, see above
						else if (lnI == 65) // Grass tile south of Reeve
						{
							if (validPlot(y, x, 3, 3, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								map[y + 0, x + 0] = map[y + 0, x + 1] = map[y + 0, x + 2] = map[y + 1, x + 0] = map[y + 1, x + 2] = map[y + 2, x + 0] = map[y + 2, x + 1] = map[y + 2, x + 2] = 0x04;
								map[y + 1, x + 1] = 0x03;

								romData[0x1b3df] = (byte)(x + 1);
								romData[0x1b3e0] = (byte)(y + 1);

								romData[0x184f3] = romData[0x18539] = (byte)(x + 1);
								romData[0x184f4] = romData[0x1853a] = (byte)(y + 1);
							}
							else
								lnI--;
						}
						else if (lnI == 66) // Isis
						{
							if (validPlot(y, x, 3, 3, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								map[y + 0, x + 0] = map[y + 0, x + 1] = map[y + 0, x + 2] = map[y + 1, x + 0] = map[y + 1, x + 2] = map[y + 2, x + 0] = map[y + 2, x + 1] = map[y + 2, x + 2] = 0x01;
								map[y + 1, x + 1] = 0x00;

								romData[0x1b39d] = (byte)(x + 1);
								romData[0x1b39e] = (byte)(y + 2);

								romData[0x1b3bb] = (byte)(x + 0);
								romData[0x1b3bc] = (byte)(y + 2);

								romData[0x1b3b5] = (byte)(x + 0);
								romData[0x1b3b6] = (byte)(y + 0);

								romData[0x1b3af] = (byte)(x + 1);
								romData[0x1b3b0] = (byte)(y + 0);

								romData[0x1b3a9] = (byte)(x + 2);
								romData[0x1b3aa] = (byte)(y + 0);

								romData[0x1b3c1] = (byte)(x + 2);
								romData[0x1b3c2] = (byte)(y + 1);

								romData[0x1b3a3] = (byte)(x + 2);
								romData[0x1b3a4] = (byte)(y + 2);

								if (returnPoints[lnI] != -1)
								{
									int byteToUseReturn = 0x1b61c + (4 * returnPoints[lnI]);
									romData[byteToUseReturn] = (byte)(x + 0);
									romData[byteToUseReturn + 1] = (byte)(y + 1);
									shipPlacement(byteToUseReturn + 2, y + 1, x, maxLake);

								}
							}
							else
								lnI--;
						}
						else if (lnI == 67) // Enticement Cave
						{
							if (chkSmallMap.Checked)
							{
								if (validPlot(y - 1, x - 1, 3, 3, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
									locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
								{
									for (int lnX = -1; lnX < 2; lnX++)
										for (int lnY = -1; lnY < 2; lnY++)
											map[y + lnY, x + lnX] = 0x04;

									map[y, x] = 0x00;

									romData[0x1b3e5] = (byte)(x);
									romData[0x1b3e6] = (byte)(y + 1);

									romData[0x18514] = romData[0x1853b] = (byte)(x);
									romData[0x18515] = romData[0x1853c] = (byte)(y + 1);
								}
								else
									lnI--;
							}
							else
							{
								if (y == 199 && x == 157) y = y;
								if (validPlot(y - 2, x - 2, 5, 5, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
									locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
								{
									for (int lnX = -2; lnX < 3; lnX++)
										for (int lnY = -2; lnY < 3; lnY++)
											map[y + lnY, x + lnX] = 0x05;

									for (int lnX = -1; lnX < 2; lnX++)
										for (int lnY = -1; lnY < 2; lnY++)
											map[y + lnY, x + lnX] = 0x04;

									map[y, x] = 0x00;
									map[y + 2, x] = 0x04;

									romData[0x1b3e5] = (byte)(x);
									romData[0x1b3e6] = (byte)(y + 1);

									romData[0x18514] = romData[0x1853b] = (byte)(x);
									romData[0x18515] = romData[0x1853c] = (byte)(y + 1);
								}
								else
									lnI--;
							}
						}
						else if (lnI == 68) // Shrine South of Romaly
						{
							if (validPlot(y, x, 1, 1, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								romData[0x1b3d9] = (byte)(x);
								romData[0x1b3da] = (byte)(y);

								romData[0x18523] = (byte)(x);
								romData[0x18524] = (byte)(y);
							}
							else
								lnI--;
						}
						else if (lnI == 69) // Pirate Ship
						{
							bool baramosLegal = true;
							if (map[y, x] != 0x00 || island[y, x] != maxLake)
								baramosLegal = false;

							if (baramosLegal)
							{
								romData[0x378ba] = romData[0x3b630] = (byte)x;
								romData[0x378be] = romData[0x3b634] = (byte)y;
							}
							else
								lnI--;
						}
						else if (lnI == 70) // Greenland house
						{
							if (validPlot(y, x, 3, 3, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								map[y + 0, x + 0] = map[y + 0, x + 1] = map[y + 0, x + 2] = map[y + 1, x + 0] = map[y + 1, x + 2] = map[y + 2, x + 0] = map[y + 2, x + 1] = map[y + 2, x + 2] = 0x01;
								map[y + 1, x + 1] = 0x02;

								romData[0x1b3d3] = (byte)(x + 1);
								romData[0x1b3d4] = (byte)(y + 1);
							}
							else
								lnI--;
						}
						else if (lnI == 71) // New Town
						{
							if (validPlot(y, x, 3, 3, (locIslands[lnI] <= 6 ? new int[] { maxIsland[locIslands[lnI]] } : islands.ToArray())) && reachable(y, x, !landLocs.Contains(lnI),
								locIslands[lnI] <= 6 ? midenX[locIslands[lnI]] : midenX[1], locIslands[lnI] <= 6 ? midenY[locIslands[lnI]] : midenY[1], maxLake, locIslands[lnI] == 6))
							{
								map[y + 0, x + 0] = map[y + 0, x + 1] = map[y + 0, x + 2] = map[y + 1, x + 0] = map[y + 1, x + 2] = map[y + 2, x + 0] = map[y + 2, x + 1] = map[y + 2, x + 2] = 0x04;
								map[y + 1, x + 1] = 0x02;

								romData[0x1b397] = (byte)(x + 1);
								romData[0x1b398] = (byte)(y + 1);
							}
							else
								lnI--;
						}

						break;
					case "X":
						continue;
				}
			}

			List<int> part1 = new List<int>() { 4, 5, 6, 7 };
			List<int> part2 = new List<int>() { 8, 9, 10, 12, 13, 14 };
			List<int> part3 = new List<int>() { 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 26, 27, 28, 29, 30, 31, 32, 33, 35, 36, 37, 39, 41, 42, 43, 44, 45, 46, 47, 48, 49 };
			List<int> tantegel = new List<int>() { 50, 51, 52, 54, 55, 57, 58, 60, 61, 63 };

			int[,] monsterZones = new int[16, 16];
			for (int lnI = 0; lnI < 16; lnI++)
				for (int lnJ = 0; lnJ < 16; lnJ++)
					monsterZones[lnI, lnJ] = 0xff;

			int midenMZX = midenX[1] / 16;
			int midenMZY = midenY[1] / 16;

			for (int mzX = 0; mzX < 16; mzX++)
				for (int mzY = 0; mzY < 16; mzY++)
				{
					if (mzX == 10) mzX = mzX;
					if (zone[mzY, mzX] / 1000 == 1)
					{
						if (Math.Abs(midenMZX - mzX) == 0 && Math.Abs(midenMZY - mzY) == 0)
							monsterZones[mzY, mzX] = 4;
						else
							monsterZones[mzY, mzX] = part1[r1.Next() % part1.Count];
					}
					else if (zone[mzY, mzX] / 1000 == 2)
						monsterZones[mzY, mzX] = part2[r1.Next() % part2.Count];
					else
						monsterZones[mzY, mzX] = part3[r1.Next() % part3.Count];

					monsterZones[mzY, mzX] += 64 * (r1.Next() % 4);
				}

			bool badMap = true;
			bool compressed = false;
			while (badMap)
			{
				// Now let's enter all of this into the ROM...
				int lnPointer = 0x821a;

				for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
				{
					romData[0x14028 + (lnI * 2)] = (byte)(lnPointer % 256);
					romData[0x14029 + (lnI * 2)] = (byte)(lnPointer / 256);

					int lnJ = 0;
					while (lnI < 256 && lnJ < 256)
					{
						if (map[lnI, lnJ] >= 0 && map[lnI, lnJ] <= 7)
						{
							int tileNumber = 0;
							int numberToMatch = map[lnI, lnJ];
							while (lnJ < 256 && tileNumber < (numberToMatch == 7 ? 8 : 32) && map[lnI, lnJ] == numberToMatch)
							{
								tileNumber++;
								lnJ++;
							}
							romData[lnPointer + 0xc010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
							lnPointer++;
						}
						else
						{
							romData[lnPointer + 0xc010] = (byte)map[lnI, lnJ];
							lnPointer++;
							lnJ++;
						}
					}
				}
				if (compressed) badMap = false;

				//lnPointer = lnPointer;
				if (lnPointer > 0x9a94)
				{
					MessageBox.Show("WARNING:  The map might have taken too much ROM space... (" + (lnPointer - 0x9a94).ToString() + " over)");
					compressed = true;
					// Might have to compress further to remove one byte stuff
					// Must compress the map by getting rid of further 1 byte lakes
				}
				else
					badMap = false;
			}

			badMap = true;
			compressed = false;
			while (badMap)
			{
				// Now let's enter all of this into the ROM... (Alefgard)
				int lnPointer = 0x9bab;

				for (int lnI = 0; lnI <= 139; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
				{
					romData[0x15aa5 + (lnI * 2)] = (byte)(lnPointer % 256);
					romData[0x15aa6 + (lnI * 2)] = (byte)(lnPointer / 256);

					int lnJ = 0;
					while (lnI < 139 && lnJ < 158)
					{
						if (map2[lnI, lnJ] >= 0 && map2[lnI, lnJ] <= 7)
						{
							int tileNumber = 0;
							int numberToMatch = map2[lnI, lnJ];
							while (lnJ < 158 && tileNumber < (numberToMatch == 7 ? 8 : 32) && map2[lnI, lnJ] == numberToMatch)
							{
								tileNumber++;
								lnJ++;
							}
							romData[lnPointer + 0xc010] = (byte)((0x20 * numberToMatch) + (tileNumber - 1));
							lnPointer++;
						}
						else
						{
							romData[lnPointer + 0xc010] = (byte)map2[lnI, lnJ];
							lnPointer++;
							lnJ++;
						}
					}
				}
				if (compressed) badMap = false;

				//lnPointer = lnPointer;
				if (lnPointer > 0xa3ee)
				{
					MessageBox.Show("WARNING:  The map might have taken too much ROM space... (" + (lnPointer - 0xa3ee).ToString() + " over)");
					compressed = true;
					// Might have to compress further to remove one byte stuff
					// Must compress the map by getting rid of further 1 byte lakes
				}
				else
					badMap = false;
			}

			// Ensure monster zones are 8x8
			if (chkSmallMap.Checked)
			{
				romData[0x2d8] = 0x85;
				romData[0x2d9] = 0x4a;
				romData[0x2da] = 0xa5;
				romData[0x2db] = 0x2b;
				romData[0x2dc] = 0x29;
				romData[0x2dd] = 0xf0;
				romData[0x2de] = 0x0a;
			}

			// Enter monster zones
			for (int lnI = 0; lnI < 16; lnI++)
				for (int lnJ = 0; lnJ < 16; lnJ++)
				{
					if (monsterZones[lnI, lnJ] == 0xff)
						monsterZones[lnI, lnJ] = (r1.Next() % 60) + ((r1.Next() % 4) * 64);
					romData[0x956 + (lnI * 16) + lnJ] = (byte)monsterZones[lnI, lnJ];
				}

			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "zones.txt")))
			{
				for (int lnY = 0; lnY < 16; lnY++)
				{
					string output = "";
					for (int lnX = 0; lnX < 16; lnX++)
						output += zone[lnY, lnX].ToString().PadLeft(5) + " ";
					writer.WriteLine(output);
				}
			}

			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "monsters.txt")))
			{
				for (int lnY = 0; lnY < 16; lnY++)
				{
					string output = "";
					for (int lnX = 0; lnX < 16; lnX++)
						output += monsterZones[lnY, lnX].ToString("X2") + " ";
					writer.WriteLine(output);
				}
			}

			return true;
		}

		private void markZoneSides()
		{
			for (int x = 0; x < 16; x++)
				for (int y = 0; y < 16; y++)
				{
					// 1 = north, 2 = east, 4 = south, 8 = west
					if (y == 0) zone[y, x] += 1;
					else if (zone[y - 1, x] / 1000 != zone[y, x] / 1000) zone[y, x] += 1;

					if (x == 15) zone[y, x] += 2;
					else if (zone[y, x + 1] / 1000 != zone[y, x] / 1000) zone[y, x] += 2;

					if (y == 15) zone[y, x] += 4;
					else if (zone[y + 1, x] / 1000 != zone[y, x] / 1000) zone[y, x] += 4;

					if (x == 0) zone[y, x] += 8;
					else if (zone[y, x - 1] / 1000 != zone[y, x] / 1000) zone[y, x] += 8;
				}
		}

		private void generateZoneMap(int zoneToUse, int islandSize, Random r1)
		{
			int xMax = (zoneToUse != -1000 ? (chkSmallMap.Checked ? 128 : 256) : (chkSmallMap.Checked ? 80 : 136)) - 7;
			int yMax = (zoneToUse != -1000 ? (chkSmallMap.Checked ? 128 : 256) : (chkSmallMap.Checked ? 80 : 132)) - 7;
			int yMin = 6;
			int xMin = 6;

			int[] terrainTypes = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7 };

			for (int lnI = 0; lnI < 100; lnI++)
			{
				int swapper1 = r1.Next() % terrainTypes.Length;
				int swapper2 = r1.Next() % terrainTypes.Length;
				int temp = terrainTypes[swapper1];
				terrainTypes[swapper1] = terrainTypes[swapper2];
				terrainTypes[swapper2] = temp;
			}

			int lnMarker = -1;
			int totalLand = 0;

			while (totalLand < islandSize)
			{
				lnMarker++;
				lnMarker = (lnMarker >= terrainTypes.Length ? 0 : lnMarker);
				int sizeToUse = (r1.Next() % 400) + 150;
				//if (terrainTypes[lnMarker] == 5) sizeToUse /= 2;

				List<int> points = new List<int> { (r1.Next() % (xMax - xMin)) + xMin, (r1.Next() % (yMax - yMin)) + yMin };
				if (validPoint(points[0], points[1], zoneToUse))
				{
					while (sizeToUse > 0)
					{
						List<int> newPoints = new List<int>();
						for (int lnI = 0; lnI < points.Count; lnI += 2)
						{
							int lnX = points[lnI];
							int lnY = points[lnI + 1];

							if (lnX == 127 && lnY == 56) lnX = lnX;

							int direction = (r1.Next() % 16);
							if (zoneToUse != -1000)
							{
								map[lnY, lnX] = terrainTypes[lnMarker];
								island[lnY, lnX] = (terrainTypes[lnMarker] == 6 ? -1 - zoneToUse : zoneToUse);
							}
							else
							{
								map2[lnY, lnX] = terrainTypes[lnMarker];
								island2[lnY, lnX] = (terrainTypes[lnMarker] == 6 ? -1 : 0);
							}

							// 1 = North, 2 = east, 4 = south, 8 = west
							if (direction % 8 >= 4 && lnY <= yMax)
							{
								if (validPoint(lnX, lnY + 1, zoneToUse))
								{
									if (zoneToUse == -1000)
									{
										if (map2[lnY + 1, lnX] == 0)
											totalLand++;
										map2[lnY + 1, lnX] = terrainTypes[lnMarker];
										island2[lnY + 1, lnX] = (terrainTypes[lnMarker] == 6 ? -1 : 0);
									}
									else
									{
										if (map[lnY + 1, lnX] == 0)
											totalLand++;
										map[lnY + 1, lnX] = terrainTypes[lnMarker];
										island[lnY + 1, lnX] = (terrainTypes[lnMarker] == 6 ? -1 - zoneToUse : zoneToUse);
									}

									newPoints.Add(lnX);
									newPoints.Add(lnY + 1);
								}
							}
							if (direction % 2 >= 1 && lnY >= yMin)
							{
								if (validPoint(lnX, lnY - 1, zoneToUse))
								{
									if (zoneToUse == -1000)
									{
										if (map2[lnY - 1, lnX] == 0)
											totalLand++;
										map2[lnY - 1, lnX] = terrainTypes[lnMarker];
										island2[lnY - 1, lnX] = (terrainTypes[lnMarker] == 6 ? -1 : 0);
									}
									else
									{
										if (map[lnY - 1, lnX] == 0)
											totalLand++;
										map[lnY - 1, lnX] = terrainTypes[lnMarker];
										island[lnY - 1, lnX] = (terrainTypes[lnMarker] == 6 ? -1 - zoneToUse : zoneToUse);
									}
									newPoints.Add(lnX);
									newPoints.Add(lnY - 1);
								}
							}
							if (direction % 4 >= 2 && lnX <= xMax)
							{
								if (validPoint(lnX + 1, lnY, zoneToUse))
								{
									if (zoneToUse == -1000)
									{
										if (map2[lnY, lnX + 1] == 0)
											totalLand++;
										map2[lnY, lnX + 1] = terrainTypes[lnMarker];
										island2[lnY, lnX + 1] = (terrainTypes[lnMarker] == 6 ? -1 : 0);
									}
									else
									{
										if (map[lnY, lnX + 1] == 0)
											totalLand++;
										map[lnY, lnX + 1] = terrainTypes[lnMarker];
										island[lnY, lnX + 1] = (terrainTypes[lnMarker] == 6 ? -1 - zoneToUse : zoneToUse);
									}
									newPoints.Add(lnX + 1);
									newPoints.Add(lnY);
								}
							}
							if (direction % 16 >= 8 && lnX >= xMin)
							{
								if (validPoint(lnX - 1, lnY, zoneToUse))
								{
									if (zoneToUse == -1000)
									{
										if (map2[lnY, lnX - 1] == 0)
											totalLand++;
										map2[lnY, lnX - 1] = terrainTypes[lnMarker];
										island2[lnY, lnX - 1] = (terrainTypes[lnMarker] == 6 ? -1 : 0);
									}
									else
									{
										if (map[lnY, lnX - 1] == 0)
											totalLand++;
										map[lnY, lnX - 1] = terrainTypes[lnMarker];
										island[lnY, lnX - 1] = (terrainTypes[lnMarker] == 6 ? -1 - zoneToUse : zoneToUse);
									}
									newPoints.Add(lnX - 1);
									newPoints.Add(lnY);
								}
							}

							int takeaway = 1 + (direction > 8 ? 1 : 0) + (direction % 8 > 4 ? 1 : 0) + (direction % 4 > 2 ? 1 : 0) + (direction % 2 > 1 ? 1 : 0);
							sizeToUse--;
						}
						if (sizeToUse <= 0) break;
						if (newPoints.Count != 0)
							points = newPoints;
					}
				}
			}

			// Fill in water...
			List<int> land = new List<int> { 1, 2, 3, 4, 5, 6, 7 };
			if (zoneToUse != -1000)
			{
				for (int lnY = 0; lnY < 256; lnY++)
					for (int lnX = 0; lnX < 256; lnX++)
					{
						if (island[lnY, lnX] == zoneToUse && island[lnY, lnX + 1] == zoneToUse && island[lnY, lnX + 2] == zoneToUse && island[lnY, lnX + 3] == zoneToUse)
						{
							if (map[lnY, lnX] == map[lnY, lnX + 2] && map[lnY, lnX] != map[lnY, lnX + 1])
							{
								map[lnY, lnX + 1] = map[lnY, lnX];
								island[lnY, lnX + 1] = island[lnY, lnX];
							}
							if (lnX < 254 && land.Contains(map[lnY, lnX]) && !land.Contains(map[lnY, lnX + 1]) && !land.Contains(map[lnY, lnX + 2]) && land.Contains(map[lnY, lnX + 3]))
							{
								map[lnY, lnX + 1] = map[lnY, lnX];
								map[lnY, lnX + 2] = map[lnY, lnX + 3];
								island[lnY, lnX + 1] = island[lnY, lnX];
								island[lnY, lnX + 2] = island[lnY, lnX + 3];
							}
						}
					}

				markIslands(zoneToUse);
			}
			else
			{
				for (int lnY = 0; lnY < 136; lnY++)
					for (int lnX = 0; lnX < 156; lnX++)
					{
						if (map2[lnY, lnX] == map2[lnY, lnX + 2] && map2[lnY, lnX] != map2[lnY, lnX + 1])
						{
							map2[lnY, lnX + 1] = map2[lnY, lnX];
							island2[lnY, lnX + 1] = island2[lnY, lnX];
						}
						if (lnX < 149 && land.Contains(map2[lnY, lnX]) && !land.Contains(map2[lnY, lnX + 1]) && !land.Contains(map2[lnY, lnX + 2]) && land.Contains(map2[lnY, lnX + 3]))
						{
							map2[lnY, lnX + 1] = map2[lnY, lnX];
							map2[lnY, lnX + 2] = map2[lnY, lnX + 3];
							island2[lnY, lnX + 1] = island2[lnY, lnX];
							island2[lnY, lnX + 2] = island2[lnY, lnX + 3];
						}
					}
			}
		}

		private void smoothMap()
		{
			// Remove one byte lands
			for (int lnX = 0; lnX < 254; lnX++)
				for (int lnY = 0; lnY < 256; lnY++)
				{
					if (map[lnY, lnX] != map[lnY, lnX + 1] && map[lnY, lnX + 1] != map[lnY, lnX + 2] && island[lnY, lnX] == island[lnY, lnX + 2])
					{
						map[lnY, lnX + 1] = map[lnY, lnX];
						island[lnY, lnX + 1] = island[lnY, lnX];
					}
				}

			int smoothRequirement = 10;
			bool badMap = true;

			while (badMap)
			{
				// Let's PRETEND to enter this into the ROM...
				int lnPointer = 0x821a;

				for (int lnI = 0; lnI <= 256; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map.
				{
					int lnJ = 0;
					while (lnI < 256 && lnJ < 256)
					{
						if (map[lnI, lnJ] >= 0 && map[lnI, lnJ] <= 7)
						{
							int tileNumber = 0;
							int numberToMatch = map[lnI, lnJ];
							while (lnJ < 256 && tileNumber < (numberToMatch == 7 ? 8 : 32) && map[lnI, lnJ] == numberToMatch)
							{
								tileNumber++;
								lnJ++;
							}
							lnPointer++;
						}
						else
						{
							lnPointer++;
							lnJ++;
						}
					}
				}
				//lnPointer = lnPointer;
				if (lnPointer <= 0x9a94 - 320)
					badMap = false;
				else // Time to remove small areas of stuff to hopefully compress the map further.
				{
					//MessageBox.Show("Map too big; " + (lnPointer - (0x9a94 - 256)).ToString() + " bytes too big");

					int lastTile = 0x00;
					int lastIsland = island[0, 0];
					for (int lnY = 0; lnY < 255; lnY++)
						for (int lnX = 0; lnX < 255; lnX++)
						{
							smoothPlot(lnX, lnY, smoothRequirement, lastTile, lastIsland);
							lastTile = map[lnY, lnX];
							lastIsland = island[lnY, lnX];
						}

					smoothRequirement += 10;
				}
			}
		}

		private void smoothMap2()
		{
			// Remove one byte lands
			for (int lnX = 0; lnX < 156; lnX++)
				for (int lnY = 0; lnY < 139; lnY++)
				{
					if (map2[lnY, lnX] != map2[lnY, lnX + 1] && map2[lnY, lnX + 1] != map2[lnY, lnX + 2] && island2[lnY, lnX] == island2[lnY, lnX + 2])
					{
						map2[lnY, lnX + 1] = map2[lnY, lnX];
						island2[lnY, lnX + 1] = island2[lnY, lnX];
					}
				}

			int smoothRequirement = 10;
			bool badMap = true;

			while (badMap)
			{
				// Let's PRETEND to enter this into the ROM...
				int lnPointer = 0x9bab;

				for (int lnI = 0; lnI <= 138; lnI++) // <---- There is a final pointer for lnI = 256, probably indicating the conclusion of the map2.
				{
					int lnJ = 0;
					while (lnI < 139 && lnJ < 158)
					{
						if (map2[lnI, lnJ] >= 0 && map2[lnI, lnJ] <= 7)
						{
							int tileNumber = 0;
							int numberToMatch = map2[lnI, lnJ];
							while (lnJ < 158 && tileNumber < (numberToMatch == 7 ? 8 : 32) && map2[lnI, lnJ] == numberToMatch)
							{
								tileNumber++;
								lnJ++;
							}
							lnPointer++;
						}
						else
						{
							lnPointer++;
							lnJ++;
						}
					}
				}
				//lnPointer = lnPointer;
				if (lnPointer <= 0xa3ee - 80)
					badMap = false;
				else // Time to remove small areas of stuff to hopefully compress the map further.
				{
					//MessageBox.Show("Map too big; " + (lnPointer - (0x9a94 - 256)).ToString() + " bytes too big");

					int lastTile = 0x00;
					int lastIsland = island2[0, 0];
					for (int lnY = 0; lnY < 139; lnY++)
						for (int lnX = 0; lnX < 158; lnX++)
						{
							smoothPlot2(lnX, lnY, smoothRequirement, lastTile, lastIsland);
							lastTile = map2[lnY, lnX];
							lastIsland = island2[lnY, lnX];
						}

					smoothRequirement += 10;
				}
			}

		}

		private void smoothPlot(int initX, int initY, int minimum, int fillTile, int fillIsland)
		{
			//if (y == 30 && x == 137)
			//    y = y;

			int x = initX;
			int y = initY;

			bool[,] plotted = new bool[256, 256];
			int landTile = map[y, x];

			bool first = true;
			List<int> toPlot = new List<int>();
			int plots = 0;
			int toFill = 0;
			while (toFill < 2)
			{
				while (first || toPlot.Count != 0)
				{
					if (!first)
					{
						y = toPlot[0];
						toPlot.RemoveAt(0);
						x = toPlot[0];
						toPlot.RemoveAt(0);
					}
					else
						first = false;

					if (toFill == 1)
					{
						map[y, x] = fillTile;
						island[y, x] = fillIsland;
					}

					for (int dir = 0; dir < 5; dir++)
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

						if (map[dirY, dirX] == landTile && !plotted[dirY, dirX])
						{
							plots++;
							plotted[dirY, dirX] = true;

							if (plots > minimum)
								return;

							if (dir != 0)
							{
								toPlot.Add(dirY);
								toPlot.Add(dirX);
							}
						}
					}
				}

				toFill++;
				x = initX;
				y = initY;
				first = true;
			}
		}

		private void smoothPlot2(int initX, int initY, int minimum, int fillTile, int fillIsland)
		{
			//if (y == 30 && x == 137)
			//    y = y;

			int x = initX;
			int y = initY;

			bool[,] plotted = new bool[139, 158];
			int landTile = map2[y, x];

			bool first = true;
			List<int> toPlot = new List<int>();
			int plots = 0;
			int toFill = 0;
			while (toFill < 2)
			{
				while (first || toPlot.Count != 0)
				{
					if (!first)
					{
						y = toPlot[0];
						toPlot.RemoveAt(0);
						x = toPlot[0];
						toPlot.RemoveAt(0);
					}
					else
						first = false;

					if (toFill == 1)
					{
						map2[y, x] = fillTile;
						island2[y, x] = fillIsland;
					}

					for (int dir = 0; dir < 5; dir++)
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 158 ? 0 : dirX == -1 ? 157 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 139 ? 0 : dirY == -1 ? 138 : dirY);

						if (map2[dirY, dirX] == landTile && !plotted[dirY, dirX])
						{
							plots++;
							plotted[dirY, dirX] = true;

							if (plots > minimum)
								return;

							if (dir != 0)
							{
								toPlot.Add(dirY);
								toPlot.Add(dirX);
							}
						}
					}
				}

				toFill++;
				x = initX;
				y = initY;
				first = true;
			}
		}

		private bool validPoint(int x, int y, int zoneToUse)
		{
			if (zoneToUse == -1000) return true;
			int zoneSize = (chkSmallMap.Checked ? 8 : 16);
			// Establish zone
			int zoneX = x / zoneSize;
			int zoneY = y / zoneSize;
			int zoneSides = zone[zoneY, zoneX] % 1000;
			//if (zone[zoneY, zoneX] % 1000 != 0) return false;
			if (zone[zoneY, zoneX] / 1000 != zoneToUse / 1000) return false;
			// 1 = north, 2 = east, 4 = south, 8 = west
			if (y % zoneSize == 0 && zoneSides % 2 == 1) return false;
			if (x % zoneSize == zoneSize - 1 && zoneSides % 4 >= 2) return false;
			if (y % zoneSize == zoneSize - 1 && zoneSides % 8 >= 4) return false;
			if (x % zoneSize == 0 && zoneSides % 16 >= 8) return false;

			return true;
		}

		private void markIslands(int zoneToUse)
		{
			if (zoneToUse != -1000)
			{
				// We should mark islands and inaccessible land...
				int landNumber = zoneToUse + 1;
				int maxLand = -2;

				int maxLandPlots = 0;
				int lastIsland = 0;
				for (int lnI = 0; lnI < 256; lnI++)
					for (int lnJ = 0; lnJ < 256; lnJ++)
					{
						if (island[lnI, lnJ] == zoneToUse && map[lnI, lnJ] != 0x06)
						{
							int plots = landPlot(landNumber, lnI, lnJ, zoneToUse);
							if (plots > maxLandPlots)
							{
								maxLandPlots = plots;
								maxLand = landNumber;
							}
							islands.Add(landNumber);
							landNumber++;

							lastIsland = island[lnI, lnJ];
						}
					}

				maxIsland[zoneToUse / 1000] = maxLand;
			}
			else
			{
				// We should mark islands and inaccessible land...
				int landNumber = 1;

				for (int lnI = 0; lnI < 139; lnI++)
					for (int lnJ = 0; lnJ < 158; lnJ++)
					{
						if (island2[lnI, lnJ] == 0 && map2[lnI, lnJ] != 0x06)
						{
							int plots = landPlot(landNumber, lnI, lnJ, zoneToUse);
							landNumber++;
						}
					}
			}
		}

		private void resetIslands()
		{
			for (int y = 0; y < 256; y++)
				for (int x = 0; x < 256; x++)
				{
					if (island[y, x] != 200 && island[y, x] != -1)
					{
						island[y, x] /= 1000;
						island[y, x] *= 1000;
					}
				}

			islands.Clear();

			markIslands(1000);
			markIslands(2000);
			markIslands(0);
			markIslands(-1000);
		}

		private void createBridges(Random r1)
		{
			List<BridgeList> bridgePossible = new List<BridgeList>();
			List<islandLinks> islandPossible = new List<islandLinks>();
			// Create bridges for points two spaces or less from two distinctly numbered islands.  Extend islands if there is interference.
			// 0x00 = Water (was 0x04 in DW2)
			// 0x06 = Mountain (was 0x05 in DW2)
			for (int y = 1; y < 252; y++)
				for (int x = 1; x < 252; x++)
				{
					if (y == 63 && x == 8) map[y, x] = map[y, x];
					if (map[y, x] == 0x06 || map[y, x] == 0x00) continue;

					for (int lnI = 2; lnI <= 4; lnI++)
					{
						if (island[y, x] != island[y + lnI, x] && island[y, x] / 1000 == island[y + lnI, x] / 1000 && map[y + lnI, x] != 0x00 && map[y + lnI, x] != 0x06)
						{
							bool fail = false;
							for (int lnJ = 1; lnJ < lnI; lnJ++)
							{
								if (map[y + lnJ, x] == 0x00)
								{
									map[y + lnJ, x - 1] = 0x00; map[y + lnJ, x + 1] = 0x00;
									island[y + lnJ, x - 1] = 0x00; island[y + lnJ, x + 1] = 0x00;
								}
								else
								{
									fail = true;
								}
							}
							if (!fail)
							{
								bridgePossible.Add(new BridgeList(x, y, true, lnI, island[y, x], island[y + lnI, x]));
								if (islandPossible.Where(c => c.island1 == island[y, x] && c.island2 == island[y + lnI, x]).Count() == 0)
									islandPossible.Add(new islandLinks(island[y, x], island[y + lnI, x]));
							}
						}

						if (island[y, x] != island[y, x + lnI] && island[y, x] / 1000 == island[y, x + lnI] / 1000 && map[y, x + lnI] != 0x00 && map[y, x + lnI] != 0x06)
						{
							bool fail = false;
							for (int lnJ = 1; lnJ < lnI; lnJ++)
							{
								if (map[y, x + lnJ] == 0x00)
								{
									map[y - 1, x + lnJ] = 0x00; map[y + 1, x + lnJ] = 0x00;
									island[y - 1, x + lnJ] = 200; island[y + 1, x + lnJ] = 200;
								}
								else
								{
									fail = true;
								}
							}
							if (!fail)
							{
								bridgePossible.Add(new BridgeList(x, y, false, lnI, island[y, x], island[y, x + lnI]));
								if (islandPossible.Where(c => c.island1 == island[y, x] && c.island2 == island[y, x + lnI]).Count() == 0)
									islandPossible.Add(new islandLinks(island[y, x], island[y, x + lnI]));
							}
						}
					}
				}

			foreach (islandLinks islandLink in islandPossible)
			{
				List<BridgeList> test = bridgePossible.Where(c => c.island1 == islandLink.island1 && c.island2 == islandLink.island2).ToList();
				// Choose one bridge out of the possibilities
				BridgeList bridgeToBuild = test[r1.Next() % test.Count];
				for (int lnI = 1; lnI <= bridgeToBuild.distance - 1; lnI++)
				{
					if (bridgeToBuild.south)
					{
						map[bridgeToBuild.y + lnI, bridgeToBuild.x] = (lnI == bridgeToBuild.distance - 1 ? 0xff : map[bridgeToBuild.y, bridgeToBuild.x]);
						island[bridgeToBuild.y + lnI, bridgeToBuild.x] = bridgeToBuild.island1;

						if (map[bridgeToBuild.y + lnI + 1, bridgeToBuild.x] == 0x00 && lnI == bridgeToBuild.distance - 1)
						{
							bridgeToBuild.distance++;
							map[bridgeToBuild.y + lnI + 1, bridgeToBuild.x - 1] = 0x00; map[bridgeToBuild.y + lnI + 1, bridgeToBuild.x + 1] = 0x00;
							island[bridgeToBuild.y + lnI + 1, bridgeToBuild.x - 1] = 0x00; island[bridgeToBuild.y + lnI + 1, bridgeToBuild.x + 1] = 0x00;
						}
					}
					else
					{
						map[bridgeToBuild.y, bridgeToBuild.x + lnI] = (lnI == bridgeToBuild.distance - 1 ? 0xfb : map[bridgeToBuild.y, bridgeToBuild.x]);
						island[bridgeToBuild.y, bridgeToBuild.x + lnI] = bridgeToBuild.island1;

						if (map[bridgeToBuild.y, bridgeToBuild.x + lnI + 1] == 0x00 && lnI == bridgeToBuild.distance - 1)
						{
							bridgeToBuild.distance++;
							map[bridgeToBuild.y - 1, bridgeToBuild.x + lnI + 1] = 0x00; map[bridgeToBuild.y + 1, bridgeToBuild.x + lnI + 1] = 0x00;
							island[bridgeToBuild.y - 1, bridgeToBuild.x + lnI + 1] = 200; island[bridgeToBuild.y + 1, bridgeToBuild.x + lnI + 1] = 200;
						}
					}
				}
			}
		}

		private class islandLinks
		{
			public int island1;
			public int island2;

			public islandLinks(int pI1, int pI2)
			{
				island1 = pI1; island2 = pI2;
			}
		}

		private class BridgeList
		{
			public int x;
			public int y;
			public bool south;
			public int distance;
			public int island1;
			public int island2;

			public BridgeList(int pX, int pY, bool pS, int pDist, int pI1, int pI2)
			{
				x = pX; y = pY; south = pS; distance = pDist; island1 = pI1; island2 = pI2;
			}
		}

		private bool createZone(int zoneNumber, int size, bool rectangle, Random r1)
		{
			int tries = 1000;
			bool firstZone = true;

			if (!rectangle)
			{
				while (size > 0 && tries > 0)
				{
					int x = r1.Next() % 16;
					int y = r1.Next() % 16;
					int minX = x, maxX = x, minY = y, maxY = y;
					if (!firstZone && zone[x, y] != zoneNumber)
					{
						continue;
					}
					if (firstZone)
					{
						firstZone = false;
						zone[x, y] = zoneNumber;
					}

					tries--;
					int direction = r1.Next() % 16;
					int totalDirections = 0;
					if (direction % 16 >= 8) totalDirections++;
					if (direction % 8 >= 4) totalDirections++;
					if (direction % 4 >= 2) totalDirections++;
					if (direction % 2 >= 1) totalDirections++;
					if (totalDirections > size) continue;

					// 1 = north, 2 = east, 4 = south, 8 = west
					if (direction % 16 >= 8 && x != 0 && zone[x - 1, y] == 0 && (minX <= (x - 1) || maxX - minX <= 11))
					{
						zone[x - 1, y] = zoneNumber;
						minX = (x - 1 < minX ? x - 1 : minX);
						size--;
						tries = 100;
					}
					if (direction % 8 >= 4 && y != 15 && zone[x, y + 1] == 0 && (maxY >= (y + 1) || maxY - minY <= 11))
					{
						zone[x, y + 1] = zoneNumber;
						maxY = (y + 1 > maxY ? y + 1 : maxY);
						size--;
						tries = 100;
					}
					if (direction % 4 >= 2 && x != 15 && zone[x + 1, y] == 0 && (minX >= (x + 1) || maxX - minX <= 11))
					{
						zone[x + 1, y] = zoneNumber;
						maxX = (x + 1 > maxX ? x + 1 : maxX);
						size--;
						tries = 100;
					}
					if (direction % 2 >= 1 && y != 0 && zone[x, y - 1] == 0 && (minY <= (y - 1) || maxY - minY <= 11))
					{
						zone[x, y - 1] = zoneNumber;
						minY = (y - 1 < minY ? y - 1 : minY);
						size--;
						tries = 100;
					}
				}
				return (size <= 0);
			}
			else
			{
				int minMeasurement = (int)Math.Ceiling((double)size / 12);
				int maxMeasurement = (int)Math.Ceiling((double)size / minMeasurement);

				int length = ((r1.Next() % (maxMeasurement - minMeasurement)) + minMeasurement);
				int width = size / length;

				int x = (r1.Next() % (16 - length));
				int y = (r1.Next() % (16 - width));

				for (int i = x; i < x + length; i++)
					for (int j = y; j < y + width; j++)
						zone[i, j] = zoneNumber;

				return true;
			}
		}

		private bool reachable(int startY, int startX, bool water, int finishX, int finishY, int maxLake, bool alefgard)
		{
			int x = startX;
			int y = startY;

			List<int> validPlots = new List<int> { 1, 2, 3, 4, 5, 7, 0xe8, 0xe9, 0xea, 0xeb, 0xec, 0xed, 0xef, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xfb, 0xff };
			if (water) validPlots.Add(0);

			bool first = true;
			List<int> toPlot = new List<int>();

			if (alefgard)
			{
				bool[,] plotted = new bool[139, 158];

				while (first || toPlot.Count != 0)
				{
					if (!first)
					{
						y = toPlot[0];
						toPlot.RemoveAt(0);
						x = toPlot[0];
						toPlot.RemoveAt(0);
					}
					else
					{
						first = false;
					}

					for (int dir = 0; dir < 5; dir++)
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 158 ? 0 : dirX == -1 ? 157 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 139 ? 0 : dirY == -1 ? 138 : dirY);

						if (validPlots.Contains(map2[dirY, dirX]) && (map2[dirY, dirX] != 0 || island2[dirY, dirX] == maxLake))
						{
							if (dir != 0 && plotted[dirY, dirX] == false)
							{
								if (finishX == dirX && finishY == dirY)
									return true;
								toPlot.Add(dirY);
								toPlot.Add(dirX);
								plotted[dirY, dirX] = true;
							}
						}
					}
				}

				return false;
			}
			else
			{
				bool[,] plotted = new bool[256, 256];

				while (first || toPlot.Count != 0)
				{
					if (!first)
					{
						y = toPlot[0];
						toPlot.RemoveAt(0);
						x = toPlot[0];
						toPlot.RemoveAt(0);
					}
					else
					{
						first = false;
					}

					for (int dir = 0; dir < 5; dir++)
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

						if (validPlots.Contains(map[dirY, dirX]) && (map[dirY, dirX] != 0 || island[dirY, dirX] == maxLake))
						{
							if (dir != 0 && plotted[dirY, dirX] == false)
							{
								if (finishX == dirX && finishY == dirY)
									return true;
								toPlot.Add(dirY);
								toPlot.Add(dirX);
								plotted[dirY, dirX] = true;
							}
						}
					}
				}

				return false;
			}
		}

		private int landPlot(int landNumber, int y, int x, int zoneToUse = 0)
		{
			bool first = true;
			List<int> toPlot = new List<int>();
			int plots = 1;
			while (first || toPlot.Count != 0)
			{
				if (!first)
				{
					y = toPlot[0];
					toPlot.RemoveAt(0);
					x = toPlot[0];
					toPlot.RemoveAt(0);
				}
				else
				{
					first = false;
				}

				for (int dir = 0; dir < 5; dir++)
				{
					if (zoneToUse != -1000)
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

						if (island[dirY, dirX] == zoneToUse)
						{
							plots++;
							island[dirY, dirX] = landNumber;

							if (dir != 0)
							{
								toPlot.Add(dirY);
								toPlot.Add(dirX);
							}
						}
					}
					else
					{
						int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
						dirX = (dirX == 158 ? 0 : dirX == -1 ? 157 : dirX);
						int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
						dirY = (dirY == 138 ? 0 : dirY == -1 ? 137 : dirY);

						if (island2[dirY, dirX] == 0)
						{
							plots++;
							island2[dirY, dirX] = landNumber;

							if (dir != 0)
							{
								toPlot.Add(dirY);
								toPlot.Add(dirX);
							}
						}
					}
				}
			}

			return plots;
		}

		private bool validPlot(int y, int x, int height, int width, int[] legalIsland)
		{
			if (legalIsland[0] == 60000)
			{
				for (int lnI = 0; lnI < height; lnI++)
					for (int lnJ = 0; lnJ < width; lnJ++)
					{
						if (y + lnI >= 137 || x + lnJ >= 156) return false;

						int legalY = y + lnI;
						int legalX = x + lnJ;

						if (map2[legalY, legalX] == 0x00 || map2[legalY, legalX] == 0x06 || map2[legalY, legalX] >= 0xe8 || map[legalY, legalX] >= 0xe8) // LAST CONDITION:  Castles, towns, villages, etc - Need to not match BOTH maps!
							return false;
					}
			}
			else
			{
				for (int lnI = 0; lnI < height; lnI++)
					for (int lnJ = 0; lnJ < width; lnJ++)
					{
						if (y + lnI >= (chkSmallMap.Checked ? 128 : 256) || x + lnJ >= (chkSmallMap.Checked ? 128 : 256)) return false;

						int legalY = (y + lnI >= 256 ? y - 256 + lnI : y + lnI);
						int legalX = (x + lnJ >= 256 ? x - 256 + lnJ : x + lnJ);

						bool ok = false;
						for (int lnK = 0; lnK < legalIsland.Length; lnK++)
							if (island[legalY, legalX] == legalIsland[lnK])
								ok = true;
						if (!ok) return false;
						if (legalY < 139 && legalX < 158)
						{
							if (map[legalY, legalX] == 0x00 || map[legalY, legalX] == 0x06 || map[legalY, legalX] >= 0xe8 || map2[legalY, legalX] >= 0xe8) // LAST CONDITION:  Castles, towns, villages, etc - Need to not match BOTH maps!
								return false;
						}
						else
						{
							if (map[legalY, legalX] == 0x00 || map[legalY, legalX] == 0x06 || map[legalY, legalX] >= 0xe8) // LAST CONDITION:  Castles, towns, villages, etc
								return false;
						}
					}
			}
			return true;
		}

		private int lakePlot(int lakeNumber, int y, int x, bool fill = false, int islandNumber = -1)
		{
			bool first = true;
			List<int> toPlot = new List<int>();
			int plots = 1;
			//if (islandNumber >= 0) plots = 1;
			while (first || toPlot.Count != 0)
			{
				if (!first)
				{
					y = toPlot[0];
					toPlot.RemoveAt(0);
					x = toPlot[0];
					toPlot.RemoveAt(0);
				}
				else
				{
					if (fill)
						map[y, x] = (islandNumber == 0 ? 0x02 : islandNumber == 1 ? 0x03 : islandNumber == 2 ? 0x04 : islandNumber == 3 ? 0x01 : 0x05);
					first = false;
				}

				for (int dir = 0; dir < 5; dir++)
				{
					int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
					dirX = (dirX == 256 ? 0 : dirX == -1 ? 255 : dirX);
					int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
					dirY = (dirY == 256 ? 0 : dirY == -1 ? 255 : dirY);

					if (island[dirY, dirX] == -1 || (island[dirY, dirX] == lakeNumber && fill))
					{
						plots++;
						island[dirY, dirX] = (fill ? islandNumber : lakeNumber);
						if (fill)
							map[dirY, dirX] = (islandNumber == 0 ? 0x02 : islandNumber == 1 ? 0x03 : islandNumber == 2 ? 0x04 : islandNumber == 3 ? 0x01 : 0x05);

						if (dir != 0)
						{
							toPlot.Add(dirY);
							toPlot.Add(dirX);
						}
						//plots += lakePlot(lakeNumber, y, x, fill);
					}
				}
			}

			return plots;
		}

		private int lakePlot2(int lakeNumber, int y, int x, bool fill = false, int islandNumber = -1)
		{
			bool first = true;
			List<int> toPlot = new List<int>();
			int plots = 1;
			//if (islandNumber >= 0) plots = 1;
			while (first || toPlot.Count != 0)
			{
				if (!first)
				{
					y = toPlot[0];
					toPlot.RemoveAt(0);
					x = toPlot[0];
					toPlot.RemoveAt(0);
				}
				else
				{
					if (fill)
						map2[y, x] = (islandNumber == 0 ? 0x02 : islandNumber == 1 ? 0x03 : islandNumber == 2 ? 0x04 : islandNumber == 3 ? 0x01 : 0x05);
					first = false;
				}

				for (int dir = 0; dir < 5; dir++)
				{
					int dirX = (dir == 4 ? x - 1 : dir == 2 ? x + 1 : x);
					dirX = (dirX == 158 ? 0 : dirX == -1 ? 157 : dirX);
					int dirY = (dir == 1 ? y - 1 : dir == 3 ? y + 1 : y);
					dirY = (dirY == 139 ? 0 : dirY == -1 ? 138 : dirY);

					if (island2[dirY, dirX] == -1 || (island2[dirY, dirX] == lakeNumber && fill))
					{
						plots++;
						island2[dirY, dirX] = (fill ? islandNumber : lakeNumber);
						if (fill)
							map2[dirY, dirX] = (islandNumber == 0 ? 0x02 : islandNumber == 1 ? 0x03 : islandNumber == 2 ? 0x04 : islandNumber == 3 ? 0x01 : 0x05);

						if (dir != 0)
						{
							toPlot.Add(dirY);
							toPlot.Add(dirX);
						}
						//plots += lakePlot(lakeNumber, y, x, fill);
					}
				}
			}

			return plots;
		}

		private void shipPlacement(int byteToUse, int top, int left, int maxLake = 0)
		{
			int minDirection = -99;
			int minDistance = 999;
			int finalX = 0;
			int finalY = 0;
			int distance = 0;
			int lnJ = top;
			int lnK = left;
			for (int lnI = 0; lnI < 4; lnI++)
			{
				lnJ = top;
				lnK = left;
				if (lnI == 0)
				{
					while (island[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnJ = (lnJ == 0 ? 255 : lnJ - 1);
					}
				}
				else if (lnI == 1)
				{
					while (island[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnJ = (lnJ == 255 ? 0 : lnJ + 1);
					}
				}
				else if (lnI == 2)
				{
					while (island[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnK = (lnK == 255 ? 0 : lnK + 1);
					}
				}
				else
				{
					while (island[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnK = (lnK == 0 ? 255 : lnK - 1);
					}
				}
				if (distance < minDistance)
				{
					minDistance = distance;
					minDirection = lnI;
					finalX = lnK;
					finalY = lnJ;
				}
				distance = 0;
			}
			romData[byteToUse] = (byte)(finalX);
			romData[byteToUse + 1] = (byte)(finalY);
			if (minDirection == 0)
			{
				lnJ = (finalY == 255 ? 0 : finalY + 1);
				while (map[lnJ, finalX] == 0x06)
				{
					map[lnJ, finalX] = 0x05;
					lnJ = (lnJ == 255 ? 0 : lnJ + 1);
				}
			}
			else if (minDirection == 1)
			{
				lnJ = (finalY == 0 ? 255 : finalY - 1);
				while (map[lnJ, finalX] == 0x06)
				{
					map[lnJ, finalX] = 0x05;
					lnJ = (lnJ == 0 ? 255 : lnJ - 1);
				}
			}
			else if (minDirection == 2)
			{
				lnK = (finalX == 0 ? 255 : finalX - 1);
				while (map[finalY, lnK] == 0x06)
				{
					map[finalY, lnK] = 0x05;
					lnK = (lnK == 0 ? 255 : lnK - 1);
				}
			}
			else
			{
				lnK = (finalX == 255 ? 0 : finalX + 1);
				while (map[finalY, lnK] == 0x06)
				{
					map[finalY, lnK] = 0x05;
					lnK = (lnK == 255 ? 0 : lnK + 1);
				}
			}
		}

		private void shipPlacement2(int byteToUse, int top, int left, int maxLake = 0)
		{
			int minDirection = -99;
			int minDistance = 999;
			int finalX = 0;
			int finalY = 0;
			int distance = 0;
			int lnJ = top;
			int lnK = left;
			for (int lnI = 0; lnI < 4; lnI++)
			{
				lnJ = top;
				lnK = left;
				if (lnI == 0)
				{
					while (island2[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnJ = (lnJ == 0 ? 138 : lnJ - 1);
					}
				}
				else if (lnI == 1)
				{
					while (island2[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnJ = (lnJ == 138 ? 0 : lnJ + 1);
					}
				}
				else if (lnI == 2)
				{
					while (island2[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnK = (lnK == 157 ? 0 : lnK + 1);
					}
				}
				else
				{
					while (island2[lnJ, lnK] != maxLake && distance < 200)
					{
						distance++;
						lnK = (lnK == 0 ? 157 : lnK - 1);
					}
				}
				if (distance < minDistance)
				{
					minDistance = distance;
					minDirection = lnI;
					finalX = lnK;
					finalY = lnJ;
				}
				distance = 0;
			}
			romData[byteToUse] = (byte)(finalX);
			romData[byteToUse + 1] = (byte)(finalY);
			if (minDirection == 0)
			{
				lnJ = (finalY == 255 ? 0 : finalY + 1);
				while (map2[lnJ, finalX] == 0x06)
				{
					map2[lnJ, finalX] = 0x05;
					lnJ = (lnJ == 255 ? 0 : lnJ + 1);
				}
			}
			else if (minDirection == 1)
			{
				lnJ = (finalY == 0 ? 255 : finalY - 1);
				while (map2[lnJ, finalX] == 0x06)
				{
					map2[lnJ, finalX] = 0x05;
					lnJ = (lnJ == 0 ? 255 : lnJ - 1);
				}
			}
			else if (minDirection == 2)
			{
				lnK = (finalX == 0 ? 255 : finalX - 1);
				while (map2[finalY, lnK] == 0x06)
				{
					map2[finalY, lnK] = 0x05;
					lnK = (lnK == 0 ? 255 : lnK - 1);
				}
			}
			else
			{
				lnK = (finalX == 255 ? 0 : finalX + 1);
				while (map2[finalY, lnK] == 0x06)
				{
					map2[finalY, lnK] = 0x05;
					lnK = (lnK == 255 ? 0 : lnK + 1);
				}
			}
		}


		private void boostGP()
		{
			// Replace monster data
			for (int lnI = 0; lnI < 139; lnI++)
			{
				int byteValStart = 0x32e3 + (23 * lnI);

				int gp = romData[byteValStart + 4] + ((romData[byteValStart + 18] % 2) * 256);
				switch (cboGoldReq.SelectedIndex)
				{
					case 0:
						gp *= 2;
						break;
					case 1:
						gp = gp * 3 / 2;
						break;
					case 2:
						break;
					case 3:
						gp /= 2;
						break;
				}
				gp = (gp > 1000 ? 1000 : gp);

				romData[byteValStart + 4] = (byte)(gp % 256);
				romData[byteValStart + 18] = (byte)(romData[byteValStart + 18] - (romData[byteValStart + 18] % 4) + (gp / 256));
			}
		}

		private void boostXP()
		{
			// Replace monster data
			for (int lnI = 0; lnI < 139; lnI++)
			{
				int byteValStart = 0x32e3 + (23 * lnI);

				int xp = romData[byteValStart + 1] + (romData[byteValStart + 2] * 256);
				switch (cboExpGains.SelectedIndex)
				{
					case 0:
						xp *= 5;
						break;
					case 1:
						xp *= 4;
						break;
					case 2:
						xp *= 3;
						break;
					case 3:
						xp *= 2;
						break;
					case 4:
						xp = xp * 3 / 2;
						break;
					case 5:
						break;
					case 6:
						xp /= 2;
						break;
					case 7:
						xp /= 4;
						break;
				}
				xp = (xp > 65500 ? 65500 : xp);

				romData[byteValStart + 1] = (byte)(xp % 256);
				romData[byteValStart + 2] = (byte)(xp / 256);
			}
		}

		private void adjustEncounters()
		{
			for (int i = 0x944; i <= 0x955; i++)
			{
				switch (cboEncounterRate.SelectedIndex)
				{
					case 0:
						romData[i] *= 4;
						break;
					case 1:
						romData[i] *= 3;
						break;
					case 2:
						romData[i] *= 2;
						break;
					case 3:
						romData[i] = (byte)(romData[i] * 3 / 2);
						break;
					case 4:
						break;
					case 5:
						romData[i] = (byte)(romData[i] * 3 / 4);
						break;
					case 6:
						romData[i] /= 2;
						break;
					case 7:
						romData[i] /= 4;
						break;
				}
			}
		}

		private void forceItemSell()
		{
			int[] forcedItemSell = { 0x16, 0x1c, 0x28, 0x32, 0x34, 0x36, 0x3b, 0x3f, 0x42, 0x48, 0x4b, 0x4c, 0x50, 0x52, 0x53, 0x58, 0x59, 0x69, 0x6f, 0x70, 0x71 };
			for (int lnI = 0; lnI < forcedItemSell.Length; lnI++)
				if (romData[0x11be + forcedItemSell[lnI]] % 32 >= 16) // Not allowed to be sold
					romData[0x11be + forcedItemSell[lnI]] -= 16; // Now allowed to be sold!

			int[] itemstoAdjust = { 0x16, 0x1c, 0x28, 0x32, 0x34, 0x36, 0x3b, 0x3f, 0x42, 0x48, 0x4b, 0x4c, 0x50, 0x52, 0x53, 0x58, 0x59, 0x5a, 0x69, 0x6f, 0x70, 0x71, // forced items to sell AND...
               0x5f, 0x60, 0x62, 0x64, 0x57, 0x75, 0x55, 0x4e, 0x4f, 0x49 }; // Some other items I want sold (see above)

			int[] itemPriceAdjust = { 5000, 35000, 15000, 10000, 8000, 12000, 10000, 800, 10, 5000, 5000, 8000, 20000, 1000, 1000, 500, 2000, 5000, 5000, 500, 2000, 500,
				5000, 3000, 2500, 5000, 800, 10000, 3000, 2000, 10000, 5000, 1000 };

			for (int lnI = 0; lnI < itemstoAdjust.Length; lnI++)
			{
				// Remove any price adjustment first.
				romData[0x11be + itemstoAdjust[lnI]] -= (byte)(romData[0x11be + itemstoAdjust[lnI]] % 4);
				int priceToUse = (romData[0x123b + itemstoAdjust[lnI]] >= 128 ? romData[0x123b + itemstoAdjust[lnI]] - 128 : romData[0x123b + itemstoAdjust[lnI]]);
				if (itemPriceAdjust[lnI] >= 10000)
				{
					romData[0x11be + itemstoAdjust[lnI]] += 3; // Now multiply by 1000
					romData[0x123b + itemstoAdjust[lnI]] = (byte)(romData[0x123b + itemstoAdjust[lnI]] >= 128 ? (itemPriceAdjust[lnI] / 1000) + 128 : itemPriceAdjust[lnI] / 1000);
				}
				else if (itemPriceAdjust[lnI] >= 1000)
				{
					romData[0x11be + itemstoAdjust[lnI]] += 2; // Now multiply by 100
					romData[0x123b + itemstoAdjust[lnI]] = (byte)(romData[0x123b + itemstoAdjust[lnI]] >= 128 ? (itemPriceAdjust[lnI] / 100) + 128 : itemPriceAdjust[lnI] / 100);
				}
				else if (itemPriceAdjust[lnI] >= 100)
				{
					romData[0x11be + itemstoAdjust[lnI]] += 1; // Now multiply by 10
					romData[0x123b + itemstoAdjust[lnI]] = (byte)(romData[0x123b + itemstoAdjust[lnI]] >= 128 ? (itemPriceAdjust[lnI] / 10) + 128 : itemPriceAdjust[lnI] / 10);
				}
				else
				{
					romData[0x123b + itemstoAdjust[lnI]] = (byte)(romData[0x123b + itemstoAdjust[lnI]] >= 128 ? itemPriceAdjust[lnI] + 128 : itemPriceAdjust[lnI]);
				}
			}
		}

		private void superRandomize()
		{
			Random r1;
			try
			{
				r1 = new Random(int.Parse(txtSeed.Text));
			}
			catch
			{
				MessageBox.Show("Invalid seed.  It must be a number from 0 to 2147483648.");
				return;
			}

			if (chkRandomizeMap.Checked)
			{
				randomizeMapv5(r1);
			}

			if (chkRandEnemyPatterns.Checked)
			{
				byte[] monsterSize = { 8, 4, 4, 4, 4, 4, 7, 4, 4, 8, 4, 4, 4, 2, 4, 4,
				4, 4, 5, 5, 2, 4, 4, 5, 4, 4, 4, 4, 4, 4, 3, 2,
				4, 4, 4, 2, 4, 5, 4, 4, 4, 4, 4, 8, 4, 4, 4, 3,
				2, 8, 4, 3, 4, 4, 2, 3, 4, 7, 3, 4, 2, 4, 4, 7,
				8, 3, 3, 4, 3, 2, 3, 4, 4, 4, 4, 4, 4, 3, 3, 4,
				2, 4, 3, 4, 3, 2, 2, 4, 3, 2, 2, 3, 2, 5, 1, 4,
				3, 3, 2, 3, 4, 1, 3, 3, 8, 7, 4, 2, 7, 4, 3, 2,
				3, 3, 3, 3, 3, 3, 3, 4, 4, 2, 1, 2, 4, 2, 3, 3,
				3, 1, 1, 3, 1, 1, 1, 2, 3, 3, 4 };

				// Totally randomize monsters (13805-13cd2)
				for (int lnI = 0; lnI < 0x8a; lnI++)
				{
					if (lnI == 0x85 || lnI == 0x86)
						continue; // Do not adjust either Zoma.

					//0 - Monster Level (probably used for running away)
					//1 - EXP
					//2 - EXP * 256
					//3 - Agility
					//4 - GP
					//5 - Attack
					//6 - Defense
					//7 - HP
					//8 - MP
					//9 - Item dropped
					//10 = Action 1
					//11 = Action 2(first half related to "AI-Lv)
					//12 = Action 3
					//13 = Action 4(first half related to "Pattern")
					//14 = Action 5(related to # atks, first bit)
					//15 = Action 6(also related to # atks, first bit)
					//16 = Action 7[1] = related to regen
					//17 = Action 8[1] = also related to regen 
					//18 - Bits 0-1 - GPx256, Bits 2-3 - Infernos resist, Bits 4-5 - Ice resist, Bits 6-7 - Fire resist
					//19 - Bits 0-1 - Attackx256, 2-3 - Sacrifice resist, 4-5 - Beat resist, 6-7 - Lightning resist
					//20 - Bits 0-1 - Defx256, 2-3 - Defense resist, 4-5 - Stopspell resist, 6-7 - Sleep resist
					//21 - Bits 0-1 - HPx256, 2-3 - Chaos resist, 4-5 - RobMagic resist, 6-7 - Surround resist
					//22 - Bits 0-3 - Drop chance (1/1, 8, 16, 32, 64, 128, 256, and 2048), 4-5 - Expel resist, 6-7 - Limbo/Slow resist
					byte[] enemyStats = { romData[0x32e3 + (lnI * 23) + 0], romData[0x32e3 + (lnI * 23) + 1], romData[0x32e3 + (lnI * 23) + 2], romData[0x32e3 + (lnI * 23) + 3], romData[0x32e3 + (lnI * 23) + 4],
					romData[0x32e3 + (lnI * 23) + 5], romData[0x32e3 + (lnI * 23) + 6], romData[0x32e3 + (lnI * 23) + 7], romData[0x32e3 + (lnI * 23) + 8], romData[0x32e3 + (lnI * 23) + 9],
					romData[0x32e3 + (lnI * 23) + 10], romData[0x32e3 + (lnI * 23) + 11], romData[0x32e3 + (lnI * 23) + 12], romData[0x32e3 + (lnI * 23) + 13], romData[0x32e3 + (lnI * 23) + 14],
					romData[0x32e3 + (lnI * 23) + 15], romData[0x32e3 + (lnI * 23) + 16], romData[0x32e3 + (lnI * 23) + 17], romData[0x32e3 + (lnI * 23) + 18], romData[0x32e3 + (lnI * 23) + 19],
					romData[0x32e3 + (lnI * 23) + 20], romData[0x32e3 + (lnI * 23) + 21], romData[0x32e3 + (lnI * 23) + 22] };

					int byteValStart = 0x32e3 + (23 * lnI);

					for (int lnJ = 3; lnJ <= 7; lnJ++)
					{
						int totalAtk = enemyStats[lnJ] + ((enemyStats[lnJ + 14] % 4) * 256);
						if (lnJ == 3) totalAtk = enemyStats[lnJ];
						if (lnJ == 7 && lnI == 0x87) totalAtk = 5; // We want Ortega to die quickly by giving him 5 HP.
						if (lnJ == 5 && lnI == 0x87) totalAtk = 2000; // ... or win the battle quickly by giving him hoards of strength!  (he still winds up "dead" I think)

						// Potentially add quadruple the possible gold for each monster.  Average 2 1/2 times...
						if (lnJ == 4 && totalAtk > 0)
							totalAtk += (r1.Next() % (totalAtk * 3));
						else
						{
							int atkRandom = (r1.Next() % 3);
							int atkDiv2 = (totalAtk / 2) + 1;
							if (atkRandom == 1)
								totalAtk += (r1.Next() % atkDiv2);
							else if (atkRandom == 2)
								totalAtk -= (r1.Next() % atkDiv2);
						}

						totalAtk = (totalAtk < 1 ? 1 : totalAtk);
						totalAtk = (totalAtk > 1020 ? 1020 : totalAtk);
						if (lnJ == 3)
							totalAtk = (totalAtk > 255 ? 255 : totalAtk);
						enemyStats[lnJ] = (byte)(totalAtk % 256);
						if (lnJ > 3)
							enemyStats[lnJ + 14] = (byte)(enemyStats[lnJ + 14] - (enemyStats[lnJ + 14] % 4) + (totalAtk / 256));
					}
					if (enemyStats[8] <= 16 && r1.Next() % 2 == 1) enemyStats[8] = (byte)(r1.Next() % 32);
					//enemyStats[8] = 255; // Always make sure the monster has MP

					// Needs to be a "legal treasure..."
					byte[] legalMonsterTreasures = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
									0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
									0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
									0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
									0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x48, 0x49, 0x4b, 0x4c, 0x4e,
									0x50, 0x55, 0x56, 0x5f,
									0x60, 0x62, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6c, 0x6d,
									0x70, 0x71, 0x73, 0x74,
									0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74, 0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74,
									0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74, 0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74 };
					enemyStats[9] = (legalMonsterTreasures[r1.Next() % legalMonsterTreasures.Length]);

					byte[] res1 = { 0, 0, 0, 0, 0, 1, 2, 3 };
					byte[] res2 = { 0, 0, 0, 0, 1, 1, 2, 3 };
					byte[] res3 = { 0, 0, 0, 1, 1, 2, 2, 3 };
					byte[] res4 = { 0, 0, 1, 1, 2, 2, 3, 3 };
					byte[] res5 = { 0, 1, 1, 2, 2, 3, 3, 3 };
					byte[] res6 = { 0, 1, 2, 2, 3, 3, 3, 3 };
					byte[] res7 = { 0, 1, 2, 3, 3, 3, 3, 3 };
					byte[] finalRes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
					for (int lnJ = 0; lnJ < finalRes.Length; lnJ++)
					{
						if (lnI < 25)
							finalRes[lnJ] = (res1[r1.Next() % 8]);
						else if (lnI < 50)
							finalRes[lnJ] = (res2[r1.Next() % 8]);
						else if (lnI < 70)
							finalRes[lnJ] = (res3[r1.Next() % 8]);
						else if (lnI < 90)
							finalRes[lnJ] = (res4[r1.Next() % 8]);
						else if (lnI < 105)
							finalRes[lnJ] = (res5[r1.Next() % 8]);
						else if (lnI < 115)
							finalRes[lnJ] = (res6[r1.Next() % 8]);
						else
							finalRes[lnJ] = (res7[r1.Next() % 8]);
					}

					enemyStats[18] = (byte)(enemyStats[18] % 4 + (finalRes[0] * 4) + (finalRes[1] * 16) + (finalRes[2] * 64));
					enemyStats[19] = (byte)(enemyStats[19] % 4 + (finalRes[3] * 4) + (finalRes[4] * 16) + (finalRes[5] * 64));
					enemyStats[20] = (byte)(enemyStats[20] % 4 + (finalRes[6] * 4) + (finalRes[7] * 16) + (finalRes[8] * 64));
					enemyStats[21] = (byte)(enemyStats[21] % 4 + (finalRes[9] * 4) + (finalRes[10] * 16) + (finalRes[11] * 64));
					// First part:  item drop chance.  Make sure it's at best 1/8.
					if (lnI == 0x36 || lnI == 0x62) // EXCEPT Man-eater Chests and Mimics
						enemyStats[22] = (byte)(0 + (finalRes[12] * 16) + (finalRes[13] * 64));
					else
						enemyStats[22] = (byte)(((r1.Next() % 7) + 1) + (finalRes[12] * 16) + (finalRes[13] * 64));

					byte[] enemyPatterns = { 2, 2, 2, 2, 2, 2, 2, 2 };

					// Types of patterns... 0:  Attack only, 1:  "Goofy attack", 2:  Totally random, 3:  Annoying, 4:  Quite annyoing, 5:  Hell monster
					byte[] pattern1 = { 45, 65, 100, 100, 100 };
					byte[] pattern2 = { 35, 60, 90, 100, 100 };
					byte[] pattern3 = { 25, 50, 80, 90, 100 };
					byte[] pattern4 = { 15, 45, 75, 85, 100 };
					byte[] pattern5 = { 10, 40, 70, 85, 100 };
					byte[] pattern6 = { 5, 30, 70, 80, 100 };
					byte[] pattern7 = { 0, 20, 60, 80, 100 };
					byte[] pattern8 = { 0, 10, 50, 60, 100 };
					byte[] pattern9 = { 0, 0, 15, 30, 100 };

					int enemyPattern = r1.Next() % 100;

					if (lnI < 15 || lnI == 0x87 || lnI == 0x68) // Ortega, so he dies quickly, and red slime, because that monster is WAY out of order
						enemyPattern = (enemyPattern < pattern1[0] ? 0 : enemyPattern < pattern1[1] ? 1 : enemyPattern < pattern1[2] ? 2 : enemyPattern < pattern1[3] ? 3 : 4);
					else if (lnI < 30)
						enemyPattern = (enemyPattern < pattern2[0] ? 0 : enemyPattern < pattern2[1] ? 1 : enemyPattern < pattern2[2] ? 2 : enemyPattern < pattern2[3] ? 3 : 4);
					else if (lnI < 45 || lnI == 0x88 || lnI == 0x8a) // Kandar 1 and Kandar Henchman
						enemyPattern = (enemyPattern < pattern3[0] ? 0 : enemyPattern < pattern3[1] ? 1 : enemyPattern < pattern3[2] ? 2 : enemyPattern < pattern3[3] ? 3 : 4);
					else if (lnI < 60)
						enemyPattern = (enemyPattern < pattern4[0] ? 0 : enemyPattern < pattern4[1] ? 1 : enemyPattern < pattern4[2] ? 2 : enemyPattern < pattern4[3] ? 3 : 4);
					else if (lnI < 75 || lnI == 0x89) // Kandar 2
						enemyPattern = (enemyPattern < pattern5[0] ? 0 : enemyPattern < pattern5[1] ? 1 : enemyPattern < pattern5[2] ? 2 : enemyPattern < pattern5[3] ? 3 : 4);
					else if (lnI < 90)
						enemyPattern = (enemyPattern < pattern6[0] ? 0 : enemyPattern < pattern6[1] ? 1 : enemyPattern < pattern6[2] ? 2 : enemyPattern < pattern6[3] ? 3 : 4);
					else if (lnI < 105)
						enemyPattern = (enemyPattern < pattern7[0] ? 0 : enemyPattern < pattern7[1] ? 1 : enemyPattern < pattern7[2] ? 2 : enemyPattern < pattern7[3] ? 3 : 4);
					else if (lnI < 120)
						enemyPattern = (enemyPattern < pattern8[0] ? 0 : enemyPattern < pattern8[1] ? 1 : enemyPattern < pattern8[2] ? 2 : enemyPattern < pattern8[3] ? 3 : 4);
					else
						enemyPattern = (enemyPattern < pattern9[0] ? 0 : enemyPattern < pattern9[1] ? 1 : enemyPattern < pattern9[2] ? 2 : enemyPattern < pattern9[3] ? 3 : 4);

					switch (enemyPattern)
					{
						case 0: // leave everything alone; it's a basic attack monster.
							break;
						case 1: // Give the monster a little goofyness to their attack...
							for (int lnJ = 0; lnJ < 8; lnJ++)
							{
								// 50% chance of setting a different attack.
								byte[] attackPattern = { 2, 2, 2, 2, 2, 0, 1, 3, 4, 5, 6, 8 };
								byte random = (attackPattern[r1.Next() % attackPattern.Length]);
								if (random != 2)
									enemyPatterns[lnJ] = random;
							}
							break;
						case 2:
							for (int lnJ = 0; lnJ < 8; lnJ++)
							{
								// 75% chance of setting a different attack.
								byte random = (byte)(r1.Next() % 80);
								if (random != 2 && random < 64 && random != 0x2b)
									enemyPatterns[lnJ] = random;
							}
							break;
						case 3:
							for (int lnJ = 0; lnJ < 8; lnJ++)
							{
								// Normal, heroic, poison, faint, heal, healmore (both self and others), sleep, stopspell, weak flames, 
								// poison and sweet breaths, call for help, double attacks, and strange jigs.
								byte[] attackPattern = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 3, 4, 5, 6, 8, 9, 10, 13, 16, 17, 19, 22, 23, 28, 34, 35, 36, 38, 39, 41, 45, 49, 54, 59 };
								byte random = (attackPattern[r1.Next() % attackPattern.Length]);
								if (random != 2 && random < 64)
									enemyPatterns[lnJ] = random;
							}
							break;
						case 4:
							for (int lnJ = 0; lnJ < 8; lnJ++)
							{
								byte[] attackPattern = { 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 6, 6, 8, 11, 12, 14, 15, 18, 20, 21, 24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 40, 40, 42, 44, 47, 48, 51, 53, 56, 58, 60 };
								byte random = (attackPattern[r1.Next() % attackPattern.Length]);
								if (random != 2 && random < 64)
									enemyPatterns[lnJ] = random;
							}
							break;
					}

					if (lnI == 0x31 || lnI == 0x6c) // Metal slime, Metal Babble
					{
						enemyPatterns[0] = 7; // run away
						enemyPatterns[1] = 7; // run away
						enemyPatterns[2] = 7; // run away
						enemyPatterns[3] = 7; // run away
						if (lnI == 0x41)
						{
							enemyPatterns[4] = 7; // run away
							enemyPatterns[5] = 7; // run away
						}
					}

					// Both bits set = 2 attacks guaranteed.  2nd bit set = up to 3 attacks.  1st bit set = up to 2 attacks.
					int badChance = (3 * lnI > 300 ? 300 : 3 * lnI);
					if (r1.Next() % 1000 < badChance / 4)
						enemyPatterns[5] += 128;
					else if (r1.Next() % 1000 < badChance / 3)
					{
						enemyPatterns[4] += 128;
						enemyPatterns[5] += 128;
					}
					else if (r1.Next() % 1000 < badChance)
						enemyPatterns[4] += 128;

					// Repeat for regeneration.  Both bits = 100 HP / round, 2nd bit = 50 HP / round, 3rd bit = 25 HP / round
					if (r1.Next() % 1000 < badChance / 3)
					{
						enemyPatterns[6] += 128;
						enemyPatterns[7] += 128;
					}
					else if (r1.Next() % 1000 < badChance / 2)
						enemyPatterns[7] += 128;
					else if (r1.Next() % 1000 < badChance)
						enemyPatterns[6] += 128;

					for (int lnJ = 0; lnJ < 8; lnJ++)
						enemyStats[10 + lnJ] = (enemyPatterns[lnJ]);

					for (int lnJ = 0; lnJ < 23; lnJ++)
						romData[byteValStart + lnJ] = enemyStats[lnJ];
				}
			}

			if (chkRandMonsterZones.Checked)
			{
				// Aliahan 1, 2, 3, Promontory Cave, Tower of Najimi B, 1, 2, Aliahan 4, Enticement Cave 1, 2, Romaly, Kanave, Champange Tower, Noaniels, Dream Cave, Assaram, Isis 1, 2, Pyramid 1, 2, 3
				List<int> gentleZones = new List<int>() { 4, 5, 6, 65, 66, 67, 68, 7, 69, 70, 8, 9, 71, 72, 10, 74, 75, 12, 13, 14, 76, 77, 80 };
				List<int> violentZone1 = new List<int>() { 78, 48, 79, 81 }; // Cave of Necrogund
				List<int> violentZone2 = new List<int>() { 82, 39, 11 }; // Baramos Castle
				List<int> violentZone3 = new List<int>() { 64, 50, 51, 52, 54, 55, 57, 58, 60, 61, 63, 59, 62, 40, 53, 56 };  // Tantegel overworld, caves, and towers
				List<int> violentZone4 = new List<int>() { 25, 34, 38, 63 }; // Zoma's Castle
																			 // Totally randomize monster zones
				for (int lnI = 0; lnI < 95; lnI++)
				{
					int byteToUse = 0xaeb + (lnI * 15);
					bool nonViolent = false;
					for (int lnJ = 1; lnJ < 13; lnJ++)
					{
						if (gentleZones.IndexOf(lnI) != -1)
							romData[byteToUse + lnJ] = monsterOrder[r1.Next() % ((gentleZones.IndexOf(lnI) * 2) + 8)];
						else if (violentZone1.Contains(lnI))
							romData[byteToUse + lnJ] = monsterOrder[(r1.Next() % 92) + 40];
						else if (violentZone2.Contains(lnI))
							romData[byteToUse + lnJ] = monsterOrder[(r1.Next() % 72) + 60];
						else if (violentZone3.Contains(lnI))
							romData[byteToUse + lnJ] = monsterOrder[(r1.Next() % 56) + 80];
						else if (violentZone4.Contains(lnI))
							romData[byteToUse + lnJ] = monsterOrder[(r1.Next() % 37) + 99];
						else
						{
							romData[byteToUse + lnJ] = monsterOrder[r1.Next() % 131];
							nonViolent = true;
						}
					}
					if (nonViolent && r1.Next() % 3 == 1)
					{
						romData[byteToUse + 13] = (byte)(r1.Next() % 20);
						romData[byteToUse + 14] = (byte)(r1.Next() % 20);
					}
				}

				// Randomize the 19 special battles
				for (int lnI = 0; lnI < 20; lnI++)
				{
					int byteToUse = 0x107a + (6 * lnI);
					for (int lnJ = 0; lnJ < 4; lnJ++)
					{
						if (r1.Next() % 2 == 1 || lnJ == 3)
							romData[byteToUse + lnJ] = monsterOrder[r1.Next() % 129];
					}
				}

				// Not sure we can really randomize boss fights... (ff separates boss fights - 0x8ee-0x918 AND 0x919-0x944)
				// But I can change the Mummy Men treasure fights to Shadow fights!
				romData[0x909] = 0x18; // was 0x20 - Mummy Men
									   // We could randomize the Granite Titan and Boss Troll fights too...
									   // Maybe remove two of the Kandar Henchmen in the first fight and place two "bonus monsters" in other fights...
			}

			//if (chkRandItemEffects.Checked)
			//{
			//    // Randomize which items equate to which effects
			//    // Select 21 items randomly from a set defined as follows:
			//    int[] legalEffectItems = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
			//                          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
			//                          0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
			//                          0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
			//                          0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46 };

			//    List<int> keyEffectItems = new List<int> { 0x10, 0x11 };

			//    // Wipe out the use byte by totally resetting the price.
			//    for (int lnI = 0; lnI < legalEffectItems.Length; lnI++)
			//    {
			//        int oldVal = romData[0x11be + legalEffectItems[lnI]];
			//        romData[0x11be + legalEffectItems[lnI]] = (byte)(oldVal % 32);
			//        //romData[0x11be + legalEffectItems[lnI]] = (byte)(oldVal % 32 >= 16 ? 0x10 : 0x00);
			//        //romData[0x11be + legalEffectItems[lnI]] += (byte)(oldVal % 16 >= 8 ? 0x08 : 0x00);
			//    }
			//    int oldVal1 = romData[0x11be + 0x4a];
			//    romData[0x11be + 0x4a] = (byte)(oldVal1 % 32);
			//    int oldVal2 = romData[0x11be + 0x5b];
			//    romData[0x11be + 0x5b] = (byte)(oldVal2 % 32);

			//    int[] legalItemSpells = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
			//                          0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
			//                          0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e,
			//                          0x30, 0x31, 0x32, 0x34,
			//                          0x38, 0x39, 0x3a }; // restore MP, everyone sneezes, self numb - 54 spells total

			//    List<int> enemyGroupSpells = new List<int> { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x09, 0x0a, 0x0b, 0x0d, 0x0e, 0x0f,
			//                           0x10, 0x12, 0x13, 0x15, 0x16, 0x17, 0x18, 0x22, 0x24, 0x25, 0x27, 0x2b, 0x2c }; // 25
			//    List<int> enemyAllSpells = new List<int> { 0x06, 0x07, 0x08, 0x0c, 0x11, 0x14, 0x39 }; // 7
			//    List<int> allySelfSpells = new List<int> { 0x1a, 0x1b, 0x1c, 0x23, 0x29, 0x2d, 0x30, 0x32, 0x34, 0x38, 0x3a }; // 11
			//    List<int> allySelectSpells = new List<int> { 0x20, 0x21, 0x28 }; // 3
			//    List<int> allyAllSpells = new List<int> { 0x19, 0x1d, 0x1e, 0x1f, 0x26, 0x2a, 0x2e, 0x31 }; // 8

			//    for (int lnI = 0; lnI < 21; lnI++)
			//    {
			//        int effectItem = legalEffectItems[r1.Next() % legalEffectItems.Length];
			//        if (romData[0x11be + effectItem] >= 0x80) // If it's already been selected...
			//        {
			//            lnI--;
			//        }
			//        else
			//        {
			//            romData[0x11be + effectItem] += 0x80;
			//        }
			//    }

			//    int iSpell = -1;
			//    for (int lnI = 0; lnI < legalEffectItems.Length; lnI++)
			//    {
			//        // Otherwise, randomize the spell it will be using.
			//        if (romData[0x11be + legalEffectItems[lnI]] < 0x80)
			//            continue;

			//        int effectSpell = legalItemSpells[r1.Next() % legalItemSpells.Length];
			//        if (effectSpell == 0x38 && keyEffectItems.Contains(effectSpell)) // Can't let a key item potentially crumble!  Redo that randomization.
			//        {
			//            lnI--;
			//            continue;
			//        }

			//        iSpell++;
			//        // Now determine what spell it is... that will determine whether to "attack" yourself, a group of monsters, a selected ally, or all monsters/allies.
			//        if (enemyGroupSpells.Contains(effectSpell))
			//            romData[0x11be + legalEffectItems[lnI]] += 0x60;
			//        else if (enemyAllSpells.Contains(effectSpell))
			//            romData[0x11be + legalEffectItems[lnI]] += 0x20;
			//        else if (allySelfSpells.Contains(effectSpell)) // 50/50 chance of targetting for self or an ally.
			//            romData[0x11be + legalEffectItems[lnI]] += (byte)(r1.Next() % 2 == 1 ? 0x00 : 0x40);
			//        else if (allySelectSpells.Contains(effectSpell))
			//            romData[0x11be + legalEffectItems[lnI]] += 0x40;
			//        else if (allyAllSpells.Contains(effectSpell))
			//            romData[0x11be + legalEffectItems[lnI]] += 0x00;

			//        romData[0x13280 + iSpell] = (byte)effectSpell;
			//    }
			//}

			if (chkRandEquip.Checked)
			{
				// Totally randomize weapons, armor, shields, helmets (13efb-13f1d, 1a00e-1a08b for pricing)
				for (int lnI = 0; lnI <= 70; lnI++)
				{
					byte power = 0;

					if (lnI == 0 || lnI == 1 || lnI == 2 || lnI == 32 || lnI == 34 || lnI == 48)
						power = (byte)(r1.Next() % 12);
					else if (lnI < 31)
						power = (byte)(Math.Pow(r1.Next() % 1000, 2.5) / 243252); // max 130
					else if (lnI < 55)
						power = (byte)(Math.Pow(r1.Next() % 1000, 2.5) / 395284); // max 80
					else if (lnI < 62)
						power = (byte)(Math.Pow(r1.Next() % 1000, 2.5) / 574959); // max 55
					else
						power = (byte)(Math.Pow(r1.Next() % 1000, 2.5) / 903507); // max 35
					power += 2; // To avoid 0 power...
					romData[0x279a0 + lnI] = power;

					// You want a max price of about 20000, shields 18300, helmets 15000
					double price = Math.Round((lnI < 31 ? Math.Pow(power, 2.04) : lnI < 55 ? Math.Pow(power, 2.26) : lnI < 62 ? Math.Pow(power, 2.45) : Math.Pow(power, 2.7)), 0);
					// TO DO:  Round to the nearest 10 (after 100GP), 50(after 1000 GP), or 100 (after 2500 GP)
					price = (float)Math.Round(price, 0);

					//// Remove any price adjustment first.
					romData[0x11be + lnI] -= (byte)(romData[0x11be + lnI] % 4);
					if (price >= 10000)
					{
						romData[0x11be + lnI] += 3; // Now multiply by 1000
						price /= 1000;
					}
					else if (price >= 1000)
					{
						romData[0x11be + lnI] += 2; // Now multiply by 100
						price /= 100;
					}
					else if (price >= 100)
					{
						romData[0x11be + lnI] += 1; // Now multiply by 10
						price /= 10;
					}
					else
					{
						romData[0x11be + lnI] += 0;
					}

					// Must keep special effects if romData is >= 128
					if (lnI < 80)
					{
						if (romData[0x123b + lnI] >= 128)
							romData[0x123b + lnI] = (byte)(128 + price);
						else
							romData[0x123b + lnI] = (byte)(price);

						if (lnI <= 2)
						{
							if ((romData[0x123b + lnI] % 16) >= 8)
								romData[0x123b + lnI] -= (byte)((romData[0x123b + lnI] % 8) + 1);
						}
					}
				}
			}

			if (chkRandEquip.Checked || chkRandItemEffects.Checked || chkRandWhoCanEquip.Checked)
			{
				string finalFile = Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW3Random_" + txtSeed.Text + "_" + txtFlags.Text + "_guide.txt");

				// Totally randomize who can equip (1a3ce-1a3f0).  At least one person can equip something...
				using (StreamWriter writer = File.CreateText(finalFile))
				{
					string[] weaponText = { "Cypress stick", "Club", "Copper sword", "Magic Knife", "Iron Spear", "Battle Axe", "Broad Sword", "Wizard's Wand",
						"Poison Needle", "Iron Claw", "Thorn Whip", "Giant Shears", "Chain Sickle", "Thor's Sword", "Snowblast Sword", "Demon Axe",
						"Staff of Rain", "Sword of Gaia", "Staff of Reflection", "Sword of Destruction", "Multi - Edge Sword", "Staff of Force", "Sword of Illusion", "Zombie Slasher",
						"Falcon Sword", "Sledge Hammer", "Thunder Sword", "Staff of Thunder", "Sword of Kings", "Orochi Sword", "Dragon Killer", "Staff of Judgement",
						"Clothes", "Training Suit", "Leather Armor", "Flashy Clothes", "Half Plate Armor", "Full Plate Armor", "Magic Armor", "Cloak of Evasion",
						"Armor of Radiance", "Iron Apron", "Animal Suit", "Fighting Suit", "Sacred Robe", "Armor of Hades", "Water Flying Cloth", "Chain Mail",
						"Wayfarers Clothes", "Revealing Swimsuit", "Magic Bikini", "Shell Armor", "Armor of Terrafirma", "Dragon Mail", "Swordedge Armor", "Angel's Robe",
						"Leather Shield", "Iron Shield", "Shield of Strength", "Shield of Heroes", "Shield of Sorrow", "Bronze Shield", "Silver Shield", "Golden Crown",
						"Iron Helmet", "Mysterious Hat", "Unlucky Helmet", "Turban", "Noh Mask", "Leather Helmet", "Iron Mask", "Golden Claw" };

					for (int lnI = 0; lnI <= 70; lnI++)
					{
						if (chkRandWhoCanEquip.Checked)
						{
							// Maintain equipment requirements for the starting equipment
							if (!(lnI == 0x00 || lnI == 0x01 || lnI == 0x02 || lnI == 0x20 || lnI == 0x22 || lnI == 0x30))
								romData[0x1147 + lnI] = (byte)(r1.Next() % 255 + 1);

							// EXCEPT those that are "FF", update the "who can use the item" to the people who are allowed to equip the item
							if (romData[0x1196 + lnI] != 255 && romData[0x1196 + lnI] != 0 && lnI < 32)
								romData[0x1196 + lnI] = romData[0x1147 + lnI];
						}

						string equipOut = "";
						equipOut += (romData[0x1147 + lnI] % 2 >= 1 ? "Hr  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 32 >= 16 ? "Sr  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 8 >= 4 ? "Pr  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 4 >= 2 ? "Wi  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 16 >= 8 ? "Sg  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 128 >= 64 ? "Fi  " : "--  ");
						equipOut += (romData[0x1147 + lnI] % 64 >= 32 ? "Mr  " : "--  ");
						equipOut += (romData[0x1147 + lnI] >= 128 ? "Gf  " : "--  ");
						equipOut += (romData[0x11be + lnI] >= 128 ? "**  " : "    ");
						equipOut += (romData[0x279a0 + lnI]);
						writer.WriteLine(weaponText[lnI].PadRight(24) + equipOut);
					}
				}

				// Remove the lines that penalize a fighter for not equipping claws.
				romData[0x1507] = romData[0x1508] = romData[0x1509] = romData[0x150a] = 0xea;
			}

			if (chkRandSpellLearning.Checked)
			{
				// Totally randomize spell learning
				// First, clear out all of the magic bytes...
				for (int lnI = 0; lnI < 252; lnI++)
					romData[0x29d6 + lnI] = 0x3f;

				// There are 64 fight spells overall, and 24 command spells overall.  Make sure that each fight spell is in the final list, then scramble after that.  Make sure there are no more than three copies of a spell, 
				// make sure there are no duplicates in blocks 0-15, 16-39, and 40-63.  Any command spells that duplicate the fight spells should be placed in their respective blocks.
				int[] finalFight = new int[64];
				int[] finalCommand = new int[24];
				for (int i = 0; i < finalFight.Length; i++) finalFight[i] = -1;
				for (int i = 0; i < finalCommand.Length; i++) finalCommand[i] = -1;

				int[] fightSpells = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 48, 49, 50, 51, 52, 53 }; // 52 (12-20-20)
				int[] commandSpells = { 26, 27, 28, 30, 31, 32, 33, 38, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61 }; // 18 (6-6-6)
				for (int lnI = 0; lnI < fightSpells.Length * 20; lnI++)
					swapArray(fightSpells, (r1.Next() % fightSpells.Length), (r1.Next() % fightSpells.Length));
				for (int lnI = 0; lnI < commandSpells.Length * 20; lnI++)
					swapArray(commandSpells, (r1.Next() % commandSpells.Length), (r1.Next() % commandSpells.Length));

				int[] heroFight2 = new int[16];
				int[] pilgrimFight2 = new int[24];
				int[] wizardFight2 = new int[24];

				for (int lnI = 0; lnI < 52; lnI++)
				{
					if (lnI < 12) heroFight2[lnI] = fightSpells[lnI];
					else if (lnI < 32) pilgrimFight2[lnI - 12] = fightSpells[lnI];
					else wizardFight2[lnI - 32] = fightSpells[lnI];
				}

				for (int lnI = 12; lnI < 16; lnI++)
				{
					heroFight2[lnI] = fightSpells[r1.Next() % fightSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (heroFight2[lnJ] == heroFight2[lnI])
						{
							lnI--;
							break;
						}
				}
				for (int lnI = 20; lnI < 24; lnI++)
				{
					pilgrimFight2[lnI] = fightSpells[r1.Next() % fightSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (pilgrimFight2[lnJ] == pilgrimFight2[lnI])
						{
							lnI--;
							break;
						}
				}
				for (int lnI = 20; lnI < 24; lnI++)
				{
					wizardFight2[lnI] = fightSpells[r1.Next() % fightSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (wizardFight2[lnJ] == wizardFight2[lnI])
						{
							lnI--;
							break;
						}
				}

				int[] heroCommand2 = new int[8];
				int[] pilgrimCommand2 = new int[8];
				int[] wizardCommand2 = new int[8];

				for (int lnI = 0; lnI < 18; lnI++)
				{
					if (lnI < 6) heroCommand2[lnI] = commandSpells[lnI];
					else if (lnI < 12) pilgrimCommand2[lnI - 6] = commandSpells[lnI];
					else wizardCommand2[lnI - 12] = commandSpells[lnI];
				}

				for (int lnI = 6; lnI < 8; lnI++)
				{
					heroCommand2[lnI] = commandSpells[r1.Next() % commandSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (heroCommand2[lnJ] == heroCommand2[lnI])
						{
							lnI--;
							break;
						}
				}
				for (int lnI = 6; lnI < 8; lnI++)
				{
					pilgrimCommand2[lnI] = commandSpells[r1.Next() % commandSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (pilgrimCommand2[lnJ] == pilgrimCommand2[lnI])
						{
							lnI--;
							break;
						}
				}
				for (int lnI = 6; lnI < 8; lnI++)
				{
					wizardCommand2[lnI] = commandSpells[r1.Next() % commandSpells.Length];
					for (int lnJ = 0; lnJ < lnI; lnJ++)
						if (wizardCommand2[lnJ] == wizardCommand2[lnI])
						{
							lnI--;
							break;
						}
				}

				int[] heroFightLevels = inverted_power_curve(1, 35, 24, 1, r1);
				int[] pilgrimFightLevels = inverted_power_curve(1, 35, 24, 1, r1);
				int[] wizardFightLevels = inverted_power_curve(1, 35, 24, 1, r1);
				int[] heroCommandLevels = inverted_power_curve(1, 35, 8, 1, r1);
				int[] pilgrimCommandLevels = inverted_power_curve(1, 35, 8, 1, r1);
				int[] wizardCommandLevels = inverted_power_curve(1, 35, 8, 1, r1);

				for (int lnI = 0; lnI < 8; lnI++)
				{
					romData[0x29d6 + heroCommand2[lnI]] = (byte)heroCommandLevels[lnI]; // (byte)(r1.Next() % 35 + 1);
					romData[0x2a15 + pilgrimCommand2[lnI]] = (byte)pilgrimCommandLevels[lnI]; // (r1.Next() % 35 + 1);
					romData[0x2a54 + wizardCommand2[lnI]] = (byte)wizardCommandLevels[lnI]; // (r1.Next() % 35 + 1);
					romData[0x2a93 + pilgrimCommand2[lnI]] = romData[0x2a15 + pilgrimCommand2[lnI]];
					romData[0x2a93 + wizardCommand2[lnI]] = romData[0x2a54 + wizardCommand2[lnI]];
					romData[0x22e7 + 24 + lnI] = (byte)heroCommand2[lnI];
					romData[0x22e7 + 32 + 24 + lnI] = (byte)pilgrimCommand2[lnI];
					romData[0x22e7 + 64 + 24 + lnI] = (byte)wizardCommand2[lnI];
				}

				romData[0x29d6 + 63 + romData[0x22e7 + 32 + 24]] = 1;
				romData[0x29d6 + 126 + romData[0x22e7 + 64 + 24]] = 1;

				for (int lnI = 0; lnI < 24; lnI++)
				{
					if (lnI < 16)
						romData[0x29d6 + heroFight2[lnI]] = (byte)heroFightLevels[lnI]; // (byte)(r1.Next() % 35 + 1);
					romData[0x2a15 + pilgrimFight2[lnI]] = (byte)pilgrimFightLevels[lnI]; // (byte)(r1.Next() % 35 + 1);
					romData[0x2a54 + wizardFight2[lnI]] = (byte)wizardFightLevels[lnI]; // (byte)(r1.Next() % 35 + 1);
					romData[0x2a93 + pilgrimFight2[lnI]] = romData[0x2a15 + pilgrimFight2[lnI]];
					romData[0x2a93 + wizardFight2[lnI]] = romData[0x2a54 + wizardFight2[lnI]];
					if (lnI < 16)
						romData[0x22e7 + lnI] = (byte)heroFight2[lnI];
					romData[0x22e7 + 32 + lnI] = (byte)pilgrimFight2[lnI];
					romData[0x22e7 + 64 + lnI] = (byte)wizardFight2[lnI];
				}
				romData[0x29d6 + romData[0x22e7]] = 2;

				// Must "complete the sentence" or really bad things happen...
				romData[0x29d6 + 62] = 0xff;
				romData[0x29d6 + 125] = 0xff;
				romData[0x29d6 + 188] = 0xff;
				romData[0x29d6 + 251] = 0xff;
			}

			if (chkRandSpellStrength.Checked)
			{
				// Totally randomize spell strengths - first, attack spells
				for (int lnI = 0; lnI < 17; lnI++)
				{
					int byteToUse = 0x134b1 + (lnI * 2);
					romData[byteToUse] = (byte)((r1.Next() % 200) + 2);
					if (lnI == 0x0d || lnI == 0x0e || lnI == 0x0f)
						romData[byteToUse + 1] = (byte)(r1.Next() % romData[byteToUse]);
					else
						romData[byteToUse + 1] = (byte)(r1.Next() % (romData[byteToUse] / 2));
				}

				// And then healing spells
				for (int lnI = 0; lnI < 6; lnI++)
				{
					if (lnI == 2 || lnI == 5) continue; // Healall/Healusall
					int byteToUse = 0x134f9 + (lnI * 2);
					romData[byteToUse] = (byte)((r1.Next() % 200) + 2);
					romData[byteToUse + 1] = (byte)(r1.Next() % (romData[byteToUse] / 2));
				}
			}

			if (chkRandTreasures.Checked)
			{
				// If the yellow orb is at a searchable spot, it won't be found unless you change this byte from 0x79 to 0x80+.  SUPER WEIRD!
				romData[0x31828] = 0xff;

				bool legal = false;

				// Totally randomize treasures... but make sure key items exist before they are needed!
				// Keep the Rainbow drop where it is
				int[] treasureAddrZ0 = { 0x29237, 0x29238, 0x29239, // Promontory Cave
                0x2927b, 0x292C4, 0x292C5, 0x292c6 }; // Najimi Tower - Thief's Key, Magic Ball - 7
				int[] treasureAddrZ1 = { 0x2927c, 0x2927d }; // Najimi Tower behind Thief's Key - Magic Ball - 2
				int[] treasureAddrZ2 = { 0x2927e, 0x2927f, // Enticement cave
                0x29234, 0x29235, // Kanave
                0x2923a, 0x2923b, 0x29280, 0x29281, 0x29282, 0x29283, 0x29284, 0x29285, 0x29286, 0x29287, // Dream cave/Wake Up Powder
                0x29252, 0x292d2, 0x292e6, // champange tower
                0x2925c, // isis meteorite band
                0x29249, 0x2924a, 0x2924b, 0x2924c, 0x2924d, 0x2924e, 0x2924f, 0x292b4, 0x292b5, 0x292b6 }; // Pyramid -> Magic key - 28
				int[] treasureAddrZ3 = { 0x292c3, 0x317f4, // Pyramid continued
                0x29255, 0x29256, 0x29257, 0x29258, 0x29249, 0x2924a, // Aliahan continued
                0x31b9c, 0x2925d, 0x2925e, 0x2925f, 0x29260, 0x29261, 0x29262, 0x29263, 0x29264, // Isis continued
                0x29269, 0x2926a, 0x2926b }; // Portuga -> Royal Scroll - 20
				int[] treasureAddrZ4 = { 0x2923c, 0x2923d, // Dwarf's Cave
                0x29251, 0x292c7, 0x292c8, 0x292c9, 0x292ca, // Garuna Tower
                0x29242, 0x29240, 0x2923f, 0x2923e, 0x29241, 0x29243, 0x2928b, 0x2928c, 0x2928e, 0x2928d }; // Kidnapper's Cave -> Black Pepper - 17
				int[] treasureAddrZ5 = { 0x31b94, 0x29270, // Tedan (except Green Orb)
                0x292e4, 0x292e7, // Jipang
                0x29272, 0x29271, 0x29273, // Pirate Cove
                0x292d1, 0x292d0, 0x292cf, 0x292cd, 0x292ce, 0x292cc, 0x292cb }; // Arp Tower - Final Key - 14
				int[] treasureAddrZ6 = { 0x29299, 0x2929c, 0x2929b, 0x2929d, 0x2929a, 0x29298, 0x29293, 0x29294, 0x29295, 0x29291, 0x29292, // Samanao Cave
                0x29296, 0x29297, 0x292a3, 0x292a4, 0x292a2, 0x2929f, 0x2929e, 0x292a0, 0x292a5, 0x292a6, 0x292a1, 0x292a7, 0x29296, // Samanao Cave
                0x29246, 0x29248, 0x29247, 0x29245, 0x29244, 0x29290, 0x2928f }; // Lancel Cave - Mirror Of Ra - 31
				int[] treasureAddrZ7 = { 0x292e5 }; // Staff Of Change - Samanao Castle - 1
				int[] treasureAddrZ8 = { 0x29277, 0x29276, 0x29275, 0x29278, 0x29279, 0x2927a }; // Sword Of Gaia - Ghost ship - 6
				int[] treasureAddrZ10 = { 0x29288, 0x29289, 0x2928a }; // All orbs - Cave Of Necrogund - 3
				int[] treasureAddrZ11 = { 0x29267, 0x29266, 0x29265, 0x29268, // Tantegel Castle
                0x292a8, 0x292ab, 0x292ac, 0x292aa, 0x292a9, // Erdrick's Cave
                0x29274, // Garin's home
                0x292df, 0x292e3, 0x292e1, 0x292e2, 0x292e0, // Rocky Mountain Cave
                0x31b90, // Hauksness
                0x31b88, // Kol
                0x29253, 0x29254, 0x292d7, 0x292d5, 0x292d6, 0x292d8, 0x292da, 0x292d9, 0x292db, 0x292dc, 0x292de, 0x292dd, // Kol Tower
                0x29233 }; // Rimuldar - Staff Of Rain, Stones Of Sunlight, Sacred Amulet - 30
				int[] treasureAddrZ12 = { 0x292ad, 0x292ae, 0x292af, 0x292b0, 0x292b1, 0x292b2, 0x292b3 }; // Zoma's Castle - Sphere of Light - 7
				int[] treasureAddrZ13 = { 0x2922a, 0x29229, 0x29228, // Baramos's Castle
                0x292b7, 0x292b8, 0x292b9, 0x292ba, 0x292bb, 0x292bc, 0x292bd, 0x292be, 0x292bf, 0x292c0, 0x292c1, 0x292c2, // Pyramid Mummy Men Chests
                0x2925b, // Eginbear
                0x31b9f, // World Tree
                0x31b97, // Luzami
                0x31b8c, // Soo
                0x2922b, // Final Key Shrine
                0x2926c, 0x2926d, 0x31b80, // New Town  0x378A9
                0x37DF1, 0x375aa, 0x37786, 0x37cb9, 0x377D5, 0x37828, 0x377fe, 0x37907, 0x37929, 0x37a25, 0x37d9d }; // NPCs - Dead zone - 32 , 0x37d5a

				// NOTICE:  Using 0x3b785, supposedly the wake-up powder NPC, warps you to weird places after jumping off the rope in the tower of Garuna...

				List<int> allTreasureList = new List<int>();

				allTreasureList = addTreasure(allTreasureList, treasureAddrZ0);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ1);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ2);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ3);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ4);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ5);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ6);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ7);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ8);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ10);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ11);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ12);
				allTreasureList = addTreasure(allTreasureList, treasureAddrZ13);

				int[] allTreasure = allTreasureList.ToArray();

				// randomize starting gold
				romData[0x2914f] = (byte)(r1.Next() % 256);

				List<byte> treasureList = new List<byte>();
				byte[] legalTreasures = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
									  0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
									  0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
									  0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
									  0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x48, 0x49, 0x4b, 0x4c, 0x4e,
									  0x53, 0x55, 0x56, 0x5f,
									  0x60, 0x62, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6c, 0x6d,
									  0x71, 0x73, 0x74,
									  0x88, 0x90, 0x98, 0xa0, 0xa8, 0xb0, 0xb8, 0xc0, 0xc8, 0xd0, 0xd8, 0xe0, 0xe8, 0xf0, 0xf8,
									  0xfd, 0xfe, 0xff, 0xfd, 0xfe, 0xff, 0xfd, 0xfe, 0xff, 0xfd, 0xfe, 0xff, 0xfd, 0xfe, 0xff};
				for (int lnI = 0; lnI < allTreasureList.Count; lnI++)
				{
					legal = false;
					while (!legal)
					{
						byte treasure = (byte)(r1.Next() % legalTreasures.Length); // the last two items we can't get...
						treasure = legalTreasures[treasure];
						// Disallow earning gold for searchable items... this is because 0x80 = 0x00 in this scenario, so anything over 0x80 is useless.  
						// (in fact, 0xfd = 0x7d, the Stick Slime, a null item.)
						if (allTreasure[lnI] > 0x29400 && treasure >= 0x80)
							continue;

						//byte[] keyItems = { 0x59, 0x5a, 0x54, 0x11, 0x78, 0x79, 0x7a, 0x7b, 0x10, 0x75 };
						//byte[] minKeyTreasure = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 134, 134 };
						//byte[] keyTreasure = { 37, 116, 124, 130, 133, 133, 133, 133, 165, 165 };

						// We need to make sure key items doesn't exceed a certain point in the story.

						// Verify that only one location exists for key items
						if (!(treasureList.Contains(treasure) && (treasure == 0x53 || treasure == 0x71)))
						{
							legal = true;
							treasureList.Add(treasure);
							romData[allTreasure[lnI]] = treasure;
						}
					}
				}

				// Verify that key items are available in either a store or a treasure chest in the right zone.
				byte[] keyItems = { 0x58, 0x57, 0x59, 0x5d, 0x4f, 0x5a, 0x51, 0x54,
									0x6b, 0x11, 0x5c, 0x77, 0x78, 0x79, 0x7a, 0x7b,
									0x7c, 0x10, 0x75, 0x72, 0x50, 0x70 };
				byte[] minKeyTreasure = { 0, 0, 0, 0, 0, 0, 0, 0,
										  0, 0, 0, 0, 0, 0, 0, 0,
										  0, 129, 129, 129, 0, 129 };
				byte[] keyTreasure = { 6, 8, 36, 56, 73, 87, 118, 119,
									   119, 125, 125, 128, 128, 128, 128, 128,
									   128, 158, 158, 165, 165, 158 };

				int echoingFluteMarker = 0;
				for (int lnJ = 0; lnJ < keyItems.Length; lnJ++)
				{
					int treasureLocation = allTreasure[minKeyTreasure[lnJ] + (r1.Next() % (keyTreasure[lnJ] - minKeyTreasure[lnJ]))];
					if (keyItems.Contains(romData[treasureLocation]))
					{
						lnJ--;
						continue;
					}
					romData[treasureLocation] = keyItems[lnJ];

					// Echoing Flute business.  01 = Silver, 02 = Red, 04 = Yellow, 08 = Purple, 10 = Blue, 20 = Green
					if (keyItems[lnJ] >= 0x77 && keyItems[lnJ] <= 0x7c)
					{
						byte[] echoLocations;
						byte orbNumber = (byte)(Math.Pow(2, Math.Abs(0x77 - keyItems[lnJ])));

						if (new int[] { 0x29237, 0x29238, 0x29239 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x2d };
						else if (new int[] { 0x2927b, 0x292C4, 0x292C5, 0x292c6, 0x2927c, 0x2927d }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x9f, 0x3c, 0xed, 0xd6, 0xd7, 0xd8 };
						else if (new int[] { 0x2927e, 0x2927f }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x98, 0xa0, 0xa1, 0xa2 };
						else if (new int[] { 0x29234, 0x29235 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x14 };
						else if (new int[] { 0x2923a, 0x2923b, 0x29280, 0x29281, 0x29282, 0x29283, 0x29284, 0x29285, 0x29286, 0x29287 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x2e, 0xa3, 0xa4, 0xa5 };
						else if (new int[] { 0x2925c, 0x31b9c, 0x2925d, 0x2925e, 0x2925f, 0x29260, 0x29261, 0x29262, 0x29263, 0x29264 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x96, 0x50, 0x51, 0x52, 0x87, 0x55, 0x54, 0x53, 0x56, 0x57 };
						else if (new int[] { 0x29255, 0x29256, 0x29257, 0x29258, 0x29249, 0x2924a }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x00, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x47 };
						else if (new int[] { 0x29249, 0x2924a, 0x2924b, 0x2924c, 0x2924d, 0x2924e, 0x2924f, 0x292b4, 0x292b5, 0x292b6, 0x292c3, 0x317f4 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x3b, 0xcf, 0xd0, 0xd1, 0xd2, 0xd3, 0xd4, 0xd5 };
						else if (new int[] { 0x2923c, 0x2923d }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x2f };
						else if (new int[] { 0x29251, 0x292c7, 0x292c8, 0x292c9, 0x292ca }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x3d, 0xdb, 0xdc, 0xdd, 0xde };
						else if (new int[] { 0x29242, 0x29240, 0x2923f, 0x2923e, 0x29241, 0x29243, 0x2928b, 0x2928c, 0x2928e, 0x2928d }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x34, 0xb2 };
						else if (new int[] { 0x31b94, 0x29270 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x15, 0x85, 0x86 };
						else if (new int[] { 0x292e4, 0x292e7 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x24, 0x8a };
						else if (new int[] { 0x29272, 0x29271, 0x29273 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x18, 0x8b, 0x8c };
						else if (new int[] { 0x292d1, 0x292d0, 0x292cf, 0x292cd, 0x292ce, 0x292cc, 0x292cb }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x3e, 0xe4, 0xe5, 0xe6, 0xe7 };
						else if (new int[] { 0x29299, 0x2929c, 0x2929b, 0x2929d, 0x2929a, 0x29298, 0x29293, 0x29294, 0x29295, 0x29291, 0x29292, 0x29296, 0x29297, 0x292a3, 0x292a4, 0x292a2, 0x2929f, 0x2929e, 0x292a0, 0x292a5, 0x292a6, 0x292a1, 0x292a7, 0x29296 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x37, 0xbe, 0xbf };
						else if (new int[] { 0x29246, 0x29248, 0x29247, 0x29245, 0x29244, 0x29290, 0x2928f }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x36, 0xbc, 0xbd };
						else if (new int[] { 0x292e5 }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x06, 0x5d, 0x5e, 0x5f, 0x60, 0x61, 0x62, 0x63, 0x64, 0x99 };
						else if (new int[] { 0x29277, 0x29276, 0x29275, 0x29278, 0x29279, 0x2927a }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x9c, 0x9d };
						else if (new int[] { 0x29288, 0x29289, 0x2928a }.Contains(treasureLocation))
							echoLocations = new byte[] { 0x32, 0xa8, 0xa9, 0xaa, 0x31 };
						else
							echoLocations = new byte[] { };

						for (int i = 0; i < echoLocations.Length; i++)
						{
							romData[0x33c51 + echoingFluteMarker] = orbNumber;
							echoingFluteMarker++;

							romData[0x33c51 + echoingFluteMarker] = echoLocations[i];
							echoingFluteMarker++;
						}
						romData[0x33c51 + echoingFluteMarker] = 0x00;
					}
				}

				// Echoing Flute business...
				byte[] echoingFlute = { 0xA5, 0x2F, 0xD0, 0x0E, 0x00, 0x1E, 0x2F, 0x00, 0x44, 0x17, 0x00, 0x0D, 0x77, 0x20, 0x33, 0xCB,
					0x38, 0x60, 0xA2, 0x00, 0xBD, 0x41, 0xBC, 0xF0, 0xEB, 0xE8, 0xBC, 0x41, 0xBC, 0xE8, 0x2D, 0xCE,
					0x60, 0xD0, 0xF1, 0xC4, 0x8B, 0xD0, 0xED, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA,
					0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0xEA, 0x4C, 0x59, 0xA2 }; // 0xC9, 0x04, 0xD0, 0x0B, 0xAD, 0xCD, 0x60, 0xC8, 0xD0, 0xE0, 

				for (int i = 0; i < echoingFlute.Length; i++)
				{
					romData[0x32228 + i] = echoingFlute[i];
				}

				// The Golden Claw location has a trigger that needs to be set so it can only be retrieved once instead of an infinite amount of times.
				romData[0x319a0] = romData[0x317f4];
			}

			if (chkRandStores.Checked)
			{
				// Totally randomize stores (19 weapon stores, 24 item stores, 248 items total)  No store can have more than 12 items.
				// I would just create random values for 248 items, then determine weapon and item stores out of that!
				byte[] legalStoreWeapons = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f,
									  0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f,
									  0x20, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29, 0x2a, 0x2b, 0x2c, 0x2d, 0x2e, 0x2f,
									  0x30, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x38, 0x39, 0x3a, 0x3b, 0x3c, 0x3d, 0x3e, 0x3f,
									  0x40, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46
				};
				byte[] legalStoreItems = { 0x48, 0x49, 0x4b, 0x4c, 0x4e,
									  0x53, 0x55, 0x56, 0x5f,
									  0x60, 0x62, 0x64, 0x65, 0x66, 0x67, 0x68, 0x69, 0x6c, 0x6f,
									  0x71, 0x73, 0x74,
									  0x56, 0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74,
									  0x56, 0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74,
									  0x56, 0x65, 0x66, 0x67, 0x68, 0x6c, 0x73, 0x74
				};

				int[] weaponStores = { 0x36838, 0x3683f, 0x36846, 0x3684d, 0x36854, 0x3685b, 0x36862, 0x36869, 0x3686e, 0x36874, 0x3687a, 0x36880, 0x36887, 0x3688d, 0x36893, 0x3689a, 0x368a1, 0x368a7, 0x368ae }; // 42
				int[] itemStores = { 0x368b4, 0x368b7, 0x368be, 0x368c4, 0x368ca, 0x368d0, 0x368d6, 0x368db, 0x368e0, 0x368e2, 0x368e6, 0x368ec, 0x368f2, 0x368f4, 0x368fa, 0x368ff, 0x36905, 0x36908, 0x3690e, 0x36914, 0x3691a, 0x36920, 0x36927, 0x3692b }; // 22

				for (int lnI = 0; lnI < weaponStores.Length; lnI++)
				{
					List<int> store = new List<int> { };
					bool lastItem = false;
					int byteToUse = weaponStores[lnI];
					int lnJ = 0;
					do
					{
						if (romData[byteToUse + lnJ] >= 128)
							lastItem = true;
						romData[byteToUse + lnJ] = legalStoreWeapons[r1.Next() % legalStoreWeapons.Length];
						bool failure = false;
						for (int lnK = 0; lnK < lnJ; lnK++)
							if (romData[byteToUse + lnJ] == romData[byteToUse + lnK])
								failure = true;
						if (lastItem)
							romData[byteToUse + lnJ] += 128;
						if (failure)
						{
							lastItem = false;
							continue;
						}
						lnJ++;
					} while (!lastItem);
				}
				for (int lnI = 0; lnI < itemStores.Length; lnI++)
				{
					List<int> store = new List<int> { };
					bool lastItem = false;
					int byteToUse = itemStores[lnI];
					int lnJ = 0;
					do
					{
						if (romData[byteToUse + lnJ] >= 128)
							lastItem = true;
						romData[byteToUse + lnJ] = legalStoreItems[r1.Next() % legalStoreItems.Length];
						bool failure = false;
						for (int lnK = 0; lnK < lnJ; lnK++)
							if (romData[byteToUse + lnJ] == romData[byteToUse + lnK])
								failure = true;
						if (lastItem)
							romData[byteToUse + lnJ] += 128;
						if (failure)
						{
							lastItem = false;
							continue;
						}
						lnJ++;
					} while (!lastItem);
				}

				//int[] storeItems = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
				//                 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
				//List<int> itemStore = new List<int>();
				//List<int> weaponStore = new List<int>();
				//for (int lnI = 0; lnI < 248; lnI++)
				//    romData[0x36838 + lnI] = (byte)(legalStoreItems[(r1.Next() % legalStoreItems.Length)]);

				//int[] weaponStoreLocations = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 247 };

				//int lnMarker = -1;
				//// Need to make sure this doesn't exceed 11
				//int average = 248 / weaponStoreLocations.Length;
				//for (int lnI = 0; lnI < weaponStoreLocations.Length - 1; lnI++)
				//{
				//    int storeSize = average;
				//    storeSize += (-3 + (r1.Next() % 7));
				//    if (storeSize > 12 || average >= 10)
				//        storeSize = 12;
				//    lnMarker += storeSize;
				//    weaponStoreLocations[lnI] = lnMarker;
				//    int avgPart1 = 248 - lnMarker + 1;
				//    int avgPart2 = weaponStoreLocations.Length - lnI - 1;
				//    average = ((248 - lnMarker + 1) / (weaponStoreLocations.Length - lnI - 1));
				//}

				//// Now we can plug in the numbers...
				//for (int lnI = 0; lnI < weaponStoreLocations.Length; lnI++)
				//    romData[0x36838 + weaponStoreLocations[lnI]] += 128;

				//// Go through each of the stores and check for duplicates.
				//int startStore = 0;
				//List<int> storeContents = new List<int> { };
				//for (int lnI = 0; lnI < 248; lnI++)
				//{
				//    bool lastItem = (romData[0x36838 + lnI] >= 128);
				//    int itemToCompare = (romData[0x36838 + lnI] >= 128 ? romData[0x36838 + lnI] - 128 : romData[0x36838 + lnI]);
				//    if (storeContents.Contains(itemToCompare))
				//    {
				//        romData[0x36838 + lnI] = (byte)((lastItem ? 128 : 0) + legalStoreItems[r1.Next() % legalStoreItems.Length]);
				//        lnI = startStore - 1;
				//        storeContents.Clear();
				//        continue;
				//    }
				//    storeContents.Add(itemToCompare);
				//    if (lastItem)
				//    {
				//        storeContents.Clear();
				//        startStore = lnI + 1;
				//    }
				//}

				// Inn prices randomized
				for (int lnI = 0; lnI < 26; lnI++)
				{
					int innPrice = (r1.Next() % 20) + 1;
					romData[0x367c1 + lnI] -= (byte)(romData[0x367c1 + lnI] % 32);
					romData[0x367c1 + lnI] += (byte)innPrice;
				}
			}

			if (chkRandStatGains.Checked)
			{
				//// Randomize starting stats.
				// Give each hero from 22HP (min for Wizard) to about 36 HP.  (Hero)  Just so everybody has a chance!
				romData[0x1eed7] = (byte)((r1.Next() % 13) + 5 + 9);
				// Remove the baseline for HP...
				romData[0x24f4] = 0xea;
				romData[0x24f5] = 0x4c;
				romData[0x24f6] = 0xfa;
				romData[0x24f7] = 0xa4;
				// ... and MP...
				romData[0x2555] = 0xea;
				romData[0x2556] = 0x4c;
				romData[0x2557] = 0x5b;
				romData[0x2558] = 0xa5;
				// ... and the rest!  But we also need to prevent someone gaining 200 points in a stat...
				romData[0x247c] = 0xa9;
				romData[0x247d] = 0x00;
				romData[0x247e] = 0x8d;
				romData[0x247f] = 0x05;
				romData[0x2480] = 0x00;
				romData[0x2481] = 0x4c;
				romData[0x2482] = 0x7d;
				romData[0x2483] = 0xa4;

				// TRY TWO
				// Max array:  [7, 5]
				// ORDER:  Hero, Wizard, Pilgrim, Sage, Soldier, Merchant, Fighter, Goof-off
				int[,] heroL41Gains = new int[,] {
							   { 134, 77, 166, 121, 69 },
							   { 33, 125, 106, 143, 108 },
							   { 55, 79, 110, 120, 107 },
							   { 80, 90, 127, 79, 97 },
							   { 149, 37, 191, 32, 33 },
							   { 96, 75, 122, 54, 60 },
							   { 188, 191, 143, 145, 43 },
							   { 36, 47, 84, 210, 52 }
							   };

				//heroL41Gains[8, 0] = 0;
				// Randomize the four multipliers from 8 to 32.  Each multiplier has six bytes.
				for (int lnI = 0; lnI < 2; lnI++)
					for (int lnJ = 0; lnJ < 5; lnJ++)
					{
						int byteToUse2 = 0x281b + (lnI * 5) + lnJ;
						romData[byteToUse2] = (byte)(((r1.Next() % 4) + 1) * 8);
					}

				// Randomize the levels to the next multiplier from 0 to 24.(First 4 bytes)  Always make the 5th byte "99" (63 hex).
				// Calculate the base gain based on the four multipliers.  Try to get as close to the target gain for each stat as possible.
				// Char byteToUse - 0x4a15b, 0x4a17f, 0x4a1a3, 0x4a1c7, 0x4a1eb, 0x4a20f, 0x4a22d, 0x4a24b
				int byteToUse = 0x290e;
				// 40 bytes for strength, 40 bytes for agility, 40 bytes for vitality, 40 bytes for luck, 40 bytes for intelligence, in that order.  NOT in character order, statistic order!
				for (int lnJ = 0; lnJ < 5; lnJ++)
				{
					for (int lnI = 0; lnI < 8; lnI++)
					{
						if (optMonsterSilly.Checked || optMonsterMedium.Checked)
						{
							int randomDir = (r1.Next() % 3);
							int difference = heroL41Gains[lnI, lnJ] / (optMonsterSilly.Checked ? 4 : 2);
							if (randomDir == 0)
								heroL41Gains[lnI, lnJ] -= (r1.Next() % difference);
							if (randomDir == 1)
								heroL41Gains[lnI, lnJ] += (r1.Next() % difference);
						}
						if (optMonsterHeavy.Checked)
						{
							if (lnJ == 2)
								heroL41Gains[lnI, lnJ] = (r1.Next() % (lnI == 0 || lnI >= 4 ? 140 : 170)) + (lnI == 0 || lnI >= 4 ? 110 : 80);
							else if (lnJ == 0)
								heroL41Gains[lnI, lnJ] = (r1.Next() % (lnI == 0 || lnI >= 4 ? 180 : 220)) + (lnI == 0 || lnI >= 4 ? 70 : 30);
							else
								heroL41Gains[lnI, lnJ] = (r1.Next() % (lnJ == 4 && lnI <= 3 ? 180 : 210) + (lnJ == 4 && lnI <= 3 ? 70 : 40));
						}

						int[] levels = { 0, 0, 0, 0, 99 };
						for (int lnK = 0; lnK < 4; lnK++)
							levels[lnK] = (byte)(r1.Next() % 50);
						Array.Sort(levels);
						//for (int lnK = 0; lnK < 4; lnK++)
						//{
						//    if ((lnK == 0 && baseStat % 2 == 1) || (lnK == 1 && baseStat % 4 >= 2) || (lnK == 2 && baseStat % 8 >= 4) || (lnK == 3 && baseStat % 16 >= 8))
						//        romData[byteToUse + lnK] = (byte)(128 + levels[lnK]);
						//    else
						//        romData[byteToUse + lnK] = (byte)(levels[lnK]);
						//}

						//if (baseStat >= 16)
						//    romData[byteToUse + 4] = 99 + 128;
						//else
						//    romData[byteToUse + 4] = 99;

						// Averages:  8-16 = .6/level, 24-32 = 1.6/level, 40-48 = 2.6/level, 56-64 = 3.6/level, 72-80 = 4.6/level, 88-96 = 5.6/level, 104-112 = 6.6/level
						// Maximize base stat at 12 (5.6/level at 8 multiplier)
						// Now to figure out the multiplier to use (+ 0) and the base multiplier (+ 5)
						double[] diffs = { 0.0, 0.0, 0.0, 0.0 };
						int[] baseMult = { 0, 0, 0, 0 };
						for (int lnK = 0; lnK < 2; lnK++)
						{
							for (baseMult[lnK] = 1; baseMult[lnK] <= 12; baseMult[lnK]++)
							{
								int byteToUse2 = 0x281b + (lnK * 5); // multipliers
								double stat = 0.0;
								int multLevel = 0;

								for (int lnL = 2; lnL <= 40; lnL++)
								{
									int multLevelToUse = (levels[multLevel]);
									if (lnL > multLevelToUse)
										multLevel++;
									stat += Math.Floor((((double)baseMult[lnK] * romData[byteToUse2 + multLevel]) - 8) / 16) + 0.85;
								}
								//baseMult[lnK] = (int)Math.Round(heroL41Gains[lnI, lnJ] / stat);
								diffs[lnK] = Math.Abs(stat - heroL41Gains[lnI, lnJ]);
								if (stat > heroL41Gains[lnI, lnJ]) break;
							}
						}

						double lowDiff = 9999;
						int lowMult = 0;
						int ultiBaseMult = 0;
						for (int lnK = 0; lnK < 2; lnK++)
						{
							if (diffs[lnK] < lowDiff)
							{
								lowDiff = diffs[lnK];
								lowMult = lnK;
								ultiBaseMult = baseMult[lnK];
							}
						}

						romData[byteToUse] = (byte)((lowMult == 0 ? 0 : 128) + levels[0]);
						romData[byteToUse + 1] = (byte)((ultiBaseMult >= 8 ? 128 : 0) + (levels[1] - levels[0]));
						romData[byteToUse + 2] = (byte)((ultiBaseMult % 8 >= 4 ? 128 : 0) + (levels[2] - levels[1]));
						romData[byteToUse + 3] = (byte)((ultiBaseMult % 4 >= 2 ? 128 : 0) + (levels[3] - levels[2]));
						romData[byteToUse + 4] = (byte)((ultiBaseMult % 2 >= 1 ? 128 : 0) + 127);

						//romData[byteToUse] += (byte)(32 * lowMult);
						//romData[byteToUse + 5] = (byte)ultiBaseMult;

						byteToUse += 5;
					}
				}
				//int asdf = 1234;
				//overrideStats();


				//romData[0x2480] = 0xea;

				// TRY ONE

				//// Randomize stat gains.
				//// First, we'll randomize the multipliers.  They will range from 4 to 20, in multiples of 4.

				//for (int lnI = 0; lnI < 10; lnI++)
				//    romData[0x281b + lnI] = (byte)(((r1.Next() % 5) + 1) * 4);

				//// ORDER:  Strength, agility, vitality, luck, intelligence - set max for each class.  Strength, agility, vitality, luck, intelligence
				//int[] statAdjust = { 160, 120, 215, 155, 115, // Hero
				//               60, 185, 135, 180, 135, // Wizard
				//               95, 110, 130, 165, 135, // Pilgrim
				//               125, 130, 120, 125, 130, // Sage
				//               175, 70, 220, 45, 50, // Soldier
				//               125, 115, 145, 105, 85, // Merchant
				//               235, 220, 183, 185, 52, // Fighter
				//               70, 85, 110, 255, 90}; // Goof-off

				//for (int lnI = 0; lnI < 40; lnI++)
				//{
				//    int[] levels = { (r1.Next() % 50), (r1.Next() % 50), (r1.Next() % 50), (r1.Next() % 50) };
				//    for (int lnJ = 0; lnJ < 3; lnJ++)
				//        for (int lnK = lnJ; lnK < 4; lnK++)
				//            if (levels[lnJ] > levels[lnK])
				//            {
				//                int temp = levels[lnJ];
				//                levels[lnJ] = levels[lnK];
				//                levels[lnK] = temp;
				//            }

				//    bool multA = (r1.Next() % 2 == 1);
				//    // Determine maximum base stat for the stats in mind.  Remember... average gain = base * mult / 13.75
				//    double attribute = 0;

				//    for (int lnJ = 0; lnJ < 50; lnJ++)
				//    {
				//        int levelToUse = (multA ? 0 : 5) + (lnJ < levels[0] ? 0 : lnJ < levels[1] ? 1 : lnJ < levels[2] ? 2 : lnJ < levels[3] ? 3 : 4);
				//        attribute += (romData[0x281b + levelToUse] / 13.75);
				//    }
				//    // This final attribute is if base = 1.  Calculate the base on the stats above.  Adjust -50% to +100%
				//    int maxStat = statAdjust[lnI];
				//    int statRandom = (r1.Next() % 3);
				//    if (statRandom == 0)
				//    {
				//        maxStat -= (r1.Next() % (maxStat / 2));
				//    } else if (statRandom == 2)
				//    {
				//        maxStat += (r1.Next() % (maxStat * 2));
				//    }

				//    int newBase = (int)Math.Round(maxStat / attribute);

				//    if (newBase < 1)
				//        newBase = 1;
				//    if (newBase > 15)
				//        newBase = 15;

				//    //if (lnI >= 16 && lnI < 24 && newBase < (int)Math.Ceiling((double)maxBase / 3))
				//    //    newBase = (int)Math.Ceiling((double)maxBase / 3); // Vitality base REALLY needs to be 1/3 max or greater or you'll never survive.
				//    //if (lnI >= 32 && lnI < 36 && newBase < (int)Math.Ceiling((double)maxBase / 3))
				//    //    newBase = (int)Math.Ceiling((double)maxBase / 3); // Intelligence base REALLY needs to be 1/3 max or greater or you'll never get MP.
				//    if (lnI >= 36 && lnI < 40)
				//        newBase = 0; // Give out no intelligence to non-MP users.
				//                     //int charLevel = 0;
				//    for (int lnJ = 0; lnJ < 5; lnJ++)
				//    {
				//        if (lnJ == 0) // Determine Multiplier path A or B with byte 0.
				//            romData[0x290e + (lnI * 5) + lnJ] = (byte)(!multA ? 128 : 0); // (byte)(lnI >= 16 && lnI < 24 ? 128 : 0);
				//        if (lnJ == 1)
				//            romData[0x290e + (lnI * 5) + lnJ] = (byte)(newBase >= 8 ? 128 : 0);
				//        if (lnJ == 2)
				//            romData[0x290e + (lnI * 5) + lnJ] = (byte)(newBase % 8 >= 4 ? 128 : 0);
				//        if (lnJ == 3)
				//            romData[0x290e + (lnI * 5) + lnJ] = (byte)(newBase % 4 >= 2 ? 128 : 0);
				//        if (lnJ == 4)
				//            romData[0x290e + (lnI * 5) + lnJ] = (byte)(newBase % 2 == 1 ? 255 : 127);

				//        if (lnJ <= 3)
				//        {
				//            int prevLevel = (lnJ == 0 ? 0 : levels[lnJ - 1]);
				//            int lvlsToNext = 0;
				//            if (lnJ == 0)
				//                lvlsToNext = levels[lnJ];
				//            else
				//                lvlsToNext = levels[lnJ] - levels[lnJ - 1];

				//            romData[0x290e + (lnI * 5) + lnJ] += (byte)(lvlsToNext);
				//        }
				//    }
				//}
			}
		}

		private int[] inverted_power_curve(int min, int max, int arraySize, double powToUse, Random r1)
		{
			int range = max - min;
			double p_range = Math.Pow(range, 1 / powToUse);
			int[] points = new int[arraySize];
			for (int i = 0; i < arraySize; i++)
			{
				double section = (double)r1.Next() / int.MaxValue;
				points[i] = (int)Math.Round(max - Math.Pow(section * p_range, powToUse));
			}
			Array.Sort(points);
			return points;
		}

		private List<int> addTreasure(List<int> currentList, int[] treasureData)
		{
			for (int lnI = 0; lnI < treasureData.Length; lnI++)
				currentList.Add(treasureData[lnI]);
			return currentList;
		}

		private void shuffle(int[] treasureData, Random r1, bool keyItemAvoidance = false)
		{
			// Do not exceed these zones defined for the key items, or you're going to be stuck!
			int[] keyZoneMax = { 13, 13, 23, 40, 45, 53 }; // Cloak of wind, Mirror Of Ra, Golden Key, Jailor's Key, Moon Fragment, Eye Of Malroth
			List<byte> keyItems = new List<byte> { 0x2b, 0x2e, 0x37, 0x39, 0x26, 0x28 }; // When we reach insane randomness, we'll want to know what the key items are so we place them in the appropriate zones...

			// Shuffle each zone 15 times the length of the array for randomness.
			for (int lnI = 0; lnI < 15 * treasureData.Length; lnI++)
			{
				int swap1 = r1.Next() % treasureData.Length;
				int swap2 = r1.Next() % treasureData.Length;

				// Don't shuffle if key items would be swapped into inaccessible areas.
				if (keyItemAvoidance)
				{
					int position1 = keyItems.IndexOf(romData[treasureData[swap1]]);
					int position2 = keyItems.IndexOf(romData[treasureData[swap2]]);
					if (position1 > -1 && swap2 > keyZoneMax[position1])
						continue;
					if (position2 > -1 && swap1 > keyZoneMax[position2])
						continue;
				}

				swap(treasureData[swap1], treasureData[swap2]);
			}
		}

		private void swap(int firstAddress, int secondAddress)
		{
			byte holdAddress = romData[secondAddress];
			romData[secondAddress] = romData[firstAddress];
			romData[firstAddress] = holdAddress;
		}

		private int[] swapArray(int[] array, int first, int second)
		{
			int holdAddress = array[second];
			array[second] = array[first];
			array[first] = holdAddress;
			return array;
		}

		private void btnCompare_Click(object sender, EventArgs e)
		{
			if (!loadRom(true)) return;
			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW3Compare.txt")))
			{
				for (int lnI = 0; lnI < 0x8a; lnI++)
					compareComposeString("monsters" + lnI.ToString("X2"), writer, (0x32e3 + (23 * lnI)), 23);

				compareComposeString("itemPrice1", writer, 0x11be, 128);
				compareComposeString("itemPrice2", writer, 0x123b, 128);
				compareComposeString("weaponEffects", writer, 0x13280, 40);

				compareComposeString("treasures-Promontory", writer, 0x29237, 3);
				compareComposeString("treasures-NajimiBasement", writer, 0x2927B, 3);
				compareComposeString("treasures-Najimi", writer, 0x292C4, 3);
				compareComposeString("treasures-Thief'sKey", writer, 0x37DF1, 1);
				compareComposeString("treasures-MagicBall", writer, 0x375AA, 1);
				compareComposeString("treasures-Invitation", writer, 0x2927E, 2);
				compareComposeString("treasures-Kanave", writer, 0x29234, 2);
				compareComposeString("treasures-Champange1", writer, 0x29252, 1);
				compareComposeString("treasures-Champange2", writer, 0x292D2, 1);
				compareComposeString("treasures-Champange3", writer, 0x292E6, 1);
				compareComposeString("treasures-Isis", writer, 0x2925C, 9);
				compareComposeString("treasures-IsisWizards", writer, 0x31B9C, 1);
				compareComposeString("treasures-GoldenClaw", writer, 0x317F4, 1);
				compareComposeString("treasures-Pyramid1st", writer, 0x29249, 7);
				compareComposeString("treasures-Pyramid3rd4th5th", writer, 0x292B4, 15);
				compareComposeString("treasures-DreamCave1", writer, 0x2923A, 2);
				compareComposeString("treasures-DreamCave2", writer, 0x29280, 8);
				compareComposeString("treasures-WakeUpNPC", writer, 0x37786, 1);
				compareComposeString("treasures-Aliahan", writer, 0x29255, 5);
				compareComposeString("treasures-Portuga", writer, 0x29269, 3);
				compareComposeString("treasures-RoyalScroll", writer, 0x37CB9, 1);
				compareComposeString("treasures-Dwarf", writer, 0x2923C, 2);
				compareComposeString("treasures-Kidnappers1", writer, 0x2923E, 6);
				compareComposeString("treasures-Kidnappers2", writer, 0x2928B, 4);
				compareComposeString("treasures-BlackPepperNPC", writer, 0x377D5, 1);
				compareComposeString("treasures-Tedan1", writer, 0x31B94, 1);
				compareComposeString("treasures-Tedan2", writer, 0x29270, 1);
				compareComposeString("treasures-TedanGreenOrb", writer, 0x37828, 1);
				compareComposeString("treasures-Garuna1", writer, 0x29251, 1);
				compareComposeString("treasures-Garuna2", writer, 0x292C7, 4);
				compareComposeString("treasures-NohMask", writer, 0x292E4, 1);
				compareComposeString("treasures-PurpleOrb", writer, 0x292E7, 1);
				compareComposeString("treasures-WaterBlaster", writer, 0x377FE, 1);
				compareComposeString("treasures-PirateCove", writer, 0x29271, 3);
				compareComposeString("treasures-Eginbear", writer, 0x2925B, 1);
				compareComposeString("treasures-FinalKey", writer, 0x2922B, 1);
				compareComposeString("treasures-ArpTower", writer, 0x292CB, 7);
				compareComposeString("treasures-Soo", writer, 0x31B8C, 1);
				compareComposeString("treasures-SamanaoCave", writer, 0x29291, 23);
				compareComposeString("treasures-SamanaoCastle", writer, 0x292E5, 1);
				compareComposeString("treasures-LancelCave1", writer, 0x29244, 5);
				compareComposeString("treasures-LancelCave2", writer, 0x2928F, 2);
				compareComposeString("treasures-Luzami", writer, 0x31B97, 1);
				compareComposeString("treasures-NewTown1", writer, 0x2926C, 2);
				compareComposeString("treasures-NewTownYellowOrb", writer, 0x31B80, 1);
				compareComposeString("treasures-Sailor'sThighNPC", writer, 0x378A9, 1);
				compareComposeString("treasures-GhostShip", writer, 0x29275, 6);
				compareComposeString("treasures-SwordOfGaia", writer, 0x31B84, 1);
				compareComposeString("treasures-Negrogund", writer, 0x29288, 3);
				compareComposeString("treasures-SilverOrb", writer, 0x37907, 1);
				compareComposeString("treasures-LeafOfWorld", writer, 0x31B9F, 1);
				compareComposeString("treasures-SphereOfLight", writer, 0x37929, 1);
				compareComposeString("treasures-Baramos", writer, 0x29228, 3);
				compareComposeString("treasures-SwordOfIllusion", writer, 0x37a25, 1);
				compareComposeString("treasures-Tantegel", writer, 0x29265, 4);
				compareComposeString("treasures-Erdrick's", writer, 0x292A8, 5);
				compareComposeString("treasures-SilverHarp", writer, 0x29274, 1);
				compareComposeString("treasures-MountainCave", writer, 0x292DF, 5);
				compareComposeString("treasures-Oricon", writer, 0x31B90, 1);
				compareComposeString("treasures-FairyFlute", writer, 0x31B88, 1);
				compareComposeString("treasures-KolTower1", writer, 0x29253, 2);
				compareComposeString("treasures-KolTower2", writer, 0x292D5, 10);
				compareComposeString("treasures-SacredAmulet", writer, 0x37D5A, 1);
				compareComposeString("treasures-StaffOfRain", writer, 0x37D9D, 1);
				compareComposeString("treasures-RainbowDrop", writer, 0x37D80, 1);
				compareComposeString("treasures-Rimuldar", writer, 0x29233, 1);
				compareComposeString("treasures-ZomaCastle", writer, 0x292AD, 7);

				compareComposeString("stores", writer, 0x36838, 248, 1, "g128");

				for (int lnI = 0; lnI < 95; lnI++)
					compareComposeString("monsterZones" + lnI.ToString("X2"), writer, (0xaeb + (15 * lnI)), 15);
				for (int lnI = 0; lnI < 20; lnI++)
					compareComposeString("monsterSpecial" + lnI.ToString("X2"), writer, (0x107a + (6 * lnI)), 6);
				//for (int lnI = 0; lnI < 13; lnI++)
				//    compareComposeString("monsterBoss" + lnI.ToString("X2"), writer, (0x10356 + (4 * lnI)), 4);
				//compareComposeString("statStart", writer, 0x13dd1, 12);
				compareComposeString("statMult", writer, 0x281b, 10);
				compareComposeString("statUpsStrength", writer, 0x290e + 0, 40);
				compareComposeString("statUpsAgility", writer, 0x290e + 40, 40);
				compareComposeString("statUpsVitality", writer, 0x290e + 80, 40);
				compareComposeString("statUpsLuck", writer, 0x290e + 120, 40);
				compareComposeString("statUpsIntelligence", writer, 0x290e + 160, 40);

				compareComposeString("spellLearningHero", writer, 0x29d6, 63);
				compareComposeString("spellsLearnedHero", writer, 0x22E7, 32);
				compareComposeString("spellLearningPilgrim", writer, 0x2A15, 63);
				compareComposeString("spellsLearnedPilgrim", writer, 0x2307, 32);
				compareComposeString("spellLearningWizard", writer, 0x2A54, 63);
				compareComposeString("spellsLearnedWizard", writer, 0x2327, 32);
				compareComposeString("spellLearningSage", writer, 0x2A93, 63);
				//for (int lnI = 0; lnI < 28; lnI++)
				//    compareComposeString("spellStats" + (lnI).ToString(), writer, 0x127d5 + (5 * lnI), 5);
				//compareComposeString("spellCmd", writer, 0x13528, 28);
				//compareComposeString("spellFieldHeal", writer, 0x18be0, 16, 8);
				//compareComposeString("spellFieldMedical", writer, 0x19602, 1);

				//compareComposeString("start1", writer, 0x3c79f, 8);
				//compareComposeString("start2", writer, 0x3c79f + 8, 8);
				//compareComposeString("start3", writer, 0x3c79f + 16, 8);
				//compareComposeString("weapons", writer, 0x13efb, 16);
				//compareComposeString("weaponcost (2.3)", writer, 0x1a00e, 32);
				//compareComposeString("armor", writer, 0x13efb + 16, 11);
				//compareComposeString("armorcost (2.4)", writer, 0x1a00e + 32, 22);
				//compareComposeString("shields", writer, 0x13efb + 27, 5);
				//compareComposeString("shieldcost (2.8)", writer, 0x1a00e + 54, 10);
				//compareComposeString("helmets", writer, 0x13efb + 32, 3);
				//compareComposeString("helmetcost (3.0)", writer, 0x1a00e + 64, 6);

			}
			lblIntensityDesc.Text = "Comparison complete!  (DW3Compare.txt)";
		}

		private StreamWriter compareComposeString(string intro, StreamWriter writer, int startAddress, int length, int skip = 1, string delimiter = "")
		{
			if (delimiter == "")
			{
				writer.WriteLine(intro);
				string final = "";
				string final2 = "";
				for (int lnI = 0; lnI < length; lnI += skip)
				{
					final += romData[startAddress + lnI].ToString("X2") + " ";
					if (lnI % 16 == 15)
					{
						writer.WriteLine(final);
						final = "";
					}
				}
				writer.WriteLine(final);
				if (length >= 16) writer.WriteLine();
				for (int lnI = 0; lnI < length; lnI += skip)
				{
					final2 += romData2[startAddress + lnI].ToString("X2") + " ";
					if (lnI % 16 == 15)
					{
						writer.WriteLine(final2);
						final2 = "";
					}
				}
				writer.WriteLine(final2);
				writer.WriteLine();
			}
			else
			{
				writer.WriteLine(intro);

				string final = "";
				for (int lnI = 0; lnI < length; lnI += skip)
				{
					final += romData[startAddress + lnI].ToString("X2") + " ";
					if (delimiter == "g128" && romData[startAddress + lnI] >= 128)
					{
						writer.WriteLine(final);
						final = "";
					}
				}
				writer.WriteLine(final);
				writer.WriteLine("---------------- VS -------------");
				final = "";
				for (int lnI = 0; lnI < length; lnI += skip)
				{
					final += romData2[startAddress + lnI].ToString("X2") + " ";
					if (delimiter == "g128" && romData2[startAddress + lnI] >= 128)
					{
						writer.WriteLine(final);
						final = "";
					}
				}
				writer.WriteLine(final);
				writer.WriteLine("");
			}
			return writer;
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (txtFileName.Text != "")
				using (StreamWriter writer = File.CreateText("lastFile.txt"))
				{
					writer.WriteLine(txtFileName.Text);
					writer.WriteLine(txtFlags.Text);
					writer.WriteLine(txtDefault1.Text);
					writer.WriteLine(txtDefault2.Text);
					writer.WriteLine(txtDefault3.Text);
					writer.WriteLine(cboClass1.SelectedIndex);
					writer.WriteLine(cboClass2.SelectedIndex);
					writer.WriteLine(cboClass3.SelectedIndex);
					writer.WriteLine(cboGender1.SelectedIndex);
					writer.WriteLine(cboGender2.SelectedIndex);
					writer.WriteLine(cboGender3.SelectedIndex);
				}
		}

		private void txtFileName_Leave(object sender, EventArgs e)
		{
			runChecksum();
		}

		private void btnCompareBrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog1 = new OpenFileDialog();

			openFileDialog1.InitialDirectory = "c:\\";
			openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog1.FilterIndex = 2;
			openFileDialog1.RestoreDirectory = true;

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				txtCompare.Text = openFileDialog1.FileName;
			}
		}

		private void textOutput()
		{
			loadRom(false);
			using (StreamWriter writer = File.CreateText(Path.Combine(Path.GetDirectoryName(txtFileName.Text), "DW3TextOutput.txt")))
			{
				for (int lnI = 0; lnI < 96; lnI++)
					outputComposeString("monstersZones" + (lnI).ToString("X2"), writer, (0xaeb + (15 * lnI)), 15);

				for (int lnI = 0; lnI < 19; lnI++)
					outputComposeString("monstersZoneSpecial" + (lnI + 1).ToString("X2"), writer, (0x108b + (6 * lnI)), 6);

				for (int lnI = 0; lnI < 140; lnI++)
					outputComposeString("monsters" + (lnI).ToString("X2"), writer, (0x32e3 + (23 * lnI)), 23);

				for (int lnI = 0; lnI < 21; lnI++)
					outputComposeString("bosses" + (lnI).ToString("X2"), writer, (0x8ee + (2 * lnI)), 2, 1, 43);
			}
			lblIntensityDesc.Text = "Text output complete!  (DW3TextOutput.txt)";
		}

		private StreamWriter outputComposeString(string intro, StreamWriter writer, int startAddress, int length, int skip = 1, int duplicate = 0)
		{
			string final = "";
			for (int lnI = 0; lnI < length; lnI += skip)
			{
				final += romData[startAddress + lnI].ToString("X2") + " ";
			}
			if (duplicate != 0)
			{
				for (int lnI = duplicate; lnI < length + duplicate; lnI += skip)
				{
					final += romData[startAddress + lnI].ToString("X2") + " ";
				}
			}
			writer.WriteLine(intro);
			writer.WriteLine(final);
			writer.WriteLine();
			return writer;
		}

		private void determineChecks(object sender, EventArgs e)
		{
			string flags = txtFlags.Text;
			int number = convertChartoInt(Convert.ToChar(flags.Substring(0, 1)));
			optMonsterLight.Checked = (number % 4 == 0);
			optMonsterSilly.Checked = (number % 4 == 1);
			optMonsterMedium.Checked = (number % 4 == 2);
			optMonsterHeavy.Checked = (number % 4 == 3);
			chkFourJobFiesta.Checked = (number % 8 >= 4);
			chkRemoveParryFight.Checked = (number % 16 >= 8);
			chkNoLamiaOrbs.Checked = (number % 32 >= 16);

			number = convertChartoInt(Convert.ToChar(flags.Substring(1, 1)));
			cboExpGains.SelectedIndex = (number % 8);
			cboEncounterRate.SelectedIndex = (number / 8);

			number = convertChartoInt(Convert.ToChar(flags.Substring(2, 1)));
			cboGoldReq.SelectedIndex = (number % 4);
			chkRandomizeXP.Checked = (number % 8 >= 4);
			chkRandomizeGP.Checked = (number % 16 >= 8);
			chkFasterBattles.Checked = (number % 32 >= 16);
			chkSpeedText.Checked = (number >= 32);

			number = convertChartoInt(Convert.ToChar(flags.Substring(3, 1)));
			chkRandStores.Checked = (number % 2 == 1);
			chkRandEnemyPatterns.Checked = (number % 4 >= 2);
			chkRandSpellLearning.Checked = (number % 8 >= 4);
			chkRandStatGains.Checked = (number % 16 >= 8);
			chkRandTreasures.Checked = (number % 32 >= 16);
			chkRandMonsterZones.Checked = (number >= 32);

			number = convertChartoInt(Convert.ToChar(flags.Substring(4, 1)));
			chkRandEquip.Checked = (number % 2 == 1);
			//chkRandItemEffects.Checked = (number % 4 >= 2);
			chkRandItemEffects.Checked = false;
			chkRandWhoCanEquip.Checked = (number % 8 >= 4);
			chkRandSpellStrength.Checked = (number % 16 >= 8);
			chkRandomizeMap.Checked = (number % 32 >= 16);
			chkSmallMap.Checked = (number >= 32);
		}

		private void determineFlags(object sender, EventArgs e)
		{
			if (loading) return;

			string flags = "";
			flags += convertIntToChar((optMonsterLight.Checked ? 0 : optMonsterSilly.Checked ? 1 : optMonsterMedium.Checked ? 2 : 3) + (chkFourJobFiesta.Checked ? 4 : 0) + (chkRemoveParryFight.Checked ? 8 : 0) + (chkNoLamiaOrbs.Checked ? 16 : 0));
			flags += convertIntToChar(cboExpGains.SelectedIndex + (8 * cboEncounterRate.SelectedIndex));
			flags += convertIntToChar((cboGoldReq.SelectedIndex) + (chkRandomizeXP.Checked ? 4 : 0) + (chkRandomizeGP.Checked ? 8 : 0) + (chkFasterBattles.Checked ? 16 : 0) + (chkSpeedText.Checked ? 32 : 0));
			flags += convertIntToChar((chkRandStores.Checked ? 1 : 0) + (chkRandEnemyPatterns.Checked ? 2 : 0) + (chkRandSpellLearning.Checked ? 4 : 0) + (chkRandStatGains.Checked ? 8 : 0) + (chkRandTreasures.Checked ? 16 : 0) + (chkRandMonsterZones.Checked ? 32 : 0));
			flags += convertIntToChar((chkRandEquip.Checked ? 1 : 0) + (chkRandItemEffects.Checked ? 2 : 0) + (chkRandWhoCanEquip.Checked ? 4 : 0) + (chkRandSpellStrength.Checked ? 8 : 0) + (chkRandomizeMap.Checked ? 16 : 0) + (chkSmallMap.Checked ? 32 : 0));
			txtFlags.Text = flags;
		}

		private string convertIntToChar(int number)
		{
			if (number >= 0 && number <= 9)
				return number.ToString();
			if (number >= 10 && number <= 35)
				return Convert.ToChar(55 + number).ToString();
			if (number >= 36 && number <= 61)
				return Convert.ToChar(61 + number).ToString();
			if (number == 62) return "!";
			if (number == 63) return "@";
			return "";
		}

		private int convertChartoInt(char character)
		{
			if (character >= Convert.ToChar("0") && character <= Convert.ToChar("9"))
				return character - 48;
			if (character >= Convert.ToChar("A") && character <= Convert.ToChar("Z"))
				return character - 55;
			if (character >= Convert.ToChar("a") && character <= Convert.ToChar("z"))
				return character - 61;
			if (character == Convert.ToChar("!")) return 62;
			if (character == Convert.ToChar("@")) return 63;
			return 0;
		}

		private void btnCopyChecksum_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(lblNewChecksum.Text);
		}

		private void txtFileName_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		private void txtFileName_DragDrop(object sender, DragEventArgs e)
		{
			var filePaths = (string[])e.Data.GetData(DataFormats.FileDrop);
			txtFileName.Text = filePaths[0];
		}
	}
}
