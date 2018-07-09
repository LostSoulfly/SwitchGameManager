using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using System.IO;
using SwitchGameManager.Helpers;
using SwitchGameManager.Properties;
using System.Diagnostics;

namespace SwitchGameManager
{
    public partial class formMain : Form
    {

        public List<XciItem> xciCache = new List<XciItem>();
        public List<XciItem> xciList = new List<XciItem>();
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {

            XciHelper.formMain = this;
            FileHelpers.formMain = this;

            SetupObjectListView();

            //Load the cache
            UpdateToolStripLabel("Loading Cache file..");
            xciCache = XciHelper.LoadXciCache("Cache.json");

            //Load the settings
            if (Helpers.Settings.LoadSettings("Config.json") == false)
                manageXciLocToolStripMenuItem_Click(null, null);

            if (Helpers.Settings.config.sdDriveLetter.Length > 0 && !Directory.Exists(Helpers.Settings.config.sdDriveLetter))
                manageXciLocToolStripMenuItem_Click(null, null);

            //Setup the OLV with the saved state (if it was saved)
            if (Helpers.Settings.config.olvState != null)
                olvLocal.RestoreState(Helpers.Settings.config.olvState);

            if (Helpers.Settings.config.formHeight > 0)
                this.Height = Helpers.Settings.config.formHeight;

            if (Helpers.Settings.config.formWidth > 0)
                this.Width = Helpers.Settings.config.formWidth;


            //todo Lazy update. Do this stuff in the background
            this.Show();
            Application.DoEvents();

            PopulateXciList();
            
            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Load/store list status and program settings Save/Load olv state to byte array, save with settings
             * Menu option to refresh library (warn that this will be slow)
             * Add red X or green check for Game cert, either use a Resource img or system image?
             * Rom Renaming
             */

        }

        public void PopulateXciList()
        {
            xciList = new List<XciItem>();

            foreach (string path in Helpers.Settings.config.localXciFolders)
            {
                xciList.AddRange(XciHelper.LoadGamesFromPath(path, recurse: true, isSdCard: false));
            }

            if (Directory.Exists(Helpers.Settings.config.sdDriveLetter))
            {

                List<XciItem> xciOnSd = new List<XciItem>();

                // SD card games are currently only in the root directory (for SX OS)
                xciOnSd = XciHelper.LoadGamesFromPath(Helpers.Settings.config.sdDriveLetter, recurse: false, isSdCard: true);

                xciList = XciHelper.CreateMasterXciList(xciList, xciOnSd);
            }
            olvLocal.SetObjects(xciList);
            UpdateToolStripLabel();
        }

