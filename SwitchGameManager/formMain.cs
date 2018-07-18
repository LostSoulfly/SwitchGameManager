using BrightIdeasSoftware;
using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static SwitchGameManager.Helpers.FileHelper;
using static SwitchGameManager.Helpers.XciHelper;

namespace SwitchGameManager
{
    public partial class formMain : Form
    {
        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

        public formMain()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

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

        private void formMain_Load(object sender, EventArgs e)
        {
            XciHelper.formMain = this;
            FileHelper.formMain = this;
            
            SetupObjectListView();

            SetupDelegates();
            
            //Load the settings
            if (Settings.LoadSettings() == false)
                manageXciLocToolStripMenuItem_Click(null, null);
            
            locationToolStripComboBox.SelectedIndex = 0;

            //Setup the OLV with the saved state (if it was saved)
            if (Settings.config.olvState != null)
            {
                olvList.RestoreState(Settings.config.olvState);
                ProcessChangeIconSize(Settings.config.listIconSize);
            }

            if (Settings.config.formHeight > 0)
                this.Height = Settings.config.formHeight;

            if (Settings.config.formWidth > 0)
                this.Width = Settings.config.formWidth;

            UpdateToolMenus();
            XciHelper.LoadXcisInBackground();

            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Add red X or green check for Game cert, either use a Resource img or system image?
             */
        }

        private bool IsListIndexUsable()
        {
            if (olvList.SelectedIndices.Count >= 1)
                return true;

            if (olvList.SelectedIndex < 0)
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
                Settings.config.localXciFolders = form.localFolders;
                Settings.config.sdDriveLetter = form.sdDriveLetter;
                //XciHelper.LoadXcis();
                UpdateToolMenus();
            }
        }
        
        private void SetupDelegates()
        {
            textBoxFilter.TextChanged += delegate (object o, EventArgs e)
            {
                TextMatchFilter filter = TextMatchFilter.Contains(olvList, textBoxFilter.Text);
                olvList.AdditionalFilter = filter;
                UpdateToolStripLabel();
            };

            olvList.MouseClick += delegate (object s, MouseEventArgs e)
            {
                UpdateToolMenus();
                if (e.Button == MouseButtons.Right) contextMenuStrip.Show(e.X, e.Y);
            };

            gameManagementToolStripMenuItem.Click += delegate (object s, EventArgs e) { UpdateToolMenus(); };

            olvList.MouseDoubleClick += delegate (object s, MouseEventArgs e)
            {
                XciItem xci = (XciItem)olvList.GetItem(olvList.SelectedIndex).RowObject;
                XciHelper.ShowXciExplorer(xci.xciFilePath);
            };

            exitToolStripMenuItem.Click += delegate (object s, EventArgs e) { Application.Exit(); };

            locationToolStripComboBox.SelectedIndexChanged += delegate (object s, EventArgs e) {
                Settings.config.defaultView = (XciLocation)locationToolStripComboBox.SelectedIndex;
                XciHelper.RefreshList();
            };

            this.FormClosing += delegate (object s, FormClosingEventArgs e) { SaveSettings(); };

            this.ResizeEnd += delegate (object s, EventArgs e)
            {
                if (this.WindowState != FormWindowState.Maximized)
                {
                    Settings.config.formHeight = this.Height;
                    Settings.config.formWidth = this.Width;
                }
            };

            cancelTransfersToolStripMenuItem.Click += delegate (object s, EventArgs e) { FileHelper.StopTransfers(); };
            rebuildCachetoolStripMenuItem.Click += delegate (object s, EventArgs e) { XciHelper.RebuildCache(); };
            refreshGamesListToolStripMenuItem.Click += delegate (object s, EventArgs e) { XciHelper.LoadXcisInBackground(); };
        }

        private void UpdateToolMenus()
        {
            //happens on right click to ContextMenu.
            //Update the ContextMenu's text items
        }

