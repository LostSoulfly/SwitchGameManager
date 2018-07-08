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

            if (Settings.config.sdDriveLetter.Length > 0)
            {
                for (int i = 0; i < comboBoxDriveLetters.Items.Count; i++)
                {
                    if (comboBoxDriveLetters.Items[i].ToString().Substring(0, 1).ToUpper() == Settings.config.sdDriveLetter.ToUpper().Substring(0, 1))
                        comboBoxDriveLetters.SelectedIndex = i;
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

        private void comboBoxDriveLetters_SelectedIndexChanged(object sender, EventArgs e)
        {
            DriveInfo driveInfo = new DriveInfo(comboBoxDriveLetters.Text.Substring(0, 1));
            List<string> info = new List<string>();
            info.Add("VolumeLabel:\t" + driveInfo.VolumeLabel);
            info.Add("Name:\t\t" + driveInfo.Name);
            info.Add("DriveType:\t" + driveInfo.DriveType);
            info.Add("DriveFormat:\t" + driveInfo.DriveFormat);
            info.Add("IsReady:\t\t" + driveInfo.IsReady);
            info.Add("TotalSize:\t\t" + XciHelper.ReadableFileSize(driveInfo.TotalSize));
            info.Add("AvailFreeSpace:\t" + XciHelper.ReadableFileSize(driveInfo.AvailableFreeSpace));
            info.Add("TotalFreeSpace:\t" + XciHelper.ReadableFileSize(driveInfo.TotalFreeSpace));

            textBoxDriveInfo.Clear();

            foreach (var item in info)
            {
                textBoxDriveInfo.Text += item + Environment.NewLine;
            }

        }
    }
}
