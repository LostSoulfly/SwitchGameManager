using Be.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace XCI_Explorer
{
	public class CertForm : Form
	{
		private IContainer components;

		private HexBox hbxHexView;

		public CertForm(MainForm mainForm)
		{
			InitializeComponent();
			FileStream fileStream = new FileStream(mainForm.TB_File.Text, FileMode.Open, FileAccess.Read);
			byte[] array = new byte[512];
			fileStream.Position = 28672L;
			fileStream.Read(array, 0, 512);
			hbxHexView.ByteProvider = new DynamicByteProvider(array);
			fileStream.Close();
		}

        public CertForm(byte[] cert, string gameName = "")
        {
            InitializeComponent();

            if (gameName.Length != 0)
                this.Text = $"{this.Text} - {gameName}";

            hbxHexView.ByteProvider = new DynamicByteProvider(cert);
        }

        public CertForm(string filePath, string gameName = "")
        {
            InitializeComponent();

            if (gameName.Length != 0)
                this.Text = $"{this.Text} - {gameName}";

            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[512];
            fileStream.Position = 28672L;
            fileStream.Read(array, 0, 512);
            hbxHexView.ByteProvider = new DynamicByteProvider(array);
            fileStream.Close();
        }

        protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.hbxHexView = new Be.Windows.Forms.HexBox();
            this.SuspendLayout();
            // 
            // hbxHexView
            // 
            this.hbxHexView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbxHexView.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbxHexView.Location = new System.Drawing.Point(0, 0);
            this.hbxHexView.Margin = new System.Windows.Forms.Padding(4);
            this.hbxHexView.Name = "hbxHexView";
            this.hbxHexView.ReadOnly = true;
            this.hbxHexView.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbxHexView.Size = new System.Drawing.Size(573, 256);
            this.hbxHexView.StringViewVisible = true;
            this.hbxHexView.TabIndex = 7;
            this.hbxHexView.UseFixedBytesPerLine = true;
            this.hbxHexView.VScrollBarVisible = true;
            this.hbxHexView.Click += new System.EventHandler(this.hbxHexView_Click);
            // 
            // CertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(573, 256);
            this.Controls.Add(this.hbxHexView);
            this.Name = "CertForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "cert data";
            this.ResumeLayout(false);

		}

        private void hbxHexView_Click(object sender, System.EventArgs e)
        {

        }
    }
}
