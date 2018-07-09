using BrightIdeasSoftware;
using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SwitchGameManager
{
    public partial class formMain : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        public List<XciItem> xciList = new List<XciItem>();

        public formMain()
        {
            InitializeComponent();
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            XciHelper.formMain = this;
            FileHelper.formMain = this;

            SetupObjectListView();

            SetupDelegates();

            //todo Lazy update. Do this stuff in the background
            this.Show();
            Application.DoEvents();

            //Load the cache
            UpdateToolStripLabel("Loading Cache file..");
            Settings.xciCache = XciHelper.LoadXciCache();

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

            

            PopulateXciList();

            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Menu option to refresh library (warn that this will be slow)
             * Add red X or green check for Game cert, either use a Resource img or system image?
             * Rom Renaming
             */
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

        private bool ProcessManagementAction(XciItem xci, int toolIndex)
        {
            switch (toolIndex)
            {
                case 2: //Delete files from BOTH
                    try
                    {
                        File.Delete(xci.xciSdFilePath);
                        File.Delete(xci.xciFilePath);
                    }
                    catch { }

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

                        XciItem oldXci = XciHelper.GetXciItemByPackageId(xci.packageId, xciList);
                        oldXci = xci;

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

        public void SaveSettings()
        {
            //save the OLV state to olvState byte array (column positions, etc)
            Helpers.Settings.config.olvState = olvLocal.SaveState();
            Helpers.XciHelper.SaveXciCache();
            Helpers.Settings.SaveSettings();
        }

        private void SetupDelegates()
        {
            textBoxFilter.TextChanged += delegate (object o, EventArgs e)
            {
                TextMatchFilter filter = TextMatchFilter.Contains(olvLocal, textBoxFilter.Text);
                olvLocal.AdditionalFilter = filter;
                UpdateToolStripLabel();
            };

            olvLocal.MouseClick += delegate (object s, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right) contextMenuStrip.Show(e.X, e.Y);
            };

            olvLocal.MouseDoubleClick += delegate (object s, MouseEventArgs e)
            {
                XciItem xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;
                XciHelper.ShowXciExplorer(xci.xciFilePath);
            };

            exitToolStripMenuItem.Click += delegate (object s, EventArgs e) { Application.Exit(); };

            this.FormClosing += delegate (object s, FormClosingEventArgs e) { SaveSettings(); };

            this.ResizeEnd += delegate (object s, EventArgs e)
            {
                Helpers.Settings.config.formHeight = this.Height;
                Helpers.Settings.config.formWidth = this.Width;
            };

            cancelTransfersToolStripMenuItem.Click += delegate (object s, EventArgs e) { FileHelper.StopTransfers(); };
            cancelTransfersToolStripMenuItem1.Click += delegate (object s, EventArgs e) { FileHelper.StopTransfers(); };

        }


        private void SetupObjectListView()
        {

            SendMessage(textBoxFilter.Handle, 0x1501, 1, "Filter Library..");

            //initialize the image lists, big and small
            olvLocal.LargeImageList = new ImageList();
            olvLocal.LargeImageList.ImageSize = new Size(128, 128);
            olvLocal.SmallImageList = new ImageList();
            olvLocal.SmallImageList.ImageSize = new Size(64, 64);

            //setup the getters that determine how the list shows data/icons
            olvColumnGameSize.AspectGetter = delegate (object row)
            {
                XciItem xciInfo = (XciItem)row;
                return xciInfo.gameSize;
            };

            olvColumnGameSize.AspectToStringConverter = delegate (object row)
            {
                double sizeInBytes = (double)row;
                return XciHelper.ReadableFileSize(sizeInBytes);
            };

            olvColumnGameUsedSize.AspectGetter = delegate (object row)
            {
                XciItem xciInfo = (XciItem)row;
                return xciInfo.gameUsedSize;
            };

            olvColumnGameUsedSize.AspectToStringConverter = delegate (object row)
            {
                double sizeInBytes = (double)row;
                return XciHelper.ReadableFileSize(sizeInBytes);
            };

            olvColumnXciName.ImageGetter = delegate (object row)
            {
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
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripFileManagement);
                        subMenuItem.Tag = "PC";
                        newMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }

                contextMenuStrip.Items.Add(newMenuItem);
            }

            olvLocal.ContextMenuStrip = contextMenuStrip;
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

            ProcessDisplayChange(displayIndex);
        }

        private void ToolStripFileManagement(object sender, EventArgs e)
        {
            bool isPcAction = false, isSdAction = false;
            string action = "", destination = "", source = "", message = "";
            XciItem xci;

            ToolStripItem clicked = sender as ToolStripItem;
            ToolStripMenuItem toolStripMenu = (ToolStripMenuItem)contextMenuStrip.Items[0];
            int toolIndex = toolStripMenu.DropDownItems.IndexOf(clicked);

            if (toolIndex < 0)
                toolIndex = sdToolStripMenuItem.DropDownItems.IndexOf(clicked);

            if (toolIndex < 0)
                toolIndex = pcToolStripMenuItem.DropDownItems.IndexOf(clicked);

            if (!IsListIndexUsable())
                return;

            if (toolIndex == 0) action = "copy";
            if (toolIndex == 1) action = "move";
            if (toolIndex == 2) action = "delete";

            if ((string)clicked.Tag == "PC")
            {
                isPcAction = true;
                source = "SD";
                destination = "PC";
            }
            else
            {
                isSdAction = true;
                source = "SD";
                destination = "PC";
            }

            if (olvLocal.SelectedIndices.Count > 1)
            {
                if (toolIndex != 2)
                    message = $"Are you sure you want to {action} {olvLocal.SelectedObjects.Count} games to {destination}?";
                else
                    message = $"Are you sure you want to {action} {olvLocal.SelectedObjects.Count} games from {destination}?";

                if (MessageBox.Show(message, $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                foreach (Object obj in olvLocal.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    ProcessFileManagement(xci, toolIndex, isSdAction, isPcAction);
                }
            }
            else
            {
                xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;

                if (toolIndex != 2)
                    message = $"Are you sure you want to {action} {xci.gameName} to {source}?";
                else
                    message = $"Are you sure you want to {action} {xci.gameName} from {source}?";

                if (MessageBox.Show(message, $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                ProcessFileManagement(xci, toolIndex, isSdAction, isPcAction);
            }
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

            if (toolIndex == 2) action = "completely delete";
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
            }
            else
            {
                xci = (XciItem)olvLocal.GetItem(olvLocal.SelectedIndex).RowObject;

                if (toolIndex < 4)
                {
                    if (MessageBox.Show($"Are you sure you want to {action} {xci.gameName}?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                        return;
                }

                ProcessManagementAction(xci, toolIndex);
            }

            olvLocal.RefreshObject(xciList);
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

            Settings.config.listIconSize = displayIndex;
            ProcessChangeIconSize(displayIndex);
        }

        public void ProcessChangeIconSize(int displayIndex)
        {
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

            olvLocal.ClearObjects();

            olvLocal.LargeImageList = new ImageList();
            olvLocal.SmallImageList = new ImageList();

            olvLocal.LargeImageList.ImageSize = largeSize;
            olvLocal.SmallImageList.ImageSize = smallSize;

            olvLocal.SetObjects(xciList);
        }

        public void ProcessDisplayChange(int displayIndex)
        {
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

        public bool ProcessFileManagement(XciItem xci, int toolIndex, bool isSdAction, bool isPcAction)
        {
            switch (toolIndex)
            {
                case 0: //copy
                    if (isSdAction)
                        FileHelper.TransferXci(xci, copyToSd: true);
                    if (isPcAction)

                        FileHelper.TransferXci(xci, copyToPc: true);
                    break;

                case 1: //move
                    if (isSdAction)
                        FileHelper.TransferXci(xci, moveXci: true, copyToSd: true);
                    if (isPcAction)
                        FileHelper.TransferXci(xci, moveXci: true, copyToPc: true);
                    break;

                case 2: //delete
                    if (isSdAction)
                        File.Delete(xci.xciSdFilePath);
                    if (isPcAction)
                        File.Delete(xci.xciFilePath);

                    PopulateXciList();
                    break;

                default:
                    break;
            }

            return true;
        }

        public void SetupProgressBar(int min, int max, int initial)
        {

            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(delegate { SetupProgressBar(min, max, initial); }));
            }
            else
            {
                toolStripProgressBar.Minimum = min;
                toolStripProgressBar.Maximum = max;
                toolStripProgressBar.Value = initial;
                toolStripProgressBar.Visible = true;
            }
        }

        public void UpdateProgressBar(int value)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(delegate { UpdateProgressBar(value); }));
            }
            else
            {
                try
                {
                    toolStripProgressBar.Value = value;
                }
                catch { }
            }
            
        }

        public void HideProgressElements()
        {
            toolStripProgressBar.Visible = false;
            toolStripProgressLabel.Visible = false;
        }

        public void UpdateProgressLabel(string text)
        {
            toolStripProgressLabel.Visible = true;
            toolStripProgressLabel.Text = text;
        }

        public void UpdateToolStripLabel(string text = "")
        {
            if (text.Length == 0 && textBoxFilter.Text.Length > 0)
            {
                toolStripStatus.Text = $"Displaying {olvLocal.Items.Count} out of {xciList.Count} Switch games.";
            }
            else if (text.Length == 0)
            {
                toolStripStatus.Text = $"Displaying {olvLocal.Items.Count} Switch games.";
            }
            else
            {
                toolStripStatus.Text = text;
            }
        }
    }
}