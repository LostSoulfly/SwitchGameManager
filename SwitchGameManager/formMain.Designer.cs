using System.Drawing;

namespace SwitchGameManager
{
    partial class formMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formMain));
            this.olvLocal = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnXciName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnDeveloper = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnTitleID = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnProductCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnPackageId = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnGameCardCapacity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnGameSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnGameUsedSize = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnGameCertEmpty = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnisXciTrimmed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnLocalGame = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSdGame = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnXciPath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnSdFilePath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.generalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageXciLocToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToSDCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToSDCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelTransfersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToPCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelTransfersToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trimGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameXCIFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showXCICertificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.visualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayStyleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.largeIconsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.detailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconsOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iconSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.biggestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bigger128x128ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.small64x64ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.smallest32x32ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.themesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSpring = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.olvLocal)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvLocal
            // 
            this.olvLocal.AllColumns.Add(this.olvColumnXciName);
            this.olvLocal.AllColumns.Add(this.olvColumnDeveloper);
            this.olvLocal.AllColumns.Add(this.olvColumnTitleID);
            this.olvLocal.AllColumns.Add(this.olvColumnProductCode);
            this.olvLocal.AllColumns.Add(this.olvColumnPackageId);
            this.olvLocal.AllColumns.Add(this.olvColumnGameCardCapacity);
            this.olvLocal.AllColumns.Add(this.olvColumnGameSize);
            this.olvLocal.AllColumns.Add(this.olvColumnGameUsedSize);
            this.olvLocal.AllColumns.Add(this.olvColumnGameCertEmpty);
            this.olvLocal.AllColumns.Add(this.olvColumnisXciTrimmed);
            this.olvLocal.AllColumns.Add(this.olvColumnLocalGame);
            this.olvLocal.AllColumns.Add(this.olvColumnSdGame);
            this.olvLocal.AllColumns.Add(this.olvColumnXciPath);
            this.olvLocal.AllColumns.Add(this.olvColumnSdFilePath);
            this.olvLocal.AllowColumnReorder = true;
            this.olvLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvLocal.CellEditUseWholeCell = false;
            this.olvLocal.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnXciName,
            this.olvColumnDeveloper,
            this.olvColumnTitleID,
            this.olvColumnProductCode,
            this.olvColumnGameSize,
            this.olvColumnGameCertEmpty,
            this.olvColumnisXciTrimmed,
            this.olvColumnLocalGame,
            this.olvColumnSdGame,
            this.olvColumnXciPath});
            this.olvLocal.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvLocal.EmptyListMsg = "No Switch Games found!";
            this.olvLocal.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.olvLocal.FullRowSelect = true;
            this.olvLocal.HeaderUsesThemes = true;
            this.olvLocal.LabelWrap = false;
            this.olvLocal.Location = new System.Drawing.Point(12, 37);
            this.olvLocal.Name = "olvLocal";
            this.olvLocal.ShowGroups = false;
            this.olvLocal.ShowImagesOnSubItems = true;
            this.olvLocal.ShowItemCountOnGroups = true;
            this.olvLocal.Size = new System.Drawing.Size(935, 381);
            this.olvLocal.TabIndex = 0;
            this.olvLocal.TileSize = new System.Drawing.Size(300, 50);
            this.olvLocal.TintSortColumn = true;
            this.olvLocal.UseCompatibleStateImageBehavior = false;
            this.olvLocal.UseFiltering = true;
            this.olvLocal.UseHotControls = false;
            this.olvLocal.UseHotItem = true;
            this.olvLocal.UseNotifyPropertyChanged = true;
            this.olvLocal.UseTranslucentHotItem = true;
            this.olvLocal.UseTranslucentSelection = true;
            this.olvLocal.View = System.Windows.Forms.View.Details;
            this.olvLocal.MouseClick += new System.Windows.Forms.MouseEventHandler(this.olvLocal_MouseClick);
            this.olvLocal.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.olvLocal_MouseDoubleClick);
            // 
            // olvColumnXciName
            // 
            this.olvColumnXciName.AspectName = "gameName";
            this.olvColumnXciName.AspectToStringFormat = "";
            this.olvColumnXciName.ButtonPadding = new System.Drawing.Size(10, 10);
            this.olvColumnXciName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnXciName.IsEditable = false;
            this.olvColumnXciName.IsTileViewColumn = true;
            this.olvColumnXciName.MinimumWidth = 125;
            this.olvColumnXciName.Text = "Game";
            this.olvColumnXciName.UseInitialLetterForGroup = true;
            this.olvColumnXciName.Width = 150;
            this.olvColumnXciName.WordWrap = true;
            // 
            // olvColumnDeveloper
            // 
            this.olvColumnDeveloper.AspectName = "gameDeveloper";
            this.olvColumnDeveloper.IsTileViewColumn = true;
            this.olvColumnDeveloper.MinimumWidth = 75;
            this.olvColumnDeveloper.Text = "Developer";
            this.olvColumnDeveloper.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnDeveloper.Width = 110;
            // 
            // olvColumnTitleID
            // 
            this.olvColumnTitleID.AspectName = "titleId";
            this.olvColumnTitleID.IsEditable = false;
            this.olvColumnTitleID.IsTileViewColumn = true;
            this.olvColumnTitleID.MinimumWidth = 75;
            this.olvColumnTitleID.Text = "Title ID";
            this.olvColumnTitleID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnTitleID.UseInitialLetterForGroup = true;
            this.olvColumnTitleID.Width = 100;
            // 
            // olvColumnProductCode
            // 
            this.olvColumnProductCode.AspectName = "productCode";
            this.olvColumnProductCode.IsEditable = false;
            this.olvColumnProductCode.MinimumWidth = 50;
            this.olvColumnProductCode.Text = "Product Code";
            this.olvColumnProductCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnProductCode.Width = 85;
            // 
            // olvColumnPackageId
            // 
            this.olvColumnPackageId.AspectName = "packageId";
            this.olvColumnPackageId.DisplayIndex = 4;
            this.olvColumnPackageId.IsVisible = false;
            this.olvColumnPackageId.MinimumWidth = 75;
            this.olvColumnPackageId.Text = "PackageID";
            this.olvColumnPackageId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnPackageId.Width = 125;
            // 
            // olvColumnGameCardCapacity
            // 
            this.olvColumnGameCardCapacity.AspectName = "gameCardCapacity";
            this.olvColumnGameCardCapacity.DisplayIndex = 5;
            this.olvColumnGameCardCapacity.IsEditable = false;
            this.olvColumnGameCardCapacity.IsVisible = false;
            this.olvColumnGameCardCapacity.MinimumWidth = 50;
            this.olvColumnGameCardCapacity.Text = "Game Card Capacity";
            this.olvColumnGameCardCapacity.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // olvColumnGameSize
            // 
            this.olvColumnGameSize.AspectName = "gameSize";
            this.olvColumnGameSize.IsEditable = false;
            this.olvColumnGameSize.MinimumWidth = 50;
            this.olvColumnGameSize.Text = "Game Size";
            this.olvColumnGameSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnGameSize.Width = 68;
            // 
            // olvColumnGameUsedSize
            // 
            this.olvColumnGameUsedSize.AspectName = "gameUsedSize";
            this.olvColumnGameUsedSize.DisplayIndex = 4;
            this.olvColumnGameUsedSize.IsEditable = false;
            this.olvColumnGameUsedSize.IsVisible = false;
            this.olvColumnGameUsedSize.MinimumWidth = 50;
            this.olvColumnGameUsedSize.Searchable = false;
            this.olvColumnGameUsedSize.Text = "Used Size";
            this.olvColumnGameUsedSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnGameUsedSize.Width = 65;
            // 
            // olvColumnGameCertEmpty
            // 
            this.olvColumnGameCertEmpty.AspectName = "isUniqueCert";
            this.olvColumnGameCertEmpty.DisplayIndex = 6;
            this.olvColumnGameCertEmpty.IsEditable = false;
            this.olvColumnGameCertEmpty.IsTileViewColumn = true;
            this.olvColumnGameCertEmpty.MinimumWidth = 35;
            this.olvColumnGameCertEmpty.Searchable = false;
            this.olvColumnGameCertEmpty.Text = "Unique Cert";
            this.olvColumnGameCertEmpty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnGameCertEmpty.UseFiltering = false;
            this.olvColumnGameCertEmpty.Width = 69;
            // 
            // olvColumnisXciTrimmed
            // 
            this.olvColumnisXciTrimmed.AspectName = "isXciTrimmed";
            this.olvColumnisXciTrimmed.DisplayIndex = 5;
            this.olvColumnisXciTrimmed.IsEditable = false;
            this.olvColumnisXciTrimmed.IsTileViewColumn = true;
            this.olvColumnisXciTrimmed.MinimumWidth = 35;
            this.olvColumnisXciTrimmed.Searchable = false;
            this.olvColumnisXciTrimmed.Text = "Trimmed";
            this.olvColumnisXciTrimmed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnisXciTrimmed.UseFiltering = false;
            // 
            // olvColumnLocalGame
            // 
            this.olvColumnLocalGame.AspectName = "isGameOnPc";
            this.olvColumnLocalGame.Text = "On PC";
            this.olvColumnLocalGame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnLocalGame.Width = 74;
            // 
            // olvColumnSdGame
            // 
            this.olvColumnSdGame.AspectName = "isGameOnSd";
            this.olvColumnSdGame.Text = "On SD";
            this.olvColumnSdGame.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnSdGame.Width = 76;
            // 
            // olvColumnXciPath
            // 
            this.olvColumnXciPath.AspectName = "xciFilePath";
            this.olvColumnXciPath.FillsFreeSpace = true;
            this.olvColumnXciPath.IsEditable = false;
            this.olvColumnXciPath.IsTileViewColumn = true;
            this.olvColumnXciPath.MinimumWidth = 125;
            this.olvColumnXciPath.Text = "Local File";
            this.olvColumnXciPath.Width = 125;
            // 
            // olvColumnSdFilePath
            // 
            this.olvColumnSdFilePath.AspectName = "xciSdFilePath";
            this.olvColumnSdFilePath.DisplayIndex = 10;
            this.olvColumnSdFilePath.FillsFreeSpace = true;
            this.olvColumnSdFilePath.IsVisible = false;
            this.olvColumnSdFilePath.Text = "SD File Path";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(804, 11);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(138, 20);
            this.textBoxFilter.TabIndex = 3;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalToolStripMenuItem,
            this.gameManagementToolStripMenuItem,
            this.visualToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(959, 24);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "Menu";
            // 
            // generalToolStripMenuItem
            // 
            this.generalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.toolStripMenuItem1,
            this.aboutToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.generalToolStripMenuItem.Name = "generalToolStripMenuItem";
            this.generalToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.generalToolStripMenuItem.Text = "General";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageXciLocToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // manageXciLocToolStripMenuItem
            // 
            this.manageXciLocToolStripMenuItem.Name = "manageXciLocToolStripMenuItem";
            this.manageXciLocToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.manageXciLocToolStripMenuItem.Text = "Manage XCI Locations";
            this.manageXciLocToolStripMenuItem.Click += new System.EventHandler(this.manageXciLocToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // gameManagementToolStripMenuItem
            // 
            this.gameManagementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sdToolStripMenuItem,
            this.copyToPCToolStripMenuItem,
            this.deleteGameToolStripMenuItem,
            this.trimGameToolStripMenuItem,
            this.renameXCIFilesToolStripMenuItem,
            this.showXCICertificateToolStripMenuItem});
            this.gameManagementToolStripMenuItem.Name = "gameManagementToolStripMenuItem";
            this.gameManagementToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.gameManagementToolStripMenuItem.Text = "Game Management";
            // 
            // sdToolStripMenuItem
            // 
            this.sdToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToSDCardToolStripMenuItem,
            this.moveToSDCardToolStripMenuItem,
            this.cancelTransfersToolStripMenuItem});
            this.sdToolStripMenuItem.Name = "sdToolStripMenuItem";
            this.sdToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.sdToolStripMenuItem.Text = "SD Storage";
            // 
            // sendToSDCardToolStripMenuItem
            // 
            this.sendToSDCardToolStripMenuItem.Name = "sendToSDCardToolStripMenuItem";
            this.sendToSDCardToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.sendToSDCardToolStripMenuItem.Text = "Copy To SD Card";
            this.sendToSDCardToolStripMenuItem.Click += new System.EventHandler(this.ToolStripSdManagement);
            // 
            // moveToSDCardToolStripMenuItem
            // 
            this.moveToSDCardToolStripMenuItem.Name = "moveToSDCardToolStripMenuItem";
            this.moveToSDCardToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.moveToSDCardToolStripMenuItem.Text = "Move To SD Card";
            // 
            // cancelTransfersToolStripMenuItem
            // 
            this.cancelTransfersToolStripMenuItem.Name = "cancelTransfersToolStripMenuItem";
            this.cancelTransfersToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.cancelTransfersToolStripMenuItem.Text = "Cancel Transfers";
            this.cancelTransfersToolStripMenuItem.Click += new System.EventHandler(this.CancelFileTransfers);
            // 
            // copyToPCToolStripMenuItem
            // 
            this.copyToPCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendToPCToolStripMenuItem,
            this.moveToPCToolStripMenuItem,
            this.cancelTransfersToolStripMenuItem1});
            this.copyToPCToolStripMenuItem.Name = "copyToPCToolStripMenuItem";
            this.copyToPCToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.copyToPCToolStripMenuItem.Text = "PC Storage";
            // 
            // sendToPCToolStripMenuItem
            // 
            this.sendToPCToolStripMenuItem.Name = "sendToPCToolStripMenuItem";
            this.sendToPCToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.sendToPCToolStripMenuItem.Text = "Copy To PC";
            this.sendToPCToolStripMenuItem.Click += new System.EventHandler(this.ToolStripPcManagement);
            // 
            // moveToPCToolStripMenuItem
            // 
            this.moveToPCToolStripMenuItem.Name = "moveToPCToolStripMenuItem";
            this.moveToPCToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.moveToPCToolStripMenuItem.Text = "Move To PC";
            // 
            // cancelTransfersToolStripMenuItem1
            // 
            this.cancelTransfersToolStripMenuItem1.Name = "cancelTransfersToolStripMenuItem1";
            this.cancelTransfersToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.cancelTransfersToolStripMenuItem1.Text = "Cancel Transfers";
            this.cancelTransfersToolStripMenuItem1.Click += new System.EventHandler(this.CancelFileTransfers);
            // 
            // deleteGameToolStripMenuItem
            // 
            this.deleteGameToolStripMenuItem.Name = "deleteGameToolStripMenuItem";
            this.deleteGameToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteGameToolStripMenuItem.Text = "Delete Game(s)";
            this.deleteGameToolStripMenuItem.Click += new System.EventHandler(this.ToolStripManagement);
            // 
            // trimGameToolStripMenuItem
            // 
            this.trimGameToolStripMenuItem.Name = "trimGameToolStripMenuItem";
            this.trimGameToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.trimGameToolStripMenuItem.Text = "Trim Game(s)";
            this.trimGameToolStripMenuItem.Click += new System.EventHandler(this.ToolStripManagement);
            // 
            // renameXCIFilesToolStripMenuItem
            // 
            this.renameXCIFilesToolStripMenuItem.Name = "renameXCIFilesToolStripMenuItem";
            this.renameXCIFilesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.renameXCIFilesToolStripMenuItem.Text = "Rename XCI File(s)";
            this.renameXCIFilesToolStripMenuItem.Click += new System.EventHandler(this.ToolStripManagement);
            // 
            // showXCICertificateToolStripMenuItem
            // 
            this.showXCICertificateToolStripMenuItem.Name = "showXCICertificateToolStripMenuItem";
            this.showXCICertificateToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.showXCICertificateToolStripMenuItem.Text = "Show XCI Certificate";
            this.showXCICertificateToolStripMenuItem.Click += new System.EventHandler(this.ToolStripManagement);
            // 
            // visualToolStripMenuItem
            // 
            this.visualToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.displayStyleToolStripMenuItem,
            this.iconSizeToolStripMenuItem,
            this.themesToolStripMenuItem});
            this.visualToolStripMenuItem.Name = "visualToolStripMenuItem";
            this.visualToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.visualToolStripMenuItem.Text = "Visual Options";
            // 
            // displayStyleToolStripMenuItem
            // 
            this.displayStyleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.smallIconsToolStripMenuItem,
            this.largeIconsToolStripMenuItem,
            this.listToolStripMenuItem,
            this.tilesToolStripMenuItem,
            this.detailsToolStripMenuItem,
            this.iconsOnlyToolStripMenuItem});
            this.displayStyleToolStripMenuItem.Name = "displayStyleToolStripMenuItem";
            this.displayStyleToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.displayStyleToolStripMenuItem.Text = "List Display Style";
            // 
            // smallIconsToolStripMenuItem
            // 
            this.smallIconsToolStripMenuItem.Name = "smallIconsToolStripMenuItem";
            this.smallIconsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.smallIconsToolStripMenuItem.Text = "Small Icons";
            this.smallIconsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripDisplayChange);
            // 
            // largeIconsToolStripMenuItem
            // 
            this.largeIconsToolStripMenuItem.Name = "largeIconsToolStripMenuItem";
            this.largeIconsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.largeIconsToolStripMenuItem.Text = "Large Icons";
            this.largeIconsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripDisplayChange);
            // 
            // listToolStripMenuItem
            // 
            this.listToolStripMenuItem.Name = "listToolStripMenuItem";
            this.listToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.listToolStripMenuItem.Text = "List";
            this.listToolStripMenuItem.Click += new System.EventHandler(this.ToolStripDisplayChange);
            // 
            // tilesToolStripMenuItem
            // 
            this.tilesToolStripMenuItem.Name = "tilesToolStripMenuItem";
            this.tilesToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.tilesToolStripMenuItem.Text = "Tiles";
            this.tilesToolStripMenuItem.Click += new System.EventHandler(this.ToolStripDisplayChange);
            // 
            // detailsToolStripMenuItem
            // 
            this.detailsToolStripMenuItem.Name = "detailsToolStripMenuItem";
            this.detailsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.detailsToolStripMenuItem.Text = "Details";
            this.detailsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripDisplayChange);
            // 
            // iconsOnlyToolStripMenuItem
            // 
            this.iconsOnlyToolStripMenuItem.Enabled = false;
            this.iconsOnlyToolStripMenuItem.Name = "iconsOnlyToolStripMenuItem";
            this.iconsOnlyToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.iconsOnlyToolStripMenuItem.Text = "Icons Only";
            // 
            // iconSizeToolStripMenuItem
            // 
            this.iconSizeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.biggestToolStripMenuItem,
            this.bigger128x128ToolStripMenuItem,
            this.small64x64ToolStripMenuItem,
            this.smallest32x32ToolStripMenuItem});
            this.iconSizeToolStripMenuItem.Name = "iconSizeToolStripMenuItem";
            this.iconSizeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.iconSizeToolStripMenuItem.Text = "Icon Size";
            // 
            // biggestToolStripMenuItem
            // 
            this.biggestToolStripMenuItem.Name = "biggestToolStripMenuItem";
            this.biggestToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.biggestToolStripMenuItem.Text = "Biggest (256x256)";
            this.biggestToolStripMenuItem.Click += new System.EventHandler(this.ChangeIconSize);
            // 
            // bigger128x128ToolStripMenuItem
            // 
            this.bigger128x128ToolStripMenuItem.Name = "bigger128x128ToolStripMenuItem";
            this.bigger128x128ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.bigger128x128ToolStripMenuItem.Text = "Bigger {128x128)";
            this.bigger128x128ToolStripMenuItem.Click += new System.EventHandler(this.ChangeIconSize);
            // 
            // small64x64ToolStripMenuItem
            // 
            this.small64x64ToolStripMenuItem.Name = "small64x64ToolStripMenuItem";
            this.small64x64ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.small64x64ToolStripMenuItem.Text = "Small (64x64)";
            this.small64x64ToolStripMenuItem.Click += new System.EventHandler(this.ChangeIconSize);
            // 
            // smallest32x32ToolStripMenuItem
            // 
            this.smallest32x32ToolStripMenuItem.Name = "smallest32x32ToolStripMenuItem";
            this.smallest32x32ToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.smallest32x32ToolStripMenuItem.Text = "Smallest (32x32)";
            this.smallest32x32ToolStripMenuItem.Click += new System.EventHandler(this.ChangeIconSize);
            // 
            // themesToolStripMenuItem
            // 
            this.themesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backgroundImageToolStripMenuItem});
            this.themesToolStripMenuItem.Enabled = false;
            this.themesToolStripMenuItem.Name = "themesToolStripMenuItem";
            this.themesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.themesToolStripMenuItem.Text = "Themes";
            // 
            // backgroundImageToolStripMenuItem
            // 
            this.backgroundImageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearImageToolStripMenuItem,
            this.selectImageToolStripMenuItem});
            this.backgroundImageToolStripMenuItem.Name = "backgroundImageToolStripMenuItem";
            this.backgroundImageToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.backgroundImageToolStripMenuItem.Text = "Background Image";
            // 
            // clearImageToolStripMenuItem
            // 
            this.clearImageToolStripMenuItem.Name = "clearImageToolStripMenuItem";
            this.clearImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.clearImageToolStripMenuItem.Text = "Clear Image";
            // 
            // selectImageToolStripMenuItem
            // 
            this.selectImageToolStripMenuItem.Name = "selectImageToolStripMenuItem";
            this.selectImageToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.selectImageToolStripMenuItem.Text = "Select Image..";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatus,
            this.toolStripSpring,
            this.toolStripProgressLabel,
            this.toolStripProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 421);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(959, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatus
            // 
            this.toolStripStatus.Name = "toolStripStatus";
            this.toolStripStatus.Size = new System.Drawing.Size(84, 17);
            this.toolStripStatus.Text = "toolStripStatus";
            // 
            // toolStripSpring
            // 
            this.toolStripSpring.Name = "toolStripSpring";
            this.toolStripSpring.Size = new System.Drawing.Size(758, 17);
            this.toolStripSpring.Spring = true;
            // 
            // toolStripProgressLabel
            // 
            this.toolStripProgressLabel.Name = "toolStripProgressLabel";
            this.toolStripProgressLabel.Size = new System.Drawing.Size(89, 17);
            this.toolStripProgressLabel.Text = "Copy progress..";
            this.toolStripProgressLabel.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 443);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.olvLocal);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SwitchGameManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.Resize += new System.EventHandler(this.formMain_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.olvLocal)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private BrightIdeasSoftware.OLVColumn olvColumnXciName;
        private BrightIdeasSoftware.OLVColumn olvColumnDeveloper;
        private BrightIdeasSoftware.OLVColumn olvColumnisXciTrimmed;
        private BrightIdeasSoftware.OLVColumn olvColumnTitleID;
        private BrightIdeasSoftware.OLVColumn olvColumnXciPath;
        private BrightIdeasSoftware.OLVColumn olvColumnGameCardCapacity;
        private BrightIdeasSoftware.OLVColumn olvColumnGameSize;
        private BrightIdeasSoftware.OLVColumn olvColumnGameUsedSize;
        private BrightIdeasSoftware.OLVColumn olvColumnProductCode;
        private BrightIdeasSoftware.OLVColumn olvColumnGameCertEmpty;
        private System.Windows.Forms.TextBox textBoxFilter;
        private BrightIdeasSoftware.OLVColumn olvColumnPackageId;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem generalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trimGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameXCIFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayStyleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem largeIconsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem detailsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showXCICertificateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem themesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backgroundImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectImageToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripSpring;
        private System.Windows.Forms.ToolStripStatusLabel toolStripProgressLabel;
        private System.Windows.Forms.ToolStripMenuItem iconSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem biggestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bigger128x128ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem small64x64ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem smallest32x32ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem iconsOnlyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToSDCardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToSDCardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendToPCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToPCToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnLocalGame;
        private BrightIdeasSoftware.OLVColumn olvColumnSdGame;
        public BrightIdeasSoftware.ObjectListView olvLocal;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripMenuItem manageXciLocToolStripMenuItem;
        private BrightIdeasSoftware.OLVColumn olvColumnSdFilePath;
        private System.Windows.Forms.ToolStripMenuItem cancelTransfersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelTransfersToolStripMenuItem1;
    }
}