        private void SetupObjectListView()
        {
            SendMessage(textBoxFilter.Control.Handle, 0x1501, 1, "Filter Library..");

            //initialize the image lists, big and small
            olvList.LargeImageList = new ImageList();
            olvList.LargeImageList.ImageSize = new Size(128, 128);
            olvList.SmallImageList = new ImageList();
            olvList.SmallImageList.ImageSize = new Size(64, 64);

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
                if (!this.olvList.LargeImageList.Images.ContainsKey(key))
                {
                    if (xciInfo.gameIcon != null)
                    {
                        this.olvList.SmallImageList.Images.Add(key, xciInfo.gameIcon);
                        this.olvList.LargeImageList.Images.Add(key, xciInfo.gameIcon);
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

            ToolStripMenuItem newMenuItem;
            ToolStripMenuItem subMenuItem;
            foreach (ToolStripMenuItem menuItem in gameManagementToolStripMenuItem.DropDownItems)
            {
                if (menuItem.DropDownItems.Count > 0)
                {
                    newMenuItem = new ToolStripMenuItem(menuItem.Text);

                    foreach (ToolStripMenuItem subItem in menuItem.DropDownItems)
                    {
                        subMenuItem = new ToolStripMenuItem(subItem.Text, null, onClick: ToolStripFileManagement);
                        newMenuItem.DropDownItems.Add(subMenuItem);
                    }
                }
                else
                {
                    newMenuItem = new ToolStripMenuItem(menuItem.Text, null, onClick: ToolStripManagement);
                }

                contextMenuStrip.Items.Add(newMenuItem);
            }

            olvList.ContextMenuStrip = contextMenuStrip;

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
            XciItem xci;
            string message = string.Empty, action = string.Empty, source = string.Empty, destination = string.Empty;
            int success = 0, failure = 0;
            FileStruct fileAction = new FileStruct();

            ToolStripItem clicked = sender as ToolStripItem;
            ToolStripMenuItem toolStripMenu = fileToolStripMenuItem;
            int toolIndex = toolStripMenu.DropDownItems.IndexOf(clicked);

            if (toolIndex < 0)
            {
                toolStripMenu = (ToolStripMenuItem)contextMenuStrip.Items[0];
                toolIndex = toolStripMenu.DropDownItems.IndexOf(clicked);
            }

            Debug.Assert(toolIndex >= 0);

            if (!IsListIndexUsable())
                return;

            if (toolIndex == 0) fileAction.action = FileAction.Copy;
            if (toolIndex == 1) fileAction.action = FileAction.Move;
            if (toolIndex == 2) fileAction.action = FileAction.Delete;

            if (Settings.config.defaultView == XciLocation.PC)
            {
                fileAction.destination = XciLocation.SD;
                fileAction.source = XciLocation.PC;
            }
            else
            {
                fileAction.destination = XciLocation.PC;
                fileAction.source = XciLocation.SD;
            }

            action = Enum.GetName(typeof(FileAction), fileAction.action);
            source = Enum.GetName(typeof(XciLocation), fileAction.source);
            destination = Enum.GetName(typeof(XciLocation), fileAction.destination);
            
            if (olvList.SelectedIndices.Count > 1)
            {

                if (fileAction.action == FileAction.Move || fileAction.action == FileAction.Copy)
                    message = $"Are you sure you want to {action} {olvList.SelectedObjects.Count} games to {destination}?";

                if (fileAction.action == FileAction.Delete)
                    message = $"Are you sure you want to {action} {olvList.SelectedObjects.Count} games from {source}?";

                if (MessageBox.Show(message, $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                foreach (Object obj in olvList.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    xci.fileAction = Clone(fileAction);
                    if (ProcessFileManagement(xci))
                        success++;
                    else
                        failure++;
                }

                UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {success}  Failed: {failure}");
            }
            else
            {
                xci = (XciItem)olvList.GetItem(olvList.SelectedIndex).RowObject;

                if (fileAction.action == FileAction.Move || fileAction.action == FileAction.Copy)
                    message = $"Are you sure you want to {action} {xci.gameName} to {destination}?";

                if (fileAction.action == FileAction.Delete)
                    message = $"Are you sure you want to {action} {xci.gameName} from {source}?";

                if (MessageBox.Show(message, $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                xci.fileAction = Clone(fileAction);
                if (ProcessFileManagement(xci))
                    UpdateToolStripLabel($"{action.ToUpperInvariant()} successful for {xci.gameName}");
                else
                    UpdateToolStripLabel($"{action.ToUpperInvariant()} failed for {xci.gameName}");
            }

            //XciHelper.LoadXcisInBackground();

        }

        private void ToolStripManagement(object sender, EventArgs e)
        {
            //use ProcessFileManagement
            //delete all copies
            //trim
            //show cert
            //show xciexplorer
            //show in explorer
            string message = string.Empty, action = string.Empty, source = string.Empty, destination = string.Empty;
            FileStruct fileAction = new FileStruct();

            ToolStripItem clicked = sender as ToolStripItem;
            int toolIndex = olvList.ContextMenuStrip.Items.IndexOf(clicked);

            if (toolIndex < 0)
                toolIndex = gameManagementToolStripMenuItem.DropDownItems.IndexOf(clicked);

            if (toolIndex == 1) fileAction.action = FileAction.Delete;
            if (toolIndex == 2) fileAction.action = FileAction.Trim;
            if (toolIndex == 3) fileAction.action = FileAction.ShowRenameWindow;
            if (toolIndex == 4) fileAction.action = FileAction.ShowCert;
            if (toolIndex == 5) fileAction.action = FileAction.ShowInExplorer;
            
            switch (fileAction.action)
            {
                case FileAction.ShowRenameWindow:
                    formRenamer renamer = new formRenamer();
                    List<XciItem> renameList = new List<XciItem>();

                    foreach (XciItem item in olvList.SelectedObjects)
                        renameList.Add(item);

                    renamer.PopulateList(renameList);
                    renamer.Show();
                    return;

                case FileAction.ShowInExplorer:
                    return;

                case FileAction.ShowCert:
                    return;

                case FileAction.ShowXciInfo:
                    return;

                default:
                    break;
            }

            XciItem xci;
            int success = 0, failure = 0;

            action = Enum.GetName(typeof(FileAction), fileAction.action);

            if (fileAction.action == FileAction.CompletelyDelete) action = "completely delete (from all locations)";

            if (olvList.SelectedIndices.Count > 1)
            {
                if (MessageBox.Show($"Are you sure you want to {action} {olvList.SelectedObjects.Count} games?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                List<XciItem> actionList = new List<XciItem>();

                foreach (object obj in olvList.SelectedObjects)
                {
                    xci = (XciItem)obj;
                    actionList.Add(Clone(xci));
                }

                foreach (XciItem obj in actionList)
                {
                    xci = (XciItem)obj;
                    xci.fileAction = Clone(fileAction);

                    if (ProcessFileManagement(xci))
                        success++;
                    else
                        failure++;

                    UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {success}  Failed: {failure}");

                    if (fileAction.action == FileAction.Trim)
                        XciHelper.UpdateXci(xci);
                }
            }
            else
            {
                xci = Clone((XciItem)olvList.GetItem(olvList.SelectedIndex).RowObject);
                xci.fileAction = fileAction;

                if (MessageBox.Show($"Are you sure you want to {action} {xci.gameName}?", $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;
                
                ProcessFileManagement(xci);

                if (fileAction.action == FileAction.Trim)
                    XciHelper.UpdateXci(xci);
            }

            if (fileAction.action == FileAction.CompletelyDelete)
                XciHelper.RefreshList();

        }

        public void HideProgressElements()
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(delegate { HideProgressElements(); }));
            }
            else
            {
                toolStripProgressBar.Visible = false;
                toolStripProgressLabel.Visible = false;
            }
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
                    olvList.TileSize = new Size(450, 256);
                    break;

                case 1:
                    largeSize = new Size(128, 128);
                    smallSize = new Size(64, 64);
                    olvList.TileSize = new Size(300, 128);
                    break;

                case 2:
                    largeSize = new Size(64, 64);
                    smallSize = new Size(32, 32);
                    olvList.TileSize = new Size(250, 64);
                    break;

                case 3:
                    largeSize = new Size(32, 32);
                    smallSize = new Size(16, 16);

                    olvList.TileSize = new Size(150, 32);
                    break;

                default:
                    largeSize = new Size(128, 128);
                    smallSize = new Size(64, 64);

                    olvList.TileSize = new Size(100, 16);
                    break;
            }

            olvList.ClearObjects();

            olvList.LargeImageList = new ImageList();
            olvList.SmallImageList = new ImageList();

            olvList.LargeImageList.ImageSize = largeSize;
            olvList.SmallImageList.ImageSize = smallSize;

            //olvList.SetObjects(XciHelper.xciList);
        }

        public void ProcessDisplayChange(int displayIndex)
        {
            switch (displayIndex)
            {
                case 0:
                    olvList.View = View.SmallIcon;
                    break;

                case 1:
                    olvList.View = View.LargeIcon;
                    break;

                case 2:
                    olvList.View = View.List;
                    break;

                case 3:
                    olvList.View = View.Tile;
                    break;

                case 4:
                    olvList.View = View.Details;
                    break;
            }
            olvList.Invalidate();
        }

        public bool ProcessFileManagement(XciItem xci)
        {
            if (!Settings.config.isSdEnabled &&
                (xci.fileAction.action == FileAction.Copy
                || xci.fileAction.action == FileAction.Delete
                || xci.fileAction.action == FileAction.Move))
                return false;



            switch (xci.fileAction.action)
            {
                case FileAction.None:
                    break;

                case FileAction.Copy:
                case FileAction.Move:
                    FileHelper.TransferXci(xci);
                    break;

                case FileAction.Delete:
                    try
                    {
                        if (File.Exists(xci.xciFilePath))
                            File.Delete(xci.xciFilePath);

                        XciHelper.UpdateXci(xci);
                        return true;
                    }
                    catch { }

                    return false;

                case FileAction.CompletelyDelete:

                    List<XciItem> deleteItems = GetAllItemsByIdentifer(xci.packageId);

                    for (int i = deleteItems.Count - 1; i >= 0; i--)
                    {
                        if (File.Exists(deleteItems[i].xciFilePath))
                            File.Delete(deleteItems[i].xciFilePath);

                        UpdateXci(deleteItems[i]);
                    }

                    return true;

                case FileAction.Trim:
                    bool trim;
                    if (xci.gameSize != xci.gameUsedSize)
                    {
                        trim = XciHelper.TrimXci(xci);

                        if (trim)
                            UpdateToolStripLabel($"Successfully trimmed {xci.gameName}!");
                        else
                            UpdateToolStripLabel($"Failed to trim {xci.gameName}!");

                        //re-process the XCI
                        RefreshXciInBackground(xci);

                        return trim;
                    }
                    else
                    {
                        UpdateToolStripLabel($"{xci.gameName} is already trimmed!");
                        return true;
                    }

                case FileAction.ShowCert:
                    XciHelper.ShowXciCert(xci);
                    return true;

                case FileAction.ShowXciInfo:
                    if (!string.IsNullOrWhiteSpace(xci.xciFilePath))
                        ShowXciExplorer(xci.xciFilePath);

                    break;

                case FileAction.ShowInExplorer:
                    //todo
                    break;

                default:
                    break;
            }

            return true;
        }

        public void SaveSettings()
        {
            //save the OLV state to olvState byte array (column positions, etc)
            Settings.config.olvState = olvList.SaveState();
            XciHelper.SaveXciCache();
            Settings.SaveSettings();
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
                Application.DoEvents();
            }
        }

        public void UpdateProgressBar(int value = 0)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(delegate { UpdateProgressBar(value); }));
            }
            else
            {
                try
                {
                    if (value == 0)
                        toolStripProgressBar.Value++;
                    else
                        toolStripProgressBar.Value = value;
                }
                catch { }
            }
        }

        public void UpdateProgressLabel(string text)
        {
            toolStripProgressLabel.Visible = true;
            toolStripProgressLabel.Text = text;
        }

        public void UpdateToolStripLabel(string text = "")
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.Invoke(new MethodInvoker(delegate { UpdateToolStripLabel(text); }));
            }
            else
            {
                if (String.IsNullOrWhiteSpace(text))
                {
                    toolStripStatus.Text = $"Displaying {olvList.Items.Count} Switch games.";
                }
                else
                {
                    toolStripStatus.Text = text;
                }
            }
        }

    }
}