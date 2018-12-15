﻿using Be.Windows.Forms;
using System.IO;
using System.Windows.Forms;

namespace XCI_Explorer {
    public partial class CertForm : Form {
        public CertForm(MainForm mainForm) {
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
    }
}
