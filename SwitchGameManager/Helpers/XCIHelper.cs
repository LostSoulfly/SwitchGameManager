using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static hacbuild.XCIManager;
using Newtonsoft.Json;

namespace SwitchGameManager.Helpers
{
    class XciHelper
    {

        private static hacbuild.XCI hac = new hacbuild.XCI();

        public static List<XciItem> LoadXciCache(string fileName, bool localGames = true)
        {
            List<XciItem> xciCache = new List<XciItem>();

            if (!File.Exists(fileName))
                return xciCache;

            xciCache = JsonConvert.DeserializeObject<IEnumerable<XciItem>>(File.ReadAllText(fileName)).ToList<XciItem>();

            //Verify that the files exist, otherwise remove them from the cache
            for (int index = xciCache.Count - 1; index >= 0; index--)
            {
                if (!File.Exists(xciCache[index].xciFilePath))
                    xciCache.RemoveAt(index);
                else
                    xciCache[index].isGameOnPc = true;
            }

            return xciCache;
        }

        public static void SaveXciCache(string fileName, List<XciItem> xciCache)
        {
            File.WriteAllText(fileName, JsonConvert.SerializeObject(xciCache, Formatting.Indented));
        }

        public static XciItem GetXciItemFromCache(ulong packageId, List<XciItem> xciCache)
        {
            XciItem xci;
            try
            {
                xci = xciCache.First(item => item.packageId == packageId);
            }
            catch (Exception ex)
            {
                //Log.Error("GetMiner", ex);
                return null;
            }
            return xci;
        }

        public static XciItem GetXciInfo(string filePath)
        {
            XciItem xci = new XciItem(filePath);

            if (!File.Exists(filePath))
                return null;

            XCI_Explorer.MainForm mainForm = new XCI_Explorer.MainForm(false);

            mainForm.ReadXci(filePath);
            
            xci.gameName = mainForm.TB_Name.Text;
            xci.gameDeveloper = mainForm.TB_Dev.Text;
            xci.gameCardCapacity = mainForm.TB_Capacity.Text;
            xci.gameIcon = (Bitmap)mainForm.PB_GameIcon.BackgroundImage;
            xci.gameRevision = mainForm.TB_GameRev.Text;
            xci.masterKeyRevision = mainForm.TB_MKeyRev.Text;
            xci.sdkVersion = mainForm.TB_SDKVer.Text;
            xci.titleId = mainForm.TB_TID.Text;
            xci.gameSize = mainForm.exactSize;
            xci.gameUsedSize = mainForm.exactUsedSpace;
            xci.productCode = mainForm.TB_ProdCode.Text;
            xci.gameCert = ReadXciCert(xci.xciFilePath);
            xci.xciFileSize = new System.IO.FileInfo(xci.xciFilePath).Length;

            // compare the expected size with the actual size
            xci.isXciTrimmed = (xci.gameSize == xci.gameUsedSize);

            // compare the first byte of the cert to the rest of the cert
            // if they're all the same, it's not unique. ex 255 for all
            xci.isUniqueCert = !xci.gameCert.All(s => s.Equals(xci.gameCert[0]));

            mainForm.Close();
            mainForm = null;

            //read header to get PackageID
            xci_header header = hac.GetXCIHeader(xci.xciFilePath);

            xci.packageId = header.PackageID;

            return xci;
        }

        public static ulong GetPackageID(string fileName)
        {

            if (!File.Exists(fileName))
                return 0;

            xci_header header = hac.GetXCIHeader(fileName);

            return header.PackageID;
        }

        public static void ShowXciExplorer(string filePath)
        {
            XCI_Explorer.MainForm mainForm = new XCI_Explorer.MainForm(true);
            mainForm.ReadXci(filePath);
        }

        public static byte[] ReadXciCert(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] array = new byte[512];
            fileStream.Position = 28672L;
            fileStream.Read(array, 0, 512);
            fileStream.Close();

            return array;
        }

        public static void ShowXciCert(XciItem xci)
        {
            XCI_Explorer.CertForm certForm;

            if (xci.gameCert == null)
                certForm = new XCI_Explorer.CertForm(xci.gameCert, xci.gameName);
            else
                certForm = new XCI_Explorer.CertForm(xci.xciFilePath, xci.gameName);

            certForm.Show();
        }

        public static bool TrimXci(XciItem xci)
        {
            if (!File.Exists(xci.xciFilePath))
                return false;

            //maybe check this for errors? Maybe copy the file first, then do it on the copy? Needs tested.
            try
            {
                FileStream fileStream = new FileStream(xci.xciFilePath, FileMode.Open, FileAccess.Write);
                fileStream.SetLength((long)xci.gameUsedSize);
                fileStream.Close();
            } catch
            {
                return false;
            }

            return true;
        }

        public static string ReadableFileSize(double fileSize)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (fileSize >= 1024 && order < sizes.Length - 1)
            {
                order++;
                fileSize = fileSize / 1024;
            }

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would
            // show a single decimal place, and no space.
            return String.Format("{0:0.##} {1}", fileSize, sizes[order]);
        }
    }
}
