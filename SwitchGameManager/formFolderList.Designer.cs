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
            this.listBoxFolders = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxDriveLetters = new System.Windows.Forms.ComboBox();
            this.sdRootLabel = new System.Windows.Forms.Label();
            this.textBoxDriveInfo = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxFolders
            // 
            this.listBoxFolders.FormattingEnabled = true;
            this.listBoxFolders.Location = new System.Drawing.Point(12, 12);
            this.listBoxFolders.Name = "listBoxFolders";
            this.listBoxFolders.Size = new System.Drawing.Size(352, 95);
            this.listBoxFolders.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxDriveInfo);
            this.groupBox1.Controls.Add(this.sdRootLabel);
            this.groupBox1.Controls.Add(this.comboBoxDriveLetters);
            this.groupBox1.Location = new System.Drawing.Point(12, 113);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 176);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SD Card";
            // 
            // comboBoxDriveLetters
            // 
            this.comboBoxDriveLetters.FormattingEnabled = true;
            this.comboBoxDriveLetters.Location = new System.Drawing.Point(65, 19);
            this.comboBoxDriveLetters.Name = "comboBoxDriveLetters";
            this.comboBoxDriveLetters.Size = new System.Drawing.Size(189, 21);
            this.comboBoxDriveLetters.TabIndex = 2;
            this.comboBoxDriveLetters.SelectedIndexChanged += new System.EventHandler(this.comboBoxDriveLetters_SelectedIndexChanged);
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
            // textBoxDriveInfo
            // 
            this.textBoxDriveInfo.Location = new System.Drawing.Point(6, 46);
            this.textBoxDriveInfo.Multiline = true;
            this.textBoxDriveInfo.Name = "textBoxDriveInfo";
            this.textBoxDriveInfo.Size = new System.Drawing.Size(248, 119);
            this.textBoxDriveInfo.TabIndex = 4;
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(289, 262);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(289, 233);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(337, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 28);
            this.button1.TabIndex = 5;
            this.button1.Text = "+";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(304, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(27, 28);
            this.button2.TabIndex = 6;
            this.button2.Text = "-";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // formFolderList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 297);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBoxFolders);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formFolderList";
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
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}