using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchGameManager
{
    public partial class formFolderList : Form
    {

        public List<string> localFolders = new List<string>();
        public string sdDriveLetter;

        public formFolderList()
        {
            InitializeComponent();
        }

        private void formFolderList_Load(object sender, EventArgs e)
        {
            foreach (var item in Settings.config.localXciFolders)
            {
                listBoxFolders.Items.Add(item);
            }

            foreach (var item in getAvailableDriveLetters())
            {
                comboBoxDriveLetters.Items.Add(item);
            }
            
            if (Settings.config.sdDriveLetter.Length == 0 || !Directory.Exists(Settings.config.sdDriveLetter))
            {
                buttonLocateSwitchSd_Click(null, null);
                SetComboboxToDriveLetter(Settings.config.sdDriveLetter);
            }
            else
            {
                if (Settings.config.sdDriveLetter.Length > 0)
                {
                    SetComboboxToDriveLetter(Settings.config.sdDriveLetter);
                }
            }
        }

        public List<string> getAvailableDriveLetters()
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();
            List<string> drives = new List<string>();

            foreach (DriveInfo item in driveInfo)
            {
                if (item.VolumeLabel.Length > 0)
                    drives.Add($"{item.RootDirectory.ToString()} {item.VolumeLabel} [{item.DriveType}]");
                else
                    drives.Add($"{item.RootDirectory.ToString()} ({item.DriveFormat}) [{item.DriveType}]");
            }

            return drives;
        }

        private void SetComboboxToDriveLetter(string drive)
        {
            if (drive.Length == 0)
                return;

            for (int i = 0; i < comboBoxDriveLetters.Items.Count; i++)
            {
                if (comboBoxDriveLetters.Items[i].ToString().Substring(0, 1).ToUpper() == drive.ToUpper().Substring(0, 1))
                    comboBoxDriveLetters.SelectedIndex = i;
            }
        }

        private void comboBoxDriveLetters_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveInfo driveInfo = new DriveInfo(comboBoxDriveLetters.Text.Substring(0, 1));
            List<string> info = new List<string>();
            info.Add("VolumeLabel:\t" + driveInfo.VolumeLabel);
            info.Add("DriveType:\t" + driveInfo.DriveType);
            info.Add("DriveFormat:\t" + driveInfo.DriveFormat);
            info.Add("IsReady:\t\t" + driveInfo.IsReady);
            info.Add("TotalSize:\t\t" + XciHelper.ReadableFileSize(driveInfo.TotalSize));
            info.Add("AvailFreeSpace:\t" + XciHelper.ReadableFileSize(driveInfo.AvailableFreeSpace));
            info.Add("TotalFreeSpace:\t" + XciHelper.ReadableFileSize(driveInfo.TotalFreeSpace));

            sdDriveLetter = driveInfo.RootDirectory.ToString();

            textBoxDriveInfo.Clear();

            foreach (var item in info)
            {
                textBoxDriveInfo.Text += item + Environment.NewLine;
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            foreach (string folder in listBoxFolders.Items)
            {
                localFolders.Add(folder);
            }
            
            this.Visible = false;
        }


        private void buttonAddFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.Description = "Please select a folder containing Switch XCI games..";
            DialogResult result = folderBrowser.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!Directory.Exists(folderBrowser.SelectedPath))
                    MessageBox.Show("The folder you've selected cannot be found.");

                listBoxFolders.Items.Add(folderBrowser.SelectedPath);
            }
        }

        private void buttonRemoveFolder_Click(object sender, EventArgs e)
        {
            if (listBoxFolders.SelectedIndex > -1)
                listBoxFolders.Items.RemoveAt(listBoxFolders.SelectedIndex);
        }

        private void buttonLocateSwitchSd_Click(object sender, EventArgs e)
        {
            DriveInfo[] driveInfo = DriveInfo.GetDrives();

            foreach (DriveInfo item in driveInfo)
            {
                if (item.DriveType == DriveType.Removable && (item.DriveFormat.ToLower() == "exfat" || item.DriveFormat.ToLower() == "fat32"))
                {
                    if (Directory.GetFiles(item.RootDirectory.ToString(), "*.xci").ToList<string>().Count > 0)
                    {
                        SetComboboxToDriveLetter(item.RootDirectory.ToString());
                        break;
                    }
                }
            }

        }
    }
}
