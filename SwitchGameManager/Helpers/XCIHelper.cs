using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static hacbuild.XCIManager;

namespace SwitchGameManager.Helpers
{
    public static class XciHelper
    {

        public enum XciLocation
        {
            PC,
            SD
        }

        private static hacbuild.XCI hac = new hacbuild.XCI();
        public static formMain formMain;
        public static bool isGameLoadingComplete = false;
        private static BackgroundWorker backgroundWorker;

        private static List<XciItem> xciOnPc = new List<XciItem>();
        private static List<XciItem> xciOnSd = new List<XciItem>();
        private static List<XciItem> xciCache;

        private static object pcListLock = new object();
        private static object sdListLock = new object();
        private static object cacheListLock = new object();


        public static void LoadXcisInBackground()
        {
            isGameLoadingComplete = false;
            if (backgroundWorker == null)
            {
                backgroundWorker = new BackgroundWorker();
                backgroundWorker.RunWorkerCompleted += delegate { };
                backgroundWorker.DoWork += delegate { LoadXcis(); };
            }
            if (!backgroundWorker.IsBusy)
            {
                formMain.olvList.ClearObjects();
                backgroundWorker.RunWorkerAsync();
            }
        }

        private static void LoadXcis()
        {
            bool updatedXciCache = false;

            formMain.UpdateToolStripLabel("Loading games..");
            formMain.olvList.EmptyListMsg = "Loading games..";

            Application.DoEvents();

            lock (pcListLock)
                xciOnPc = GetPcXcis();
            lock (sdListLock)
                xciOnSd = GetSdXcis();
            
            int progressCount = xciOnPc.Count + xciOnSd.Count;
            int progress = 0;

            if (xciOnPc.Count > 0)
                progressCount--;
            if (xciOnSd.Count > 0)
                progressCount--;

            formMain.SetupProgressBar(0, progressCount, progress);
            
            lock (pcListLock)
            {
                for (int i = 0; i < xciOnPc.Count; i++)
                {
                    formMain.UpdateToolStripLabel($"Processing [{progress}/{progressCount}] {Path.GetFileName(xciOnPc[i].xciFilePath)} ");

                    xciOnPc[i] = RefreshGame(xciOnPc[i]);

                    if (FindXciByIdentifer(xciOnPc[i].packageId, xciOnSd) != null)
                        xciOnPc[i].isGameOnSd = true;

                    xciOnPc[i].xciLocation = XciLocation.PC;

                    if (Settings.config.defaultView == 0)
                        formMain.olvList.AddObject(xciOnPc[i]);

                    if (UpdateXciCache(xciOnPc[i]))
                        updatedXciCache = true;

                    progress++;
                    formMain.UpdateProgressBar(progress);
                }
            }

            lock (sdListLock) {
                for (int i = 0; i < xciOnSd.Count; i++)
                {
                    progress++;
                    formMain.UpdateProgressBar(progress);

                    formMain.UpdateToolStripLabel($"Processing [{progress}/{progressCount}] {Path.GetFileName(xciOnSd[i].xciFilePath)}");

                    xciOnSd[i] = RefreshGame(xciOnSd[i]);

                    if (FindXciByIdentifer(xciOnSd[i].packageId, xciOnPc) != null)
                        xciOnSd[i].isGameOnPc = true;

                    xciOnSd[i].xciLocation = XciLocation.SD;

                    if (Settings.config.defaultView == 1)
                        formMain.olvList.AddObject(xciOnSd[i]);

                    if (UpdateXciCache(xciOnSd[i]))
                        updatedXciCache = true;
                }
            }

            if (updatedXciCache)
                SaveXciCache();

            formMain.HideProgressElements();
            formMain.UpdateToolStripLabel();

            isGameLoadingComplete = true;
        }

        /*
        internal static List<XciItem> CreateMasterXciList(List<XciItem> xciList, List<XciItem> xciOnSd)
        {
            List<XciItem> masterList = new List<XciItem>();
            XciItem xciTemp;

            //Update each XCI in case it's empty, which means we must call xciTemp = XciHelper.GetXciInfo(item)
            //Use AddObject to add the game to the list as it is loaded and processed
            //Maybe this shouldn't worry about that. Maybe call UpdateXci on each Xci in the list in LoadXcis

            for (int i = xciList.Count - 1; i >= 0; i--)
            {
                xciTemp = GetXciItemByPackageId(xciList[i].packageId, xciOnSd);
                if (xciTemp == null)
                {
                    xciList[i].isGameOnSd = false;
                    xciList[i].isGameOnPc = true;
                }
                else
                {
                    xciList[i].isGameOnPc = true;
                    xciList[i].isGameOnSd = true;
                    xciList[i].xciSdFilePath = xciTemp.xciSdFilePath;
                    xciOnSd.Remove(xciTemp);
                }
                masterList.Add(Clone(xciList[i]));
            }

            for (int i = xciOnSd.Count - 1; i >= 0; i--)
            {
                xciTemp = GetXciItemByPackageId(xciOnSd[i].packageId, masterList);
                if (xciTemp == null)
                {
                    xciOnSd[i].isGameOnSd = true;
                    xciOnSd[i].isGameOnPc = false;
                    //xciOnSd[i].xciSdFilePath = xciOnSd[i].xciFilePath;
                    xciOnSd[i].xciFilePath = string.Empty;
                    masterList.Add(xciOnSd[i]);
                }
                else
                {
                    masterList[i].isGameOnPc = true;
                    masterList[i].isGameOnSd = true;
                    xciOnSd[i].xciSdFilePath = xciOnSd[i].xciFilePath;
                    xciOnSd[i].xciFilePath = xciTemp.xciFilePath;
                }
            }

            return masterList;
        }
        */
        
