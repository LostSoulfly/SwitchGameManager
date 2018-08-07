using BrightIdeasSoftware;
using SwitchGameManager.Helpers;
using SwitchGameManager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static SwitchGameManager.Helpers.FileHelper;
using static SwitchGameManager.Helpers.XciHelper;

namespace SwitchGameManager
{
    public partial class formMain : Form
    {

        const int WM_DEVICECHANGE = 0x0219; //see msdn site
        const int DBT_DEVICEARRIVAL = 0x8000;
        const int DBT_DEVICEREMOVALCOMPLETE = 0x8004;
        const int DBT_DEVTYPVOLUME = 0x00000002;

        private ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        private FileSystemWatcher sdWatcher = new FileSystemWatcher();
        private DriveInfo sdInfo;

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

            Helpers.Settings.config.listIconSize = displayIndex;
            ProcessChangeIconSize(displayIndex);
        }

        private void formMain_Load(object sender, EventArgs e)
        {
            XciHelper.formMain = this;
            FileHelper.formMain = this;

            this.Text += " " + Application.ProductVersion;

            if (!File.Exists("keys.txt"))
            {
                MessageBox.Show("Please make sure to put the keys.txt file in the same folder as SwitchGameManager.exe" + Environment.NewLine + "The program will now close.", "Keys.txt not found");
                Application.Exit();
            }

            SetupObjectListView();

            SetupDelegates();

            //Load the settings
            if (Helpers.Settings.LoadSettings() == false)
                manageXciLocToolStripMenuItem_Click(null, null);

            SetupFileSysWatcher();

            locationToolStripComboBox.SelectedIndex = 0;

            //Setup the OLV with the saved state (if it was saved)
            if (Helpers.Settings.config.olvState != null)
            {
                olvList.RestoreState(Helpers.Settings.config.olvState);
                ProcessChangeIconSize(Helpers.Settings.config.listIconSize);
            }

            if (Helpers.Settings.config.formHeight > 0)
                this.Height = Helpers.Settings.config.formHeight;

            if (Helpers.Settings.config.formWidth > 0)
                this.Width = Helpers.Settings.config.formWidth;

            UpdateToolMenus();
            XciHelper.LoadXcisInBackground();

            /* todo
             * Check if Keys.txt exists, otherwise download it. Maybe procedurally generate the URL? brute force the key?
             * Add red X or green check for Game cert, either use a Resource img or system image?
             */
        }

        private void SetupFileSysWatcher()
        {
            if (Helpers.Settings.CheckForSdCard())
            {
                sdWatcher = new FileSystemWatcher(Helpers.Settings.config.sdDriveLetter);
                sdWatcher.Changed += delegate (object o, FileSystemEventArgs e)
                {
                    //UpdateToolStripLabel("SD Card: " + e.FullPath + " " + e.ChangeType);
                    UpdateSdInfo();
                };
                sdWatcher.Deleted += delegate (object o, FileSystemEventArgs e)
                {
                    //UpdateToolStripLabel("SD Card: " + e.FullPath + " " + e.ChangeType);
                    UpdateSdInfo();
                };
                sdWatcher.Created += delegate (object o, FileSystemEventArgs e)
                {
                    //UpdateToolStripLabel("SD Card: " + e.FullPath + " " + e.ChangeType);
                    UpdateSdInfo();
                };
                sdWatcher.Error += delegate (object sender, ErrorEventArgs e)
                {
                    //UpdateToolStripLabel("SD Card Err: " + e.GetException().Message);
                    UpdateSdInfo();
                };
                sdWatcher.EnableRaisingEvents = true;
                sdWatcher.IncludeSubdirectories = true;
            }

            UpdateSdInfo();
        }

