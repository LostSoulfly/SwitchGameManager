using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SwitchGameManager.Helpers
{
    public static class FileHelper
    {
        private static CustomFileCopy customCopy;
        private static object lockObject = new object();
        private static int totalFiles = 0;
        private static int transferredFiles = 0;
        private static BackgroundWorker transferWorker;
        private static List<Tuple<string, string, bool>> xciTransfers = new List<Tuple<string, string, bool>>();

        public struct FileStruct
        {
            public FileAction action;
            public string destinationPath;
            public string sourcePath;
            public XciHelper.XciLocation source;
            public XciHelper.XciLocation destination;
        }

        public enum FileAction
        {
            None,
            Copy,
            Move,
            Delete,
            Trim,
            ShowCert,
            ShowXciInfo
        }

        //TODO
        //create a list of successful transfers and failed transfers
        //and link their xci object to them. Update their file information after transfers, then update their information.
        //also for trimming. TODO
        public static formMain formMain;
        public static bool isTransferInProgress;

        private static void CustomCopy_OnComplete(bool Canceled)
        {
        }

        private static void CustomCopy_OnProgressChanged(double Percentage, ref bool Cancel)
        {
            Cancel = transferWorker.CancellationPending;
            transferWorker.ReportProgress((int)Percentage);
        }

        private static void TransferWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //todo check if enough space on destination for transfer
            formMain.SetupProgressBar(0, 100, 0);

            Tuple<string, string, bool> action;

            totalFiles = xciTransfers.Count();
            transferredFiles = 0;

            while (xciTransfers.Count > 0)
            {
                lock (lockObject)
                    action = xciTransfers.First();

                customCopy = new CustomFileCopy(action.Item1, action.Item2);

                customCopy.OnProgressChanged += CustomCopy_OnProgressChanged;
                customCopy.OnComplete += CustomCopy_OnComplete;

                formMain.UpdateProgressLabel($"Copying {Path.GetFileName(action.Item1)} [{transferredFiles}/{totalFiles}]");

                customCopy.Copy();

                //if file is set to be moved, and we didn't cancel, delete the source file
                if (action.Item3 && !transferWorker.CancellationPending)
                    File.Delete(action.Item1);

                transferredFiles++;

                lock (lockObject)
                {
                    xciTransfers.Remove(action);
                }

                if (transferWorker.CancellationPending)
                {
                    File.Delete(action.Item2); //delete the destination if we cancelled early
                    e.Cancel = true;
                    return;
                }
            }
        }

        private static void TransferWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            formMain.UpdateProgressBar(e.ProgressPercentage);
        }

        private static void TransferWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //needs error reporting
            formMain.HideProgressElements();

            isTransferInProgress = false;

            if (transferWorker.CancellationPending)
                formMain.UpdateProgressLabel($"Transfer cancelled; [{transferredFiles}/{totalFiles}] transferred.");

            if (e.Cancelled)
                MessageBox.Show("File transfers were canceled.");
            else
                MessageBox.Show("All files transferred.");

            //refresh the xciList and OLV
            //XciHelper.LoadXcis();
        }

        public static void StopTransfers()
        {
            if (transferWorker.IsBusy)
                transferWorker.CancelAsync();
        }

        public static bool TransferXci(XciItem xci, bool moveXci = false, bool copyToPc = false, bool copyToSd = false)
        {
            if (transferWorker != null && !transferWorker.IsBusy && transferWorker.CancellationPending)
            {
                transferWorker.Dispose();
                transferWorker = null;
            }

            if (transferWorker == null)
            {
                transferWorker = new BackgroundWorker();
                transferWorker.DoWork += TransferWorker_DoWork;
                transferWorker.WorkerSupportsCancellation = true;
                transferWorker.WorkerReportsProgress = true;
                transferWorker.RunWorkerCompleted += TransferWorker_RunWorkerCompleted;
                transferWorker.ProgressChanged += TransferWorker_ProgressChanged;
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

            if (String.IsNullOrWhiteSpace(source) || String.IsNullOrWhiteSpace(destination))
                return false;

            if (File.Exists(destination))
            {
                if (MessageBox.Show($"{destination} already exists. Overwrite it?", "Overwrite Destination File", MessageBoxButtons.YesNo) != DialogResult.Yes)
                    return false;

                File.Delete(destination);
            }

            lock (lockObject)
            {
                xciTransfers.Add(new Tuple<string, string, bool>(source, destination, moveXci));
                totalFiles++;
                formMain.UpdateProgressLabel($"Copying {Path.GetFileName(xciTransfers.First().Item1)} [{transferredFiles}/{totalFiles}]");
            }

            if (!transferWorker.IsBusy)
            {
                isTransferInProgress = true;
                transferWorker.RunWorkerAsync();
            }

            return true;
        }
    }
}