        public static void UpdateXci(XciItem xci)
        {

            switch (xci.fileAction.action)
            {
                case FileHelper.FileAction.None:
                    break;
                case FileHelper.FileAction.Copy:
                    break;
                case FileHelper.FileAction.Move:
                    break;
                case FileHelper.FileAction.Delete:
                    break;
                case FileHelper.FileAction.Trim:
                    break;
                case FileHelper.FileAction.ShowCert:
                    break;
                case FileHelper.FileAction.ShowXciInfo:
                    break;
                default:
                    break;
            }

            //check if on sd/pc/file name changed, etc
            //then formMain.olvLocal.removeObject() if necesary
            // otherwise .refreshObject()
        }

        public static void RefreshList()
        {

            if (!XciHelper.isGameLoadingComplete)
                return;

            formMain.olvList.ClearObjects();

            switch (Settings.config.defaultView)
            {
                case 0: //PC game list
                    lock (pcListLock)
                        formMain.olvList.AddObjects(xciOnPc);

                    break;

                case 1: //SD game list
                    lock (sdListLock)
                        formMain.olvList.AddObjects(xciOnSd);

                    break;
            }

        }

        public static bool UpdateXciCache(XciItem xci)
        {
            lock (cacheListLock)
            {
                XciItem xciTemp = FindXciByIdentifer(xci.packageId);

                if (!IsXciInfoValid(xciTemp))
                {
                    if (xciTemp == null)
                    {
                        xciCache.Add(xci);
                        return true;
                    }
                }
            }
            return false;
        }

        public static List<XciItem> LoadGamesFromPath(string dirPath, bool recurse = true, bool isSdCard = false)
        {
            List<XciItem> pathXciList = new List<XciItem>();
            ulong packageId;
            XciItem xciTemp;

            List<string> xciFileList = FindAllFiles(dirPath, "*.xci", recurse);

            foreach (var item in xciFileList)
            {
                packageId = XciHelper.GetXciIdentifier(item);

                // Check if this game is in the Cache and Clone the cache XciItem to decouple the objects
                lock (cacheListLock)
                    xciTemp = Clone(XciHelper.FindXciByIdentifer(packageId)); 

                if (xciTemp == null)
                    xciTemp = new XciItem();

                xciTemp.xciFilePath = "";
                xciTemp.xciSdFilePath = "";

                xciTemp.isGameOnSd = isSdCard;
                xciTemp.isGameOnPc = !isSdCard;

                if (isSdCard)
                    xciTemp.xciSdFilePath = item;
                else
                    xciTemp.xciFilePath = item;


                pathXciList.Add(xciTemp);
            }

            return pathXciList;
        }

        public static XciItem RefreshGame(XciItem xci, bool force = false)
        {
            if (force || !IsXciInfoValid(xci))
            {
                if (xci.isGameOnPc)
                {
                    xci.isGameOnSd = false;
                    if (File.Exists(xci.xciFilePath))
                        xci = GetXciInfo(xci.xciFilePath);
                }
                if (xci.isGameOnSd)
                {
                    xci.isGameOnPc = false;
                    if (File.Exists(xci.xciSdFilePath))
                        xci = GetXciInfo(xci.xciSdFilePath);
                }
            }

            return xci;
        }

        public static List<XciItem> GetPcXcis()
        {
            List<XciItem> xciList = new List<XciItem>();

            foreach (string path in Settings.config.localXciFolders)
            {
                xciList.AddRange(XciHelper.LoadGamesFromPath(path, recurse: true, isSdCard: false));
            }

            //xciList = XciHelper.CreateMasterXciList(xciList, xciOnSd);

            return xciList;

        }

        public static List<XciItem> GetSdXcis()
        {
            List<XciItem> xciList = new List<XciItem>();

            if (Directory.Exists(Settings.config.sdDriveLetter))
            {
                // SD card games are currently only in the root directory (for SX OS)
                xciList = XciHelper.LoadGamesFromPath(Settings.config.sdDriveLetter, recurse: false, isSdCard: true);
            }

            return xciList;
        }

        public static bool IsXciInfoValid(XciItem xci)
        {
            if (xci == null)
                return false;

            if (string.IsNullOrWhiteSpace(xci.gameName))
                return false;

            if (string.IsNullOrWhiteSpace(xci.titleId))
                return false;

            return true;
        }

