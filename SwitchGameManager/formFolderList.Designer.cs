namespace SwitchGameManager
{
    partial class formFolderList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formFolderList));
            this.listBoxFolders = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonLocateSwitchSd = new System.Windows.Forms.Button();
            this.textBoxDriveInfo = new System.Windows.Forms.TextBox();
            this.sdRootLabel = new System.Windows.Forms.Label();
            this.comboBoxDriveLetters = new System.Windows.Forms.ComboBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonAddFolder = new System.Windows.Forms.Button();
            this.buttonRemoveFolder = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxFolders
            // 
            this.listBoxFolders.FormattingEnabled = true;
            this.listBoxFolders.Location = new System.Drawing.Point(12, 12);
            this.listBoxFolders.Name = "listBoxFolders";
            this.listBoxFolders.Size = new System.Drawing.Size(376, 95);
            this.listBoxFolders.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonLocateSwitchSd);
            this.groupBox1.Controls.Add(this.textBoxDriveInfo);
            this.groupBox1.Controls.Add(this.sdRootLabel);
            this.groupBox1.Controls.Add(this.comboBoxDriveLetters);
            this.groupBox1.Location = new System.Drawing.Point(12, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(295, 161);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SD Card";
            // 
            // buttonLocateSwitchSd
            // 
            this.buttonLocateSwitchSd.Location = new System.Drawing.Point(214, 19);
            this.buttonLocateSwitchSd.Name = "buttonLocateSwitchSd";
            this.buttonLocateSwitchSd.Size = new System.Drawing.Size(75, 23);
            this.buttonLocateSwitchSd.TabIndex = 5;
            this.buttonLocateSwitchSd.Text = "Auto Find";
            this.buttonLocateSwitchSd.UseVisualStyleBackColor = true;
            this.buttonLocateSwitchSd.Click += new System.EventHandler(this.buttonLocateSwitchSd_Click);
            // 
            // textBoxDriveInfo
            // 
            this.textBoxDriveInfo.Enabled = false;
            this.textBoxDriveInfo.Location = new System.Drawing.Point(6, 46);
            this.textBoxDriveInfo.Multiline = true;
            this.textBoxDriveInfo.Name = "textBoxDriveInfo";
            this.textBoxDriveInfo.Size = new System.Drawing.Size(283, 109);
            this.textBoxDriveInfo.TabIndex = 4;
            // 
            // sdRootLabel
            // 
            this.sdRootLabel.AutoSize = true;
            this.sdRootLabel.Location = new System.Drawing.Point(6, 23);
            this.sdRootLabel.Name = "sdRootLabel";
            this.sdRootLabel.Size = new System.Drawing.Size(53, 13);
            this.sdRootLabel.TabIndex = 3;
            this.sdRootLabel.Text = "SD Drive:";
            // 
            // comboBoxDriveLetters
            // 
            this.comboBoxDriveLetters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDriveLetters.FormattingEnabled = true;
            this.comboBoxDriveLetters.Location = new System.Drawing.Point(65, 19);
            this.comboBoxDriveLetters.Name = "comboBoxDriveLetters";
            this.comboBoxDriveLetters.Size = new System.Drawing.Size(142, 21);
            this.comboBoxDriveLetters.TabIndex = 2;
            this.comboBoxDriveLetters.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriveLetters_SelectedIndexChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(313, 250);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(313, 221);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonAddFolder
            // 
            this.buttonAddFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddFolder.Location = new System.Drawing.Point(361, 113);
            this.buttonAddFolder.Name = "buttonAddFolder";
            this.buttonAddFolder.Size = new System.Drawing.Size(27, 28);
            this.buttonAddFolder.TabIndex = 5;
            this.buttonAddFolder.Text = "+";
            this.buttonAddFolder.UseVisualStyleBackColor = true;
            this.buttonAddFolder.Click += new System.EventHandler(this.buttonAddFolder_Click);
            // 
            // buttonRemoveFolder
            // 
            this.buttonRemoveFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonRemoveFolder.Location = new System.Drawing.Point(328, 113);
            this.buttonRemoveFolder.Name = "buttonRemoveFolder";
            this.buttonRemoveFolder.Size = new System.Drawing.Size(27, 28);
            this.buttonRemoveFolder.TabIndex = 6;
            this.buttonRemoveFolder.Text = "-";
            this.buttonRemoveFolder.UseVisualStyleBackColor = true;
            this.buttonRemoveFolder.Click += new System.EventHandler(this.buttonRemoveFolder_Click);
            // 
            // formFolderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 285);
            this.Controls.Add(this.buttonRemoveFolder);
            this.Controls.Add(this.buttonAddFolder);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBoxFolders);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formFolderList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Game Locations";
            this.Load += new System.EventHandler(this.formFolderList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxFolders;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label sdRootLabel;
        private System.Windows.Forms.ComboBox comboBoxDriveLetters;
        private System.Windows.Forms.TextBox textBoxDriveInfo;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonAddFolder;
        private System.Windows.Forms.Button buttonRemoveFolder;
        private System.Windows.Forms.Button buttonLocateSwitchSd;
    }
}