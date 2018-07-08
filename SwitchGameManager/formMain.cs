using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using BrightIdeasSoftware;
using System.IO;
using SwitchGameManager.Helpers;

namespace SwitchGameManager
{
    public partial class formMain : Form
    {

        public List<XciItem> localXcis = new List<XciItem>();
        public List<XciItem> externalXcis = new List<XciItem>();

        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            //BrightIdeasSoftware.ImageRenderer renderer = new BrightIdeasSoftware.ImageRenderer();
            SetupObjectListView();
            
            this.Show();

            //Settings.LoadSettings

            LoadAndDisplayLocalGames();

            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Load XCIs from directory recursively
             * Load/store list status and program settings Save/Load olv state to byte array, save with settings
             * Add settings window/options for selecting game directories (allow multiple directories!)
             * Add SD card searching for /switch/ directory
             * Menu option to refresh library (warn that this will be slow)
             * Add red X or green check for Game cert, either use a Resource img or system image?
             * Rom Renaming
             */

        }

        private void LoadAndDisplayLocalGames()
        {

            UpdateToolStripLabel("Loading Games..");
            olvLocal.EmptyListMsg = "Loading games..";

            localXcis = XciHelper.LoadXciCache("test.json");

            string path = @"F:\Games\Emulation\Switch\Games\";
            toolStripProgressBar.Maximum = localXcis.Count;
            toolStripProgressBar.Minimum = 0;
            toolStripProgressBar.Visible = true;
            ulong packageId;

            int xciCount = Directory.GetFiles(path, "*.xci").Length;

            if (localXcis.Count < xciCount)
                toolStripProgressBar.Maximum = xciCount;

            XciItem loadedXci;

            foreach (var item in Directory.GetFiles(path, "*.xci"))
            {
                UpdateToolStripLabel("Processing " + item);
                toolStripProgressBar.Value += 1;
                packageId = XciHelper.GetPackageID(item);
                if (XciHelper.GetXciItemFromCache(packageId, localXcis) == null)
                {
                    loadedXci = XciHelper.GetXciInfo(item);
                    localXcis.Add(loadedXci);
                }
            }

            toolStripProgressBar.Visible = false;
            olvLocal.SetObjects(localXcis);
            UpdateToolStripLabel();

            olvLocal.EmptyListMsg = "No Switch games found!";

            XciHelper.SaveXciCache("test.json", localXcis);
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

            //set up the right-click menu for the objectlistview without remaking it..
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem newMenuItem;
            ToolStripMenuItem subMenuItem;
            foreach (ToolStripMenuItem menuItem in gameManagementToolStripMenuItem.DropDownItems)
            {
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
                toolStripStatus.Text = $"Displaying {olvLocal.Items.Count} out of {localXcis.Count - 1} Switch games.";
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
            int displayIndex = olvLocal.ContextMenuStrip.Items.IndexOf(clicked);

            if (!IsListIndexUsable()) return;

            XciItem xci;
            int successful = 0;
            int failure = 0;
            string action = "";

            if (olvLocal.SelectedIndices.Count > 1)
            {
                if (displayIndex == 2) action = "delete";
                if (displayIndex == 3) action = "trim";
                if (displayIndex == 4) action = "rename";
                if (displayIndex == 5) action = "show certs for";

                if (MessageBox.Show($"Are you sure you want to {action.ToLowerInvariant()} {olvLocal.SelectedObjects.Count} games?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                foreach (Object obj in olvLocal.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    if (ProcessManagementAction(xci, displayIndex))
                        successful++;
                    else
                        failure++;
                    UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {successful}  Failed: {failure}");
                }
            } else
            {
                xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;
                ProcessManagementAction(xci, displayIndex);
            }

            olvLocal.RefreshObject(localXcis);

        }

        private bool ProcessManagementAction(XciItem xci, int displayIndex)
        {
            switch (displayIndex)
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
                        XciItem oldXci = XciHelper.GetXciItemFromCache(xci.packageId, localXcis);
                        localXcis.Remove(oldXci);

                        localXcis.Add(xci);

                        olvLocal.RefreshObject(localXcis);
                        return trim;
                    }
                    else
                    {
                        UpdateToolStripLabel($"{xci.gameName} is already trimmed!");
                        return true;
                    }

                case 4: //Rename files
                    return true; ;

                case 5: //show XCI Cert
                    XciHelper.ShowXciCert(xci);
                    return true; ;

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

            //olvLocal.TileSize = new Size(300, olvLocal.LargeImageList.ImageSize.Height);

            olvLocal.LargeImageList.ImageSize = largeSize;
            olvLocal.SmallImageList.ImageSize = smallSize;

            olvLocal.ClearObjects();
            olvLocal.SetObjects(localXcis);

        }

    }
}
