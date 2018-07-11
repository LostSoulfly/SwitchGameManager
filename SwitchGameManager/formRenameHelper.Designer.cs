namespace SwitchGameManager
{
    partial class formRenameHelper
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
            this.listSelect = new System.Windows.Forms.ListBox();
            this.labelFilter = new System.Windows.Forms.Label();
            this.labelDesc = new System.Windows.Forms.Label();
            this.buttonAddFilter = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listSelect
            // 
            this.listSelect.FormattingEnabled = true;
            this.listSelect.Location = new System.Drawing.Point(12, 12);
            this.listSelect.Name = "listSelect";
            this.listSelect.Size = new System.Drawing.Size(112, 147);
            this.listSelect.TabIndex = 0;
            this.listSelect.SelectedIndexChanged += new System.EventHandler(this.listSelect_SelectedIndexChanged);
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFilter.Location = new System.Drawing.Point(141, 12);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(51, 20);
            this.labelFilter.TabIndex = 1;
            this.labelFilter.Text = "label1";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(142, 43);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(186, 73);
            this.labelDesc.TabIndex = 2;
            this.labelDesc.Text = "label2";
            // 
            // buttonAddFilter
            // 
            this.buttonAddFilter.Location = new System.Drawing.Point(197, 136);
            this.buttonAddFilter.Name = "buttonAddFilter";
            this.buttonAddFilter.Size = new System.Drawing.Size(131, 23);
            this.buttonAddFilter.TabIndex = 3;
            this.buttonAddFilter.Text = "Add To Rename Filter";
            this.buttonAddFilter.UseVisualStyleBackColor = true;
            this.buttonAddFilter.Click += new System.EventHandler(this.buttonAddFilter_Click);
            // 
            // formRenameHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 171);
            this.Controls.Add(this.buttonAddFilter);
            this.Controls.Add(this.labelDesc);
            this.Controls.Add(this.labelFilter);
            this.Controls.Add(this.listSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "formRenameHelper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rename Helper";
            this.Load += new System.EventHandler(this.formRenameHelper_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listSelect;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label labelDesc;
        private System.Windows.Forms.Button buttonAddFilter;
    }
}