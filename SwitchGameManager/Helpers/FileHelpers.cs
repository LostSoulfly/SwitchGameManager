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
        private static BackgroundWorker transferWorker;

        private static CustomFileCopy customCopy;

        private static List<Tuple<string, string, bool>> xciTransfers = new List<Tuple<string, string, bool>>();
        private static object lockObject = new object();

        public static formMain formMain;

        public static bool TransferXci(XciItem xci, bool moveXci = false, bool copyToPc = false, bool copyToSd = false)
        {
            if (transferWorker == null)
            {
                transferWorker = new BackgroundWorker();
                transferWorker.DoWork += CopyWorker_DoWork;
                transferWorker.WorkerSupportsCancellation = true;
                transferWorker.RunWorkerCompleted += CopyWorker_RunWorkerCompleted;
                transferWorker.ProgressChanged += CopyWorker_ProgressChanged;
            }

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
                xciTransfers.Add(new Tuple<string, string, bool>(source, destination, moveXci));
            }

            if (!transferWorker.IsBusy)
            {
                transferWorker.RunWorkerAsync();
            }

            return true;
        }

        public static void StopTransfers()
        {
            if (transferWorker.IsBusy)
                transferWorker.CancelAsync();
            /*
            if (moveWorker.IsBusy)
                moveWorker.CancelAsync();
            */
        }

        private static void CopyWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CopyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private static void CopyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (xciTransfers.Count > 0)
            {
                Tuple<string, string, bool> action;

                lock (lockObject)
                {
                    action = xciTransfers.First();
                }
                customCopy = new CustomFileCopy(action.Item1, action.Item2);

                customCopy.OnProgressChanged += CustomCopy_OnProgressChanged;
                customCopy.OnComplete += CustomCopy_OnComplete;

                customCopy.Copy();

                //File.Copy(action.Item1, action.Item2);
                
                if (transferWorker.CancellationPending)
                {
                    break;
                }
            }
        }

        private static void CustomCopy_OnComplete(bool Canceled)
        {
            
        }

        private static void CustomCopy_OnProgressChanged(double Percentage, ref bool Cancel)
        {
            Cancel = transferWorker.CancellationPending;
            formMain.UpdateToolStripLabel("Progress: " + Percentage);
        }
    }
}
