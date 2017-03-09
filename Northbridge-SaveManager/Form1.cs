﻿using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NorthbridgeSubSystem.Properties;

namespace NorthbridgeSubSystem
{
    public partial class Form1 : Form
    {
         //All variables are saved to the CommonVariables.cs file. If you come from version R1.04 (1.4.0.XXXX) and older, make sure to check it.

        public Form1()
        {
            InitializeComponent();
        }

        private void RestoreBackupButton_Click(object sender, EventArgs e)
        {
            //Code used to restore an older backup.
            RestoreCompleteLabel.Visible = false;
            RestoreFailedLabel.Visible = false;
            var rli = RestoreFolderPicker.ShowDialog();
            if (rli != DialogResult.OK) return;
            try
            {
                Directory.Delete(CommonVariables.SaveLocation, true);
                Directory.CreateDirectory(CommonVariables.SaveLocation);
                DirectoryCopy(RestoreFolderPicker.SelectedPath, CommonVariables.SaveLocation, true);
                RestoreCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                RestoreFailedLabel.Visible = true;
            }
        }

        private void DeleteBackupCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DeleteBackupButton.Enabled = DeleteBackupCheckBox.Checked;
        }

        private void DeleteBackupButton_Click(object sender, EventArgs e)
        {
            DeleteBackupsFailed.Visible = false;
            BackupDeleteCompleteLabel.Visible = false;
            try
            {
                var backupFolder = Settings.Default.AutoBackupLocation;
                Directory.Delete(backupFolder, true);
                Directory.CreateDirectory(backupFolder);
                BackupDeleteCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                DeleteBackupsFailed.Visible = true;
            }
        }

        private void TestBackupButton_Click(object sender, EventArgs e)
        {
            BackupCompleteLabel.Visible = false;
            BackupFailedLabel.Visible = false;
            try
            {
                var backupFolder = Settings.Default.AutoBackupLocation + "\\" + "Backup" +
                                   DateTime.Now.ToString("yyyyMMddHHmm") + "\\";
                DirectoryCopy(CommonVariables.SaveLocation, backupFolder, true);
                BackupCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                BackupFailedLabel.Visible = true;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            var dl = BackupFolderPicker.ShowDialog();
            if (dl != DialogResult.OK) return;
            Settings.Default.AutoBackupLocation = BackupFolderPicker.SelectedPath;
            LocationBackup.Text = Settings.Default.AutoBackupLocation;
            Settings.Default.Save();
        }

        private void LocationBackup_TextChanged(object sender, EventArgs e)
        {
            RestoreFolderPicker.SelectedPath = Settings.Default.AutoBackupLocation;
            Settings.Default.AutoBackupLocation = LocationBackup.Text;
            Settings.Default.Save();
        }

        private void AutoBackupCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.AutoBackupEnabled = AutoBackupCheckbox.Enabled;
            BrowseButton.Enabled = AutoBackupCheckbox.Enabled;
            LocationBackup.Enabled = AutoBackupCheckbox.Enabled;
            RestoreBackupButton.Enabled = AutoBackupCheckbox.Enabled;
            DeleteBackupCheckBox.Enabled = AutoBackupCheckbox.Checked;
            TestBackupButton.Enabled = AutoBackupCheckbox.Checked;
            LocationBackup.Text = Settings.Default.AutoBackupLocation;
            RestoreBackupButton.Enabled = AutoBackupCheckbox.Checked;
            Settings.Default.Save();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteFailedLabel.Visible = false;
            DeleteCompleteLabel.Visible = false;
            try
            {
                Directory.Delete(CommonVariables.SaveLocation, true);
                Directory.CreateDirectory(CommonVariables.SaveLocation);
                ExportButton.Enabled = false;
                DeleteCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                DeleteFailedLabel.Visible = true;
            }
        }

