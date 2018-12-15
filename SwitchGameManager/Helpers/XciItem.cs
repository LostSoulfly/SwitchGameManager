using Newtonsoft.Json;
using System.Drawing;
using System.IO;

namespace SwitchGameManager.Helpers
{
    public class XciItem
    {
        /*
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        */

        [JsonIgnore]
        public FileHelper.FileStruct fileAction;

        public string gameCardCapacity;

        public byte[] gameCertCompressed;

        [JsonIgnore]
        public byte[] gameCert
        {
            get
            {
                return XciHelper.Decompress(this.gameCertCompressed);
            }
            set
            {
                this.gameCertCompressed = XciHelper.Compress(value);
            }
        }

        public string gameDeveloper;

        public byte[] gameIconBytes;

        public string gameName;

        public string gameRevision;

        public double gameSize;

        public double gameUsedSize;

        public bool isCertEncrypted;

        [JsonIgnore]
        public bool isGameOnPc;

        [JsonIgnore]
        public bool isGameOnSd;

        public bool isUniqueCert;
        public bool isXciTrimmed;
        public string masterKeyRevision;
        //public ulong packageId;
        public string uniqueId;
        public string productCode;
        public string sdkVersion;
        public string titleId;
        public string xciFilePath;
        public long xciFileSize;
        public XciHelper.XciLocation xciLocation;
        //public string xciSdFilePath;

        //Can't serialize a Bitmap object.. so we convert it to and from a Byte array on the fly
        internal Bitmap gameIcon
        {
            get
            {
                Bitmap bmp;
                try
                {
                    using (var ms = new MemoryStream(XciHelper.Decompress(this.gameIconBytes)))
                    {
                        bmp = new Bitmap(ms);
                        return bmp;
                    }
                }
                catch { return new Bitmap(256, 256); }
            }
            set
            {
                try
                {
                    using (var stream = new MemoryStream())
                    {
                        value.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                        this.gameIconBytes = XciHelper.Compress(stream.ToArray());
                    }
                }
                catch { }
            }
        }

        public XciItem(string filePath)
        {
            this.xciFilePath = filePath;
        }

        public XciItem()
        {
        }
    }
}