        private void UpdateSdInfo()
        {
            try
            {
                if (Helpers.Settings.CheckForSdCard())
                {
                    sdInfo = new DriveInfo(sdWatcher.Path);
                    sdInfoToolStripStatus.Text = $"{sdInfo.RootDirectory} ({ReadableFileSize(sdInfo.AvailableFreeSpace)}) Free";
                }
                else
                {
                    sdInfoToolStripStatus.Text = $"";
                }
            }
            catch (Exception ex)
            {
                sdInfoToolStripStatus.Text = "Something went wrong";
                sdInfoToolStripStatus.ToolTipText = ex.Message;
            }
        }
        
        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == WM_DEVICECHANGE)
                {
                    DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                    if ((m.WParam.ToInt32() == DBT_DEVICEARRIVAL) && (vol.dbcv_devicetype == DBT_DEVTYPVOLUME))
                    {
                        if (Helpers.Settings.config.sdDriveLetter.Contains(DriveMaskToLetter(vol.dbcv_unitmask).ToString()))
                        {
                            locationToolStripComboBox.SelectedIndex = 1;
                            XciHelper.LoadXcisInBackground();
                            UpdateSdInfo();
                        }
                    }
                    if ((m.WParam.ToInt32() == DBT_DEVICEREMOVALCOMPLETE) && (vol.dbcv_devicetype == DBT_DEVTYPVOLUME))
                    {
                        if (Helpers.Settings.config.sdDriveLetter.Contains(DriveMaskToLetter(vol.dbcv_unitmask).ToString()))
                        {
                            locationToolStripComboBox.SelectedIndex = 0;
                            XciHelper.LoadXcisInBackground();
                            UpdateSdInfo();
                        }
                    }
                }
                base.WndProc(ref m);
            }
            catch { }
        }

        [StructLayout(LayoutKind.Sequential)] //Same layout in mem
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }

        private static char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; //1 = A, 2 = B, 3 = C
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)    // while there is any bit set in the mask shift it right        
            {
                pom = pom / 2;
                cnt++;
            }
            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';
            return letter;
        }

        private bool IsListIndexUsable(bool suppress = false)
        {
            if (olvList.SelectedIndices.Count >= 1)
                return true;

            if (olvList.SelectedIndex < 0)
            {
                if (!suppress)
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
                SetupFileSysWatcher();
                XciHelper.LoadXcisInBackground();
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
                Helpers.Settings.config.defaultView = (XciLocation)locationToolStripComboBox.SelectedIndex;
                XciHelper.RefreshList();
                UpdateToolStripLabel();
            };

            aboutToolStripMenuItem.Click += delegate (object o, EventArgs e)
            {
                formAbout formAbout = new formAbout();
                formAbout.Show();
            };

            this.FormClosing += delegate (object s, FormClosingEventArgs e) { SaveSettings(); };

            this.ResizeEnd += delegate (object s, EventArgs e)
            {
                if (this.WindowState != FormWindowState.Maximized)
                {
                    Helpers.Settings.config.formHeight = this.Height;
                    Helpers.Settings.config.formWidth = this.Width;
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

            XciLocation destination = new XciLocation();
            XciLocation source = new XciLocation();
            if (Helpers.Settings.config.defaultView == XciLocation.PC)
            {
                destination = XciLocation.SD;
                source = XciLocation.PC;
            }
            else
            {
                destination = XciLocation.PC;
                source = XciLocation.SD;
            }
            
            if (IsListIndexUsable(true))
            {
                ToolStripMenuItem toolStripMenu = (ToolStripMenuItem)contextMenuStrip.Items[0];
                gameManagementToolStripMenuItem.Enabled = true;
                contextMenuStrip.Enabled = true;

                copyToolStripMenuItem.Text = $"Copy to {destination}";
                moveToolStripMenuItem.Text = $"Move to {destination}";
                deleteToolStripMenuItem.Text = $"Delete from {source}";

                XciItem selected = (XciItem)olvList.SelectedObject;
                if (olvList.SelectedIndices.Count > 1)
                {
                    trimGameToolStripMenuItem.Text = $"Trim {olvList.SelectedIndices.Count} Games";
                    copyToolStripMenuItem.Text = $"Copy {olvList.SelectedIndices.Count} Games to {destination}";
                    moveToolStripMenuItem.Text = $"Move {olvList.SelectedIndices.Count} Games to {destination}";
                    deleteToolStripMenuItem.Text = $"Delete {olvList.SelectedIndices.Count} Games from {source}";
                    showInXCIExplorerToolStripMenuItem.Text = $"Show {olvList.SelectedIndices.Count} Games in XCI Explorer";
                    showInWindowsExplorerToolStripMenuItem.Text = $"Show {olvList.SelectedIndices.Count} Games in Windows Explorer";
                    showXCICertificateToolStripMenuItem.Text = $"Show {olvList.SelectedIndices.Count} XCI Certificates";
                }
                else
                {
                    trimGameToolStripMenuItem.Text = $"Trim {selected.gameName}";
                    copyToolStripMenuItem.Text = $"Copy {selected.gameName} to {destination}";
                    moveToolStripMenuItem.Text = $"Move {selected.gameName} to {destination}";
                    deleteToolStripMenuItem.Text = $"Delete {selected.gameName} from {source}";
                    showInXCIExplorerToolStripMenuItem.Text = $"Show {selected.gameName} in XCI Explorer";
                    showInWindowsExplorerToolStripMenuItem.Text = $"Show {selected.gameName} in Windows Explorer";
                    showXCICertificateToolStripMenuItem.Text = $"Show {selected.gameName} XCI Certificate";
                }

                contextMenuStrip.Items[2].Text = trimGameToolStripMenuItem.Text;
                contextMenuStrip.Items[4].Text = showXCICertificateToolStripMenuItem.Text;
                contextMenuStrip.Items[5].Text = showInXCIExplorerToolStripMenuItem.Text;
                contextMenuStrip.Items[6].Text = showInWindowsExplorerToolStripMenuItem.Text;

                toolStripMenu.DropDownItems[0].Text = copyToolStripMenuItem.Text;
                toolStripMenu.DropDownItems[1].Text = moveToolStripMenuItem.Text;
                toolStripMenu.DropDownItems[2].Text = deleteToolStripMenuItem.Text;
            }
            else
            {
                gameManagementToolStripMenuItem.Enabled = false;
                contextMenuStrip.Enabled = false;
            }
                        
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
                true, Resources.trimmed
            });

            this.olvColumnLocalGame.Renderer = new MappedImageRenderer(new Object[] {
                true, Resources.computer
            });

            this.olvColumnSdGame.Renderer = new MappedImageRenderer(new Object[] {
                true, Resources.microsd
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

            //Refresh Game Info
            if (toolIndex == 3)
            {
                List<XciItem> filtered = olvList.SelectedObjects.Cast<XciItem>().ToList();

                foreach (XciItem obj in filtered)
                    XciHelper.RefreshXciInBackground(obj);

                return;
            }


            if (Helpers.Settings.config.defaultView == XciLocation.PC)
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

                List<XciItem> filtered = olvList.SelectedObjects.Cast<XciItem>().ToList();

                double totalSize = filtered.Sum(x => x.gameSize);
                double sizeDiff;
                if (fileAction.action == FileAction.Move || fileAction.action == FileAction.Copy)
                {
                    try
                    {
                        if ((double)sdInfo.AvailableFreeSpace <= totalSize)
                        {
                            sizeDiff = totalSize - (double)sdInfo.AvailableFreeSpace;
                            MessageBox.Show($"Unable to copy games to SD! {Environment.NewLine} You need {ReadableFileSize(sizeDiff)} more space for this transfer!", "Unable To Transfer", MessageBoxButtons.OK);
                            return;
                        }
                    }
                    catch { }

                    message = $"Are you sure you want to {action} {olvList.SelectedObjects.Count} games ({ReadableFileSize(totalSize)}) to {destination}?";
                }

                if (fileAction.action == FileAction.Delete)
                    message = $"Are you sure you want to {action} {olvList.SelectedObjects.Count} games from {source}?";

                if (MessageBox.Show(message, $"Confirm {action.ToUpperInvariant()}", MessageBoxButtons.YesNoCancel) != DialogResult.Yes)
                    return;

                

                foreach (XciItem obj in filtered)
                {
                    xci = Clone(obj);
                    xci.fileAction = Clone(fileAction);
                    if (ProcessFileManagement(xci))
                        success++;
                    else
                        failure++;
                }

                if (fileAction.action == FileAction.Copy || fileAction.action == FileAction.Move)
                    UpdateToolStripLabel($"Games queued for {action.ToUpperInvariant()}: {success}");
                else
                    UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {success} Failed: {failure}");
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

                xci.fileAction = fileAction;
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

            if (toolIndex == 1) fileAction.action = FileAction.CompletelyDelete;
            if (toolIndex == 2) fileAction.action = FileAction.Trim;
            if (toolIndex == 3) fileAction.action = FileAction.ShowRenameWindow;
            if (toolIndex == 4) fileAction.action = FileAction.ShowCert;
            if (toolIndex == 5) fileAction.action = FileAction.ShowXciInfo;
            if (toolIndex == 6) fileAction.action = FileAction.ShowInExplorer;

            if (Helpers.Settings.config.defaultView == XciLocation.PC)
            {
                fileAction.destination = XciLocation.SD;
                fileAction.source = XciLocation.PC;
            }
            else
            {
                fileAction.destination = XciLocation.PC;
                fileAction.source = XciLocation.SD;
            }

            switch (fileAction.action)
            {
                case FileAction.ShowRenameWindow:
                    formRenamer renamer = new formRenamer();

                    List<XciItem> renameList = olvList.SelectedObjects.Cast<XciItem>().ToList();

                    renamer.PopulateList(renameList);
                    renamer.Show();
                    return;

                case FileAction.ShowInExplorer:
                case FileAction.ShowCert:
                case FileAction.ShowXciInfo:
                    List<XciItem> showInfo = olvList.SelectedObjects.Cast<XciItem>().ToList();

                    foreach (XciItem item in showInfo)
                    {
                        item.fileAction = Clone(fileAction);
                        ProcessFileManagement(item);
                    }
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

                List<XciItem> actionList = olvList.SelectedObjects.Cast<XciItem>().ToList();

                foreach (XciItem obj in actionList)
                {
                    xci = Clone(obj);
                    xci.fileAction = Clone(fileAction);

                    if (ProcessFileManagement(xci))
                        success++;
                    else
                        failure++;

                    UpdateToolStripLabel($"{action.ToUpperInvariant()} results: Success: {success} Failed: {failure}");

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


            olvList.LargeImageList = new ImageList();
            olvList.SmallImageList = new ImageList();

            olvList.LargeImageList.ImageSize = largeSize;
            olvList.SmallImageList.ImageSize = smallSize;

            Helpers.XciHelper.RefreshList();
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
            bool success = false;

            if (!Helpers.Settings.CheckForSdCard() &&
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

                        if (!File.Exists(xci.xciFilePath))
                            success = true;

                        xci.fileAction.actionCompleted = true;
                        xci.fileAction.actionSuccess = success;

                        XciHelper.UpdateXci(xci);
                        return success;
                    }
                    catch { }

                    return false;

                case FileAction.CompletelyDelete:

                    List<XciItem> deleteItems = GetAllItemsByIdentifer(xci.packageId);

                    for (int i = deleteItems.Count - 1; i >= 0; i--)
                    {
                        if (File.Exists(deleteItems[i].xciFilePath))
                            File.Delete(deleteItems[i].xciFilePath);

                        if (!File.Exists(deleteItems[i].xciFilePath))
                            success = true;

                        deleteItems[i].fileAction = Clone(xci.fileAction);
                        deleteItems[i].fileAction.actionCompleted = true;
                        deleteItems[i].fileAction.actionSuccess = success;

                        UpdateXci(deleteItems[i]);
                    }

                    return success;

                case FileAction.Trim:
                    bool trim;
                    if (xci.gameSize != xci.gameUsedSize)
                    {
                        trim = XciHelper.TrimXci(xci);

                        if (trim)
                            UpdateToolStripLabel($"Successfully trimmed {xci.gameName}!");
                        else
                            UpdateToolStripLabel($"Failed to trim {xci.gameName}!");

                        xci.fileAction.actionCompleted = true;
                        xci.fileAction.actionSuccess = true;

                        //re-process the XCI
                        //RefreshXciInBackground(xci);

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
                    if (!File.Exists(xci.xciFilePath))
                    {
                        UpdateToolStripLabel($"{xci.xciFilePath} could not be found!");
                        return false;
                    }

                    Process.Start("explorer.exe", "/select, \"" + xci.xciFilePath + "\"");
                    return true;

                default:
                    break;
            }

            return true;
        }

        public void SaveSettings()
        {
            //save the OLV state to olvState byte array (column positions, etc)
            Helpers.Settings.config.olvState = olvList.SaveState();
            XciHelper.SaveXciCache();
            Helpers.Settings.SaveSettings();
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