        private void SetupObjectListView()
        {
            //initialize the image lists, big and small
            olvLocal.LargeImageList = new ImageList();
            olvLocal.LargeImageList.ImageSize = new Size(128, 128);
            olvLocal.SmallImageList = new ImageList();
            olvLocal.SmallImageList.ImageSize = new Size(64, 64);

            //setup the getters that determine how the list shows data/icons
            olvColumnGameSize.AspectGetter = delegate (object row)
            {
                XciItem xciInfo = (XciItem)row;
                return XciHelper.ReadableFileSize(xciInfo.gameSize);
            };

            olvColumnGameUsedSize.AspectGetter = delegate (object row)
            {
                XciItem xciInfo = (XciItem)row;
                return XciHelper.ReadableFileSize(xciInfo.gameUsedSize);
            };

            olvColumnXciName.ImageGetter = delegate (object row) {
                XciItem xciInfo = (XciItem)row;
                String key = xciInfo.packageId.ToString();
                if (!this.olvLocal.LargeImageList.Images.ContainsKey(key))
                {
                    if (xciInfo.gameIcon != null)
                    {
                        this.olvLocal.SmallImageList.Images.Add(key, xciInfo.gameIcon);
                        this.olvLocal.LargeImageList.Images.Add(key, xciInfo.gameIcon);
                    }
                }
                return key;
            };
            
            /*
            this.olvColumnisXciTrimmed.Renderer = new MappedImageRenderer(new Object[] {
                "True", Resources.check,
                "False", Resources.error
            });
            */

            //set up the right-click menu for the objectlistview without remaking it..
            ToolStripMenuItem newMenuItem;
            ToolStripMenuItem subMenuItem;
            foreach (ToolStripMenuItem menuItem in gameManagementToolStripMenuItem.DropDownItems)
            {
                if (menuItem.DropDownItems.Count > 0)
                    newMenuItem = new ToolStripMenuItem(menuItem.Text);
                else
                    newMenuItem = new ToolStripMenuItem(menuItem.Text, null, onClick: ToolStripManagement);
                
                if (newMenuItem.Text.Contains("SD"))
                {
                    foreach (ToolStripMenuItem subItem in menuItem.DropDownItems)
                    {
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripFileManagement);
                        subMenuItem.Tag = "SD";
                        newMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                if (newMenuItem.Text.Contains("PC"))
                {
                    foreach (ToolStripMenuItem subItem in menuItem.DropDownItems)
                    {
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripPcManagement);
                        subMenuItem.Tag = "PC";
                        newMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                contextMenuStrip.Items.Add(newMenuItem);
            }

            olvLocal.ContextMenuStrip = contextMenuStrip;
        }

        private void ToolStripPcManagement(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void ToolStripFileManagement(object sender, EventArgs e)
        {
            ToolStripItem clicked = sender as ToolStripItem;
            ToolStripMenuItem toolStripMenu = (ToolStripMenuItem)contextMenuStrip.Items[0];
            int toolIndex = toolStripMenu.DropDownItems.IndexOf(clicked);

            if (toolIndex < 0)
                toolIndex = sdToolStripMenuItem.DropDownItems.IndexOf(clicked);

            if (!IsListIndexUsable())
                return;

            //TODO
            //Use the TAGs to determine whether it's a PC or SD operaation and do so below

            XciItem xci;
            string action = "";

            if (toolIndex == 0) action = "copy";
            if (toolIndex == 1) action = "move";
            if (toolIndex == 2) action = "delete";

            if (olvLocal.SelectedIndices.Count > 1)
            {
                if (MessageBox.Show($"Are you sure you want to {action} {olvLocal.SelectedObjects.Count} games to SD?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                foreach (Object obj in olvLocal.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    switch (toolIndex)
                    {
                        case 0: //copy
                            FileHelpers.TransferXci(xci, copyToSd: true);
                    break;

                        case 1: //move

                            FileHelpers.TransferXci(xci, moveXci: true, copyToSd: true);
                            break;

                        case 2:
                            File.Delete(xci.xciSdFilePath);
                            xciList.Remove(xci);
                            olvLocal.RefreshObject(xciList);
                            break;

                        default:
                            break;
                    }
                }
            }
            else
            {
                xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;

                if (MessageBox.Show($"Are you sure you want to {action} {xci.gameName} to SD?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                switch (toolIndex)
                {
                    case 0: //copy
                        FileHelpers.TransferXci(xci, copyToSd: true);
                        break;

                    case 1: //move

                        FileHelpers.TransferXci(xci, moveXci: true, copyToSd: true);
                        break;

                    case 2:
                        File.Delete(xci.xciSdFilePath);
                        xciList.Remove(xci);
                        olvLocal.RefreshObject(xciList);
                        break;

                    default:
                        break;
                }
            }
            
        }
        
        public void SetupProgressBar(int min, int max, int initial)
        {
            toolStripProgressBar.Minimum = min;
            toolStripProgressBar.Maximum = max;
            toolStripProgressBar.Value = initial;
            toolStripProgressBar.Visible = true;
        }

        public void UpdateProgressBar(int value)
        {
            try
            {
                toolStripProgressBar.Value = value;
            }
            catch { }
        }

        public void UpdateProgressLabel(string text)
        {
            toolStripProgressLabel.Text = text;
        }

        public void HideProgressElements()
        {
            toolStripProgressBar.Visible = false;
            toolStripProgressLabel.Visible = false;
        }

        public void UpdateToolStripLabel(string text = "")
        {
            if (text.Length == 0 && textBoxFilter.Text.Length > 0)
            {
                toolStripStatus.Text = $"Displaying {olvLocal.Items.Count} out of {xciList.Count - 1} Switch games.";
            }
            else if (text.Length == 0)
            {
                toolStripStatus.Text = $"Displaying {olvLocal.Items.Count -1} Switch games.";
            }
            else
            {
                toolStripStatus.Text = text;
            }
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            TextMatchFilter filter = TextMatchFilter.Contains(olvLocal, textBoxFilter.Text);
            
            olvLocal.AdditionalFilter = filter;
            UpdateToolStripLabel();
        }


        private void olvLocal_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            XciItem xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;
            XciHelper.ShowXciExplorer(xci.xciFilePath);
        }

        private void ToolStripDisplayChange(object sender, EventArgs e)
        {
            ToolStripItem clicked = sender as ToolStripItem;
            int displayIndex = displayStyleToolStripMenuItem.DropDownItems.IndexOf(clicked);
            foreach (ToolStripMenuItem item in displayStyleToolStripMenuItem.DropDownItems)
            {
                if (item == clicked)
                    item.Checked = true;
                else
                    item.Checked = false;
            }

            switch (displayIndex)
            {
                case 0:
                    olvLocal.View = View.SmallIcon;
                    break;
                case 1:
                    olvLocal.View = View.LargeIcon;
                    break;
                case 2:
                    olvLocal.View = View.List;
                    break;
                case 3:
                    olvLocal.View = View.Tile;
                    break;
                case 4:
                    olvLocal.View = View.Details;
                    break;
            }
            olvLocal.Invalidate();

        }

        private void ToolStripManagement(object sender, EventArgs e)
        {
            ToolStripItem clicked = sender as ToolStripItem;
            int toolIndex = olvLocal.ContextMenuStrip.Items.IndexOf(clicked);

            if (toolIndex < 0)
                toolIndex = gameManagementToolStripMenuItem.DropDownItems.IndexOf(clicked);

            //Debug.Assert(toolIndex > 0, "toolIndex should be > 0");

            if (!IsListIndexUsable()) return;

            XciItem xci;
            int successful = 0;
            int failure = 0;
            string action = "";

            if (toolIndex == 2) action = "delete";
            if (toolIndex == 3) action = "trim";
            if (toolIndex == 4) action = "rename";
            if (toolIndex == 5) action = "show certs for";

            if (olvLocal.SelectedIndices.Count > 1)
            {
                if (MessageBox.Show($"Are you sure you want to {action} {olvLocal.SelectedObjects.Count} games?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                foreach (Object obj in olvLocal.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    if (ProcessManagementAction(xci, toolIndex))
                        successful++;
                    else
                        failure++;
                    UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {successful}  Failed: {failure}");
                }
            } else
            {
                xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;

                if (MessageBox.Show($"Are you sure you want to {action} {xci.gameName}?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                ProcessManagementAction(xci, toolIndex);
            }

            olvLocal.RefreshObject(xciList);

        }

        private bool ProcessManagementAction(XciItem xci, int toolIndex)
        {
            switch (toolIndex)
            {
                case 2: //Delete files

                    return true;

                case 3: //trim games
                    bool trim;
                    if (xci.gameSize != xci.gameUsedSize)
                    {
                        trim = XciHelper.TrimXci(xci);

                        if (trim)
                            UpdateToolStripLabel($"Successfully trimmed {xci.gameName}!");
                        else
                            UpdateToolStripLabel($"Failed to trim {xci.gameName}!");

                        //re-process the XCI
                        xci = XciHelper.GetXciInfo(xci.xciFilePath);

                        //Could probably just update the old xciCache item
                        XciItem oldXci = XciHelper.GetXciItemByPackageId(xci.packageId, xciList);
                        xciList.Remove(oldXci);

                        xciList.Add(xci);

                        olvLocal.RefreshObject(xciList);
                        return trim;
                    }
                    else
                    {
                        UpdateToolStripLabel($"{xci.gameName} is already trimmed!");
                        return true;
                    }

                case 4: //Rename files
                    return true;

                case 5: //show XCI Cert
                    XciHelper.ShowXciCert(xci);
                    return true;

                default:
                    return false;
            }
        }
        
        private bool IsListIndexUsable()
        {

            if (olvLocal.SelectedIndices.Count >= 1)
                return true;

            if (olvLocal.SelectedIndex < 0)
            {
                MessageBox.Show("You need to select a thing before doing that!");
                return false;
            }

            return true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SaveSettings()
        {
            //save the OLV state to olvState byte array (column positions, etc)
            Helpers.Settings.config.olvState = olvLocal.SaveState();

            Helpers.Settings.SaveSettings("Config.json");
        }

        private void ChangeIconSize(object sender, EventArgs e)
        {
            ToolStripItem clicked = sender as ToolStripItem;
            int displayIndex = iconSizeToolStripMenuItem.DropDownItems.IndexOf(clicked);
            foreach (ToolStripMenuItem item in iconSizeToolStripMenuItem.DropDownItems)
            {
                if (item == clicked)
                    item.Checked = true;
                else
                    item.Checked = false;
            }

            Size largeSize;
            Size smallSize;
            switch (displayIndex)
            {
                case 0: //biggest
                    largeSize = new Size(256, 256);
                    smallSize = new Size(128, 128);
                    olvLocal.TileSize = new Size(450, 256);
                    break;
                case 1:
                    largeSize = new Size(128, 128);
                    smallSize = new Size(64, 64);
                    olvLocal.TileSize = new Size(300, 128);
                    break;
                case 2:
                    largeSize = new Size(64, 64);
                    smallSize = new Size(32, 32);
                    olvLocal.TileSize = new Size(250, 64);
                    break;
                case 3:
                    largeSize = new Size(32, 32);
                    smallSize = new Size(16, 16);

                    olvLocal.TileSize = new Size(150, 32);
                    break;

                default:
                    largeSize = new Size(128, 128);
                    smallSize = new Size(64, 64);

                    olvLocal.TileSize = new Size(100, 16);
                    break;
            }
            
            olvLocal.LargeImageList.ImageSize = largeSize;
            olvLocal.SmallImageList.ImageSize = smallSize;

            olvLocal.ClearObjects();
            olvLocal.SetObjects(xciList);

        }

        private void manageXciLocToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formFolderList form = new formFolderList();
            DialogResult result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                Helpers.Settings.config.localXciFolders = form.localFolders;
                Helpers.Settings.config.sdDriveLetter = form.sdDriveLetter;
                PopulateXciList();
            }
        }

        private void olvLocal_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip.Show(e.X, e.Y);
            }
        }

        private void formMain_Resize(object sender, EventArgs e)
        {
            Helpers.Settings.config.formHeight = this.Height;
            Helpers.Settings.config.formWidth = this.Width;
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        
        private void CancelFileTransfers(object sender, EventArgs e)
        {
            FileHelpers.StopTransfers();
        }
    }
}
