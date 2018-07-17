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
        private static List<XciItem> xciTransfers = new List<XciItem>();

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
            ShowXciInfo,
            ShowInExplorer,
            ShowRenameWindow
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

            XciItem xciAction;

            totalFiles = xciTransfers.Count();
            transferredFiles = 0;

            while (xciTransfers.Count > 0)
            {
                lock (lockObject)
                    xciAction = xciTransfers.First();

                customCopy = new CustomFileCopy(xciAction.fileAction.sourcePath, xciAction.fileAction.destinationPath);

                customCopy.OnProgressChanged += CustomCopy_OnProgressChanged;
                customCopy.OnComplete += CustomCopy_OnComplete;

                formMain.UpdateProgressLabel($"Copying {Path.GetFileName(xciAction.fileAction.sourcePath)} [{transferredFiles}/{totalFiles}]");

                customCopy.Copy();

                //if file is set to be moved, and we didn't cancel, delete the source file
                if (xciAction.fileAction.action == FileAction.Move && !transferWorker.CancellationPending)
                    File.Delete(xciAction.fileAction.sourcePath);

                transferredFiles++;

                lock (lockObject)
                    xciTransfers.Remove(xciAction);

                if (transferWorker.CancellationPending)
                {
                    File.Delete(xciAction.fileAction.destinationPath); //delete the destination if we cancelled early
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

        public static bool IsXciInTransferList(XciItem xci)
        {
            lock (lockObject)
            {
                if (xciTransfers.Contains(xci))
                    return true;
            }

            return false;
        }

        public static bool TransferXci(XciItem xci)
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

            if (xci.fileAction.destination == XciHelper.XciLocation.PC)
            {
                source = xci.xciSdFilePath;
                destination = Path.Combine(Settings.config.localXciFolders[0], Path.GetFileName(source));
            }
            else
            {
                source = xci.xciFilePath;
                destination = Path.Combine(Settings.config.sdDriveLetter, Path.GetFileName(source));
            }

            if (source == destination)
                return false;

            if (xci.fileAction.destination == xci.fileAction.source)
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
                xciTransfers.Add(xci);
                totalFiles++;
                formMain.UpdateProgressLabel($"Copying {Path.GetFileName(xciTransfers.First().fileAction.sourcePath)} [{transferredFiles}/{totalFiles}]");
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