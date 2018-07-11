using BrightIdeasSoftware;
using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchGameManager
{
    public partial class formRenamer : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

        List<RenameItem> renameItems = new List<RenameItem>();

        public formRenamer()
        {
            InitializeComponent();
        }

        private void formRenamer_Load(object sender, EventArgs e)
        {
            columnError.IsVisible = false;
            SendMessage(textBoxFilter.Handle, 0x1501, 1, "Rename Filter Ex. {name} {titleid} (Click Info for details -->)");
            SendMessage(textBoxSearch.Handle, 0x1501, 1, "Search List..");

            columnPath.AspectToStringConverter = delegate (object row)
            {
                string path = (string)row;

                return ProcessPathAspect(path);
            };

            columnRenamed.AspectToStringConverter = delegate (object row)
            {
                string path = (string)row;

                return ProcessPathAspect(path);
            };

            columnError.AspectGetter = delegate (object row)
            {
                RenameItem item = (RenameItem)row;

                if (item.renameException == null)
                    return "";

                return item.renameException.Message;
            };
            olvRenameList.RebuildColumns();
        }

        private string ProcessPathAspect(string path)
        {
            if (checkBoxExtensions.Checked && checkBoxShowPath.Checked)
                return path;

            string file = Path.GetFileName(path);
            string fileWithoutExtension = Path.GetFileNameWithoutExtension(path);
            string filePath = Path.GetDirectoryName(path);
            string output = string.Empty;

            if (checkBoxExtensions.Checked)
                output = file;
            else
                output = fileWithoutExtension;

            if (checkBoxShowPath.Checked)
                output = Path.Combine(filePath, output);

            return output;
        }

        public void PopulateList(List<XciItem> xciList)
        {
            foreach (XciItem item in xciList)
            {
                if (File.Exists(item.xciFilePath))
                    renameItems.Add(new RenameItem(item.xciFilePath, item.xciFilePath, item));
                if (File.Exists(item.xciSdFilePath))
                    renameItems.Add(new RenameItem(item.xciSdFilePath, item.xciSdFilePath, item));
            }

            olvRenameList.SetObjects(renameItems);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            TextMatchFilter search = TextMatchFilter.Contains(olvRenameList, textBoxSearch.Text);
            olvRenameList.AdditionalFilter = search;
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            ProcessNameFilter(textBoxFilter.Text);
        }

        public void ProcessNameFilter(string text)
        {

            List<RenameItem> filtered = olvRenameList.FilteredObjects.Cast<RenameItem>().ToList();

            foreach (RenameItem item in filtered)
            {
                item.renamePath = Path.Combine(Path.GetDirectoryName(item.filePath), GetRenamedFile(text, item.xci));
                olvRenameList.UpdateObject(item);
            }
        }

        private string GetRenamedFile(string renameFilter, XciItem xci)
        {
            string renamed = string.Empty;

            renamed = renameFilter;
            renamed = renamed.Replace("{name}", xci.gameName);
            renamed = renamed.Replace("{titleid}", xci.titleId);
            renamed = renamed.Replace("{tid}", xci.titleId);
            renamed = renamed.Replace("{productcode}", xci.productCode);
            renamed = renamed.Replace("{prodcode}", xci.productCode);
            renamed = renamed.Replace("{size}", Helpers.XciHelper.ReadableFileSize(xci.gameSize));
            renamed = renamed.Replace("{developer}", xci.gameDeveloper);
            renamed = renamed.Replace("{dev}", xci.gameDeveloper);
            renamed = renamed.Replace("{trimmed}", xci.isXciTrimmed ? "trimmed" : "");
            renamed = renamed.Replace("{uniquecert}", xci.isUniqueCert ? "unique" : "");
            renamed = renamed.Replace("{cert}", xci.isUniqueCert ? "unique" : "");
            renamed = renamed.Replace("{revision}", xci.gameRevision);
            renamed = renamed.Replace("{gamerev}", xci.gameRevision);
            renamed = renamed.Replace("{rev}", xci.gameRevision);
            renamed = renamed.Replace("{masterkey}", xci.masterKeyRevision);
            renamed = renamed.Replace("{keyrev}", xci.masterKeyRevision);
            renamed = renamed.Replace("{keyrevision}", xci.masterKeyRevision);

            return RemoveIllegalCharacters(renamed) + ".xci";

        }

        private string RemoveIllegalCharacters(string fileName)
        {
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

            foreach (char c in invalid)
            {
                fileName = fileName.Replace(c.ToString(), "");
            }

            return fileName;
        }

        private void checkBoxExtensions_CheckedChanged(object sender, EventArgs e)
        {
            olvRenameList.UpdateObjects(renameItems);
        }

        private void checkBoxShowPath_CheckedChanged(object sender, EventArgs e)
        { 
            olvRenameList.UpdateObjects(renameItems);
        }

        private void buttonInfo_Click(object sender, EventArgs e)
        {
            formRenameHelper helper = new formRenameHelper();
            XciItem xciItem;
            try {
                xciItem = renameItems.First().xci;
            } catch
            { 
                xciItem = new XciItem(@"C:\FAKEPATH\000 - Mario Meets Jesus [trimmed] (EnFrDeLol).xci");
                xciItem.gameName = "Mario Meets Jesus";
                xciItem.gameDeveloper = "Not Nintendo";
                xciItem.isXciTrimmed = true;
                xciItem.productCode = "ABCDEF";
                xciItem.titleId = "0123456789123456";
                xciItem.masterKeyRevision = "MasterKey 9(12.0.0.0)";
                xciItem.isUniqueCert = true;
                xciItem.gameSize = 123456789;
            }
            helper.SetupHelper(xciItem, this);
            helper.Show();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Dispose();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            buttonRename.Enabled = false;

            List<RenameItem> filtered = olvRenameList.FilteredObjects.Cast<RenameItem>().ToList();

            for (int i = filtered.Count - 1; i >= 0; i--)
            {
                if (TryRename(filtered[i]))
                {
                    olvRenameList.RemoveObject(filtered[i]);
                }
                else
                {
                    columnError.IsVisible = true;
                    olvRenameList.UpdateObject(filtered[i]);
                }
            }

            olvRenameList.RebuildColumns();
            buttonRename.Enabled = true;
            XciHelper.formMain.PopulateXciList();
        }

        private bool TryRename(RenameItem item)
        {
            try
            {
                File.Move(item.filePath, item.renamePath);
                //TODO verify file was moved
                return true;
            } catch (Exception ex)
            {
                item.renameException = ex;
                return false;
            }
        }

    }

    public class RenameItem
    {
        public string filePath;
        public string renamePath;
        public XciItem xci;
        public Exception renameException;

        public RenameItem(string filePath, string renamePath, XciItem xci)
        {
            this.filePath = filePath;
            this.renamePath = renamePath;
            this.xci = xci;
        }
    }
}
