using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SwitchGameManager.Helpers
{
    public class XciItem
    {
        public XciItem(string filePath)
        {
            this.xciFilePath = filePath;
            
        } 

        /*
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        */

        internal Bitmap gameIcon
        {
            get
            {
                Bitmap bmp;
                using (var ms = new MemoryStream(this.gameIconBytes))
                {
                    bmp = new Bitmap(ms);
                    return bmp;
                }
            }
            set
            {
                using (var stream = new MemoryStream())
                {
                    value.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    this.gameIconBytes = stream.ToArray();
                }
            }
        }
        public byte[] gameIconBytes;
        public string gameName;
        public string gameDeveloper;
        public string gameRevision;
        public string masterKeyRevision;
        public string sdkVersion;
        public string gameCardCapacity;
        public string titleId;
        public double gameSize;
        public double gameUsedSize;
        public long xciFileSize;
        public string productCode;
        public byte[] gameCert;
        public bool isXciTrimmed;
        public string xciFilePath;
        public ulong packageId;
        public bool isUniqueCert;
        [JsonIgnore]
        public bool isGameOnSd;
        [JsonIgnore]
        public bool isGameOnPc;
    }
}