        private void DeleteAllSavesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            DeleteButton.Enabled = DeleteAllSavesCheckBox.Checked;
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExportCompleteLabel.Visible = false;
            ExportFailedLabel.Visible = false;
            var eli = ExportFolderDialog.ShowDialog();
            if (eli != DialogResult.OK) return;
            try
            {
                DirectoryCopy(CommonVariables.SaveLocation, ExportFolderDialog.SelectedPath, true);
                ExportCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                ExportFailedLabel.Visible = true;
            }
        }

        private void SlotSelectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown1.Enabled = SlotSelectCheckBox.Checked;
        }

        private void ImportButton_Click(object sender, EventArgs e)
        {
            ImportCompleteLabel.Visible = false;
            ImportFailedLabel.Visible = false;
            var il = ImportFilePicker.ShowDialog();
            if (il != DialogResult.OK) return;
            try
            {
                if (SlotSelectCheckBox.Checked)
                {
                    var count = numericUpDown1.Value;
                    var i = 0;
                    foreach (var fileName in ImportFilePicker.FileNames)
                    {
                        if (count >= 10 || count + i >= 10)
                            File.Copy(fileName, CommonVariables.SaveLocation + @"\" + "GameSave" + (count + i) + ".isgsf", true);

                        else
                        {
                            File.Copy(fileName, CommonVariables.SaveLocation + @"\" + "GameSave0" + (count + i) + ".isgsf", true);
                        }
                        i++;
                    }
                }
                else
                    foreach (var fileName in ImportFilePicker.FileNames)
                    {
                        File.Copy(fileName, CommonVariables.SaveLocation + @"\" + Path.GetFileName(fileName), true);
                    }
                if (Directory.GetFiles(CommonVariables.SaveLocation, "*.isgsf").Length > 0)
                {
                    ExportButton.Enabled = true;
                    SelectiveExportButton.Enabled = true;
                    AutoBackupCheckbox.Enabled = true;
                    LocationBackup.Enabled = AutoBackupCheckbox.Checked;
                    DeleteAllSavesCheckBox.Enabled = true;
                    DeleteButton.Enabled = DeleteAllSavesCheckBox.Checked;
                    TestBackupButton.Enabled = AutoBackupCheckbox.Checked;
                    RestoreBackupButton.Enabled = AutoBackupCheckbox.Checked;
                    DeleteBackupCheckBox.Enabled = AutoBackupCheckbox.Checked;
                    DeleteBackupButton.Enabled = DeleteBackupCheckBox.Checked;
                }
                else
                {
                    ExportButton.Enabled = false;
                    SelectiveExportButton.Enabled = false;
                    AutoBackupCheckbox.Enabled = false;
                    LocationBackup.Enabled = false;
                    DeleteAllSavesCheckBox.Enabled = false;
                    DeleteButton.Enabled = false;
                    TestBackupButton.Enabled = false;
                    RestoreBackupButton.Enabled = false;
                    DeleteBackupCheckBox.Enabled = false;
                    DeleteBackupButton.Enabled = false;
                }
                ImportCompleteLabel.Visible = true;
            }
            catch (Exception)
            {
                ImportFailedLabel.Visible = true;
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static void DirectoryCopy(
            string sourceDirName, string destDirName, bool copySubDirs)
        {
            var dir = new DirectoryInfo(sourceDirName);
            var dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            var files = dir.GetFiles();

            foreach (var file in files)
            {
                // Create the path to the new copy of the file.
                var temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (!copySubDirs) return;
            {
                foreach (var subdir in dirs)
                {
                    // Create the subdirectory.
                    var temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, true);
                }
            }
        }

        private static void AutoBackupCode()
        {
            var areSavesAvaliable = Directory.GetFiles(CommonVariables.SaveLocation, CommonVariables.SaveFilename + CommonVariables.SaveFileExtension).Length > 0;
            var i = 1;
            if (!areSavesAvaliable) return;
            if (!Directory.Exists(CommonVariables.BackupFolderSetup))
                DirectoryCopy(CommonVariables.SaveLocation, CommonVariables.BackupFolderSetup, true);
            else
            {
                //This variable sets a new location to create the backup, should a folder with the same name exists.
                //Adjust the format to your liking.
                var newBackupFolder = CommonVariables.BackupFolderSetup + "-" + i + "\\";
                while (Directory.Exists(CommonVariables.BackupFolderSetup +"-"+ i + "\\"))
                {
                    i++;
                }
                DirectoryCopy(CommonVariables.SaveLocation, newBackupFolder, true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialises an array were all arguments passed to this executable are saved.
            var args = Environment.GetCommandLineArgs();
            //Checks if the argument to call the UI is present. If not, launch the game.
            if (args.Contains("-NBSetup")) return;
            //Checks if the files set in lines 12 - 19 are present. Adjust this accordingly.
            if (!File.Exists(CommonVariables.PlayerFile) || !File.Exists(CommonVariables.RgssLib) || !File.Exists(CommonVariables.GameFile))
            {
                MessageBox.Show(@"Missing file(s). Aborting.");
                Close();
            }
            else
            {
                Hide();
                //Calls the setup mode from the BackupEngine.cs file.
                BackupEngine.BackupModeSetup();
                //Any arguments for the game executable (set in this executable's code) are loaded.
                //So, for example you pass the arguments --LoadFile <file> via code, this will happen:
                // Run "<Game Folder>\Game.exe" --LoadFile <file>
                // Set these with _gameInfo.Arguments = "argument1 argument2";
                CommonVariables.GameProcess.StartInfo = CommonVariables.GameInfo;
                //Start the game.
                CommonVariables.GameProcess.Start();
                //Wait for the game to close before closing Northbridge.
                CommonVariables.GameProcess.WaitForExit();
                //Calls the Auto-Backup code.
                if (Settings.Default.AutoBackupEnabled && Settings.Default.AutoBackupLocation != null && CommonVariables.CountChanges > 0 && !Settings.Default.SingleBackupMode) AutoBackupCode();
                Close();
            }
            AutoBackupCheckbox.Checked = Settings.Default.AutoBackupEnabled;
            LocationBackup.Text = Settings.Default.AutoBackupLocation;
            if (!Directory.Exists(CommonVariables.SaveLocation))
            {
                Directory.CreateDirectory(CommonVariables.SaveLocation);
                ExportButton.Enabled = false;
                SelectiveExportButton.Enabled = false;
                AutoBackupCheckbox.Enabled = false;
                LocationBackup.Enabled = false;
                DeleteAllSavesCheckBox.Enabled = false;
                DeleteButton.Enabled = false;
                TestBackupButton.Enabled = false;
                RestoreBackupButton.Enabled = false;
                DeleteBackupCheckBox.Enabled = false;
                DeleteBackupButton.Enabled = false;
            }
            else
            if (Directory.GetFiles(CommonVariables.SaveLocation, CommonVariables.SaveFilename + CommonVariables.SaveFileExtension).Length > 0)
            {
                ExportButton.Enabled = true;
                SelectiveExportButton.Enabled = true;
                AutoBackupCheckbox.Enabled = true;
                LocationBackup.Enabled = AutoBackupCheckbox.Checked;
                DeleteAllSavesCheckBox.Enabled = true;
                DeleteButton.Enabled = DeleteAllSavesCheckBox.Checked;
                TestBackupButton.Enabled = AutoBackupCheckbox.Checked;
                RestoreBackupButton.Enabled = AutoBackupCheckbox.Checked;
                DeleteBackupCheckBox.Enabled = AutoBackupCheckbox.Checked;
                DeleteBackupButton.Enabled = DeleteBackupCheckBox.Checked;
                EnableSnapshotCheckbox.Enabled = AutoBackupCheckbox.Checked;
            }
            else
            {
                ExportButton.Enabled = false;
                SelectiveExportButton.Enabled = false;
                AutoBackupCheckbox.Enabled = false;
                LocationBackup.Enabled = false;
                DeleteAllSavesCheckBox.Enabled = false;
                DeleteButton.Enabled = false;
                TestBackupButton.Enabled = false;
                RestoreBackupButton.Enabled = false;
                DeleteBackupCheckBox.Enabled = false;
                DeleteBackupButton.Enabled = false;
                EnableSnapshotCheckbox.Enabled = false;
            }
        }

        private void SelectiveExportButton_Click(object sender, EventArgs e)
        {
            Form sExport = new ExportTool();
            sExport.Show();
        }
    }
    }
