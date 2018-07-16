using BrightIdeasSoftware;
using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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

        private bool ProcessManagementAction(XciItem xci, int toolIndex)
        {
            return true;
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

            olvList.MouseDoubleClick += delegate (object s, MouseEventArgs e)
            {
                XciItem xci = (XciItem)olvList.GetItem(olvList.SelectedIndex).RowObject;
                XciHelper.ShowXciExplorer(xci.xciFilePath);
            };

            exitToolStripMenuItem.Click += delegate (object s, EventArgs e) { Application.Exit(); };

            locationToolStripComboBox.SelectedIndexChanged += delegate (object s, EventArgs e) { Settings.config.defaultView = locationToolStripComboBox.SelectedIndex; XciHelper.RefreshList(); };

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
            cancelTransfersToolStripMenuItem1.Click += delegate (object s, EventArgs e) { FileHelper.StopTransfers(); };
            rebuildCachetoolStripMenuItem.Click += delegate (object s, EventArgs e) { XciHelper.RebuildCache(); };
            refreshGamesListToolStripMenuItem.Click += delegate (object s, EventArgs e) { XciHelper.LoadXcisInBackground(); };
        }

        private void UpdateToolMenus()
        {

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

        }

        private void ToolStripManagement(object sender, EventArgs e)
        {

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

        public bool ProcessFileManagement(XciItem xci, int toolIndex, bool isSdAction, bool isPcAction)
        {
            

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
            if (String.IsNullOrWhiteSpace(text) && textBoxFilter.Text.Length > 0)
            {
                //toolStripStatus.Text = $"Displaying {olvList.Items.Count} out of {XciHelper.xciList.Count} Switch games.";
            }
            else if (String.IsNullOrWhiteSpace(text))
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