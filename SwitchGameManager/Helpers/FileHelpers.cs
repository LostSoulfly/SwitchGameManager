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

            if (!transferWorker.IsBusy && transferWorker.CancellationPending)
            {
                transferWorker.Dispose();
                transferWorker = null;
            }

            if (transferWorker == null)
            {
                transferWorker = new BackgroundWorker();
                transferWorker.DoWork += TransferWorker_DoWork;
                transferWorker.WorkerSupportsCancellation = true;
                transferWorker.RunWorkerCompleted += TransferWorker_RunWorkerCompleted;
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
        }
        
        private static void TransferWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //needs error reporting
            formMain.HideProgressElements();
            if (e.Cancelled)
                formMain.UpdateToolStripLabel("File transfers were canceled.");
            else
                formMain.UpdateToolStripLabel("All files transferred.");

            //refresh the xciList and OLV
            formMain.PopulateXciList();

        }

        private static void TransferWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //todo check if enough space on destination for transfer
            formMain.SetupProgressBar(0, 100, 0);
            while (xciTransfers.Count > 0)
            {
                Tuple<string, string, bool> action;
                int totalFiles = 0;
                int transferredFiles = 0;

                lock (lockObject)
                {
                    action = xciTransfers.First();
                    totalFiles = xciTransfers.Count();
                }
                customCopy = new CustomFileCopy(action.Item1, action.Item2);

                customCopy.OnProgressChanged += CustomCopy_OnProgressChanged;
                customCopy.OnComplete += CustomCopy_OnComplete;

                formMain.UpdateProgressLabel($"Copying {Path.GetFileName(action.Item1)} [{transferredFiles}/{totalFiles}]");

                customCopy.Copy();

                //if file is set to be moved, and we didn't cancel, delete the source file
                if (action.Item3 && !transferWorker.CancellationPending)
                    File.Delete(action.Item1);

                transferredFiles++;
                                
                if (transferWorker.CancellationPending)
                    break;
            }
        }

        private static void CustomCopy_OnComplete(bool Canceled)
        {
            
        }

        private static void CustomCopy_OnProgressChanged(double Percentage, ref bool Cancel)
        {
            Cancel = transferWorker.CancellationPending;
            formMain.UpdateProgressBar((int)Percentage);
        }
    }
}
