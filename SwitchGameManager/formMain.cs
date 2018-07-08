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

        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {

            XciHelper.formMain = this;

            SetupObjectListView();
            
            this.Show();
            UpdateToolStripLabel("Loading Cache file..");
            Application.DoEvents();

            //Settings.LoadSettings

            //Load the cache
            xciCache = XciHelper.LoadXciCache("Cache.json");

            xciList = XciHelper.LoadGamesFromPath(@"F:\Games\Emulation\Switch\Games\", recurse: true);

            List<XciItem> xciOnSd = new List<XciItem>();

            // SD card games are currently only in the root directory (for SX OS)
            xciOnSd = XciHelper.LoadGamesFromPath(@"E:\", recurse: false, isSdCard: true);

            xciList = XciHelper.CreateMasterXciList(xciList, xciOnSd);

            olvLocal.SetObjects(xciList);
            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Load/store list status and program settings Save/Load olv state to byte array, save with settings
             * Add settings window/options for selecting game directories (allow multiple directories!)
             * Add SD card searching for /switch/ directory
             * Menu option to refresh library (warn that this will be slow)
             * Add red X or green check for Game cert, either use a Resource img or system image?
             * Rom Renaming
             */

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
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
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
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripSdManagement);
                        newMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                if (newMenuItem.Text.Contains("PC"))
                {
                    foreach (ToolStripMenuItem subItem in menuItem.DropDownItems)
                    {
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripPcManagement);
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

        private void ToolStripSdManagement(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

            Debug.Assert(toolIndex > 0, "toolIndex should be > 0");

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
            //TODO save settings before exiting
            //Save cache before exiting?
            Application.Exit();
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

    }
}
