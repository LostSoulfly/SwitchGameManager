namespace SwitchGameManager
{
    partial class formRenamer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formRenamer));
            this.olvRenameList = new BrightIdeasSoftware.ObjectListView();
            this.columnPath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnRenamed = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.columnError = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.checkBoxExtensions = new System.Windows.Forms.CheckBox();
            this.checkBoxShowPath = new System.Windows.Forms.CheckBox();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.buttonRename = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonInfo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.olvRenameList)).BeginInit();
            this.SuspendLayout();
            // 
            // olvRenameList
            // 
            this.olvRenameList.AllColumns.Add(this.columnPath);
            this.olvRenameList.AllColumns.Add(this.columnRenamed);
            this.olvRenameList.AllColumns.Add(this.columnError);
            this.olvRenameList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvRenameList.CellEditUseWholeCell = false;
            this.olvRenameList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPath,
            this.columnRenamed});
            this.olvRenameList.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvRenameList.EmptyListMsg = "Nothing to see here!";
            this.olvRenameList.EmptyListMsgFont = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvRenameList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.olvRenameList.FullRowSelect = true;
            this.olvRenameList.GridLines = true;
            this.olvRenameList.Location = new System.Drawing.Point(12, 37);
            this.olvRenameList.Name = "olvRenameList";
            this.olvRenameList.ShowGroups = false;
            this.olvRenameList.Size = new System.Drawing.Size(776, 378);
            this.olvRenameList.TabIndex = 1;
            this.olvRenameList.UseAlternatingBackColors = true;
            this.olvRenameList.UseCompatibleStateImageBehavior = false;
            this.olvRenameList.UseExplorerTheme = true;
            this.olvRenameList.UseFiltering = true;
            this.olvRenameList.UseHotItem = true;
            this.olvRenameList.View = System.Windows.Forms.View.Details;
            // 
            // columnPath
            // 
            this.columnPath.AspectName = "filePath";
            this.columnPath.FillsFreeSpace = true;
            this.columnPath.IsEditable = false;
            this.columnPath.Text = "Original File";
            // 
            // columnRenamed
            // 
            this.columnRenamed.AspectName = "renamePath";
            this.columnRenamed.FillsFreeSpace = true;
            this.columnRenamed.Hideable = false;
            this.columnRenamed.IsEditable = false;
            this.columnRenamed.Text = "Renamed File";
            // 
            // columnError
            // 
            this.columnError.DisplayIndex = 2;
            this.columnError.FillsFreeSpace = true;
            this.columnError.IsEditable = false;
            this.columnError.IsVisible = false;
            this.columnError.Searchable = false;
            this.columnError.Sortable = false;
            this.columnError.Text = "Rename Error";
            this.columnError.UseFiltering = false;
            this.columnError.Width = 150;
            // 
            // checkBoxExtensions
            // 
            this.checkBoxExtensions.AutoSize = true;
            this.checkBoxExtensions.Checked = true;
            this.checkBoxExtensions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxExtensions.Location = new System.Drawing.Point(12, 14);
            this.checkBoxExtensions.Name = "checkBoxExtensions";
            this.checkBoxExtensions.Size = new System.Drawing.Size(126, 17);
            this.checkBoxExtensions.TabIndex = 2;
            this.checkBoxExtensions.Text = "Show File Extensions";
            this.checkBoxExtensions.UseVisualStyleBackColor = true;
            this.checkBoxExtensions.CheckedChanged += new System.EventHandler(this.checkBoxExtensions_CheckedChanged);
            // 
            // checkBoxShowPath
            // 
            this.checkBoxShowPath.AutoSize = true;
            this.checkBoxShowPath.Checked = true;
            this.checkBoxShowPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowPath.Location = new System.Drawing.Point(144, 14);
            this.checkBoxShowPath.Name = "checkBoxShowPath";
            this.checkBoxShowPath.Size = new System.Drawing.Size(97, 17);
            this.checkBoxShowPath.TabIndex = 3;
            this.checkBoxShowPath.Text = "Show Full Path";
            this.checkBoxShowPath.UseVisualStyleBackColor = true;
            this.checkBoxShowPath.CheckedChanged += new System.EventHandler(this.checkBoxShowPath_CheckedChanged);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilter.Location = new System.Drawing.Point(12, 428);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(436, 20);
            this.textBoxFilter.TabIndex = 5;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSearch.Location = new System.Drawing.Point(646, 11);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(142, 20);
            this.textBoxSearch.TabIndex = 6;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // buttonRename
            // 
            this.buttonRename.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRename.Location = new System.Drawing.Point(646, 426);
            this.buttonRename.Name = "buttonRename";
            this.buttonRename.Size = new System.Drawing.Size(141, 23);
            this.buttonRename.TabIndex = 7;
            this.buttonRename.Text = "Rename Listed Games";
            this.buttonRename.UseVisualStyleBackColor = true;
            this.buttonRename.Click += new System.EventHandler(this.buttonRename_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(565, 426);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 8;
            this.buttonClose.Text = "Cancel";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonInfo
            // 
            this.buttonInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonInfo.Location = new System.Drawing.Point(454, 426);
            this.buttonInfo.Name = "buttonInfo";
            this.buttonInfo.Size = new System.Drawing.Size(48, 23);
            this.buttonInfo.TabIndex = 9;
            this.buttonInfo.Text = "Info";
            this.buttonInfo.UseVisualStyleBackColor = true;
            this.buttonInfo.Click += new System.EventHandler(this.buttonInfo_Click);
            // 
            // formRenamer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 461);
            this.Controls.Add(this.buttonInfo);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRename);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.textBoxFilter);
            this.Controls.Add(this.checkBoxShowPath);
            this.Controls.Add(this.checkBoxExtensions);
            this.Controls.Add(this.olvRenameList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "formRenamer";
            this.Text = "XCI Renamer";
            this.Load += new System.EventHandler(this.formRenamer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvRenameList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvRenameList;
        private System.Windows.Forms.CheckBox checkBoxExtensions;
        private System.Windows.Forms.CheckBox checkBoxShowPath;
        private System.Windows.Forms.TextBox textBoxSearch;
        private BrightIdeasSoftware.OLVColumn columnPath;
        private BrightIdeasSoftware.OLVColumn columnRenamed;
        private System.Windows.Forms.Button buttonRename;
        private System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.Button buttonInfo;
        private BrightIdeasSoftware.OLVColumn columnError;
    }
}