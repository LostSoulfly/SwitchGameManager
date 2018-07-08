using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitchGameManager.Helpers
{
    static class FileHelpers
    {
        private static BackgroundWorker copyWorker = new BackgroundWorker();
        private static BackgroundWorker moveWorker = new BackgroundWorker();

        private static List<Tuple<string, string>> xciToCopy = new List<Tuple<string, string>>();
        private static List<Tuple<string, string>> xciToMove = new List<Tuple<string, string>>();
        private static object lockObject = new object();

        public static bool CopyXci(XciItem xci, bool copyToPc = false, bool copyToSd = false)
        {

            string source = string.Empty;
            string destination = string.Empty;

            if (copyToPc)
            {
                source = xci.xciSdFilePath;
                destination = Path.Combine(Settings.config.localXciFolders[0], Path.GetFileName(source));
            }

            if (copyToSd)
            {
                source = xci.xciFilePath;
                destination = Path.Combine(Settings.config.sdDriveLetter, Path.GetFileName(source));
            }

            if (source == destination)
                return false;

            if (copyToPc == copyToSd)
                return false;

            if (source.Length == 0 || destination.Length == 0)
                return false;

            lock (lockObject)
            {
                xciToCopy.Add(new Tuple<string, string>(source, destination));
            }

            return true;
        }


    }
}
