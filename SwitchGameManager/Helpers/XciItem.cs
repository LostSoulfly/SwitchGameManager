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

        public string gameCardCapacity;

        public byte[] gameCert;

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

        [JsonIgnore]
        public FileHelper.FileStruct fileAction;

        public XciHelper.XciLocation xciLocation;

        public bool isUniqueCert;

        public bool isXciTrimmed;

        public string masterKeyRevision;

        public ulong packageId;

        public string productCode;

        public string sdkVersion;

        public string titleId;

        public string xciFilePath;

        public long xciFileSize;

        public string xciSdFilePath;

        //Can't serialize a Bitmap object.. so we convert it to and from a Byte array on the fly
        internal Bitmap gameIcon
        {
            get
            {
                Bitmap bmp;
                try
                {
                    using (var ms = new MemoryStream(this.gameIconBytes))
                    {
                        bmp = new Bitmap(ms);
                        return bmp;
                    }
                }
                catch { return new Bitmap(256, 256); }
            }
            set
            {
                using (var stream = new MemoryStream())
                {
                    value.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                    this.gameIconBytes = stream.ToArray();
                }
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