        public static List<string> FindAllFiles(string startDir, string filter, bool recurse = true)
        {
            List<string> files = new List<string>();

            if (recurse)
            {
                if (!Directory.Exists(startDir))
                    return files;

                foreach (var folder in Directory.GetDirectories(startDir))
                {
                    files.AddRange(FindAllFiles(folder, filter, recurse));
                }
            }

            files.AddRange(Directory.GetFiles(startDir, filter).ToList());

            return files;
        }

        public static T Clone<T>(T source)
        {
            try
            {
                var serialized = JsonConvert.SerializeObject(source);
                return JsonConvert.DeserializeObject<T>(serialized);
            }
            catch { return default(T); }
        }

        public static List<XciItem> LoadXciCache(string fileName = "")
        {
            List<XciItem> xciCache = new List<XciItem>();

            lock (cacheListLock)
            {
                if (String.IsNullOrWhiteSpace(fileName))
                    fileName = Settings.cacheFileName;

                if (!File.Exists(fileName))
                    return xciCache;

                xciCache = JsonConvert.DeserializeObject<IEnumerable<XciItem>>(File.ReadAllText(fileName)).ToList<XciItem>();
            }

            return xciCache;
        }

        internal static void RebuildCache()
        {
            lock (cacheListLock)
                xciCache = new List<XciItem>();

            File.Delete(Settings.cacheFileName);
            XciHelper.LoadXcisInBackground();
        }

        public static ulong GetXciIdentifier(string fileName)
        {
            if (!File.Exists(fileName))
                return 0;

            xci_header header = hac.GetXCIHeader(fileName);

            return header.PackageID;
        }

        public static XciItem GetXciInfo(string filePath)
        {
            XciItem xci = new XciItem(filePath);

            if (!File.Exists(filePath))
                return null;

            XCI_Explorer.MainForm mainForm = new XCI_Explorer.MainForm(false);
            
            xci_header header = hac.GetXCIHeader(xci.xciFilePath);

            xci.packageId = header.PackageID;

            mainForm.ReadXci(filePath);

            xci.gameName = mainForm.TB_Name.Text;
            xci.gameDeveloper = mainForm.TB_Dev.Text;
            xci.gameCardCapacity = mainForm.TB_Capacity.Text;
            xci.gameIcon = (Bitmap)mainForm.PB_GameIcon.BackgroundImage;
            xci.gameRevision = mainForm.TB_GameRev.Text;
            xci.masterKeyRevision = mainForm.TB_MKeyRev.Text;
            xci.sdkVersion = mainForm.TB_SDKVer.Text;
            xci.titleId = mainForm.TB_TID.Text;
            if (xci.titleId.Length != 16) xci.titleId = 0 + xci.titleId;
            xci.gameSize = mainForm.exactSize;
            xci.gameUsedSize = mainForm.exactUsedSpace;
            xci.productCode = mainForm.TB_ProdCode.Text;
            xci.gameCert = ReadXciCert(xci.xciFilePath);
            xci.xciFileSize = new System.IO.FileInfo(xci.xciFilePath).Length;

            // compare the expected size with the actual size
            xci.isXciTrimmed = (xci.gameSize == xci.gameUsedSize);

            // compare the first byte of the cert to the rest of the cert if they're all the same,
            // it's not unique. ex 255 for all
            xci.isUniqueCert = !xci.gameCert.All(s => s.Equals(xci.gameCert[0]));

            mainForm.Close();
            mainForm = null;

            return xci;
        }

        public static XciItem FindXciByIdentifer(ulong packageId, List<XciItem> xciCache = null)
        {
            if (xciCache == null)
            {
                if (XciHelper.xciCache == null)
                    XciHelper.xciCache = LoadXciCache();

                xciCache = XciHelper.xciCache;
            }

            XciItem xci;
            try
            {
                xci = xciCache.First(item => item.packageId == packageId);
            }
            catch (Exception ex)
            {
                return null;
            }
            return xci;
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

            // Adjust the format string to your preferences. For example "{0:0.#}{1}" would show a
            // single decimal place, and no space.
            return String.Format("{0:0.##} {1}", fileSize, sizes[order]);
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

        public static void SaveXciCache(string fileName = "", List<XciItem> xciCache = null)
        {
            if (String.IsNullOrWhiteSpace(fileName))
                fileName = Settings.cacheFileName;

            lock (cacheListLock)
            {
                if (xciCache == null)
                    xciCache = XciHelper.xciCache;

                if (xciCache == null || xciCache.Count == 0)
                    return;

                File.WriteAllText(fileName, JsonConvert.SerializeObject(xciCache, Formatting.Indented));
            }
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

        public static void ShowXciExplorer(string filePath)
        {
            XCI_Explorer.MainForm mainForm = new XCI_Explorer.MainForm(true);
            mainForm.ReadXci(filePath);
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
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}