using SwitchGameManager.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchGameManager
{
    public partial class formRenameHelper : Form
    {

        private XciItem xci;
        private formRenamer formRenamer;
        private Dictionary<string, string> exampleDict = new Dictionary<string, string>();

        public formRenameHelper()
        {
            InitializeComponent();
        }

        public void SetupHelper(XciItem xci, formRenamer formRenamer)
        {
            this.formRenamer = formRenamer;
            this.xci = xci;
            AddListBoxItem("{name}", xci.gameName);
            AddListBoxItem("{titleid}", xci.titleId);
            AddListBoxItem("{tid}", xci.titleId);
            AddListBoxItem("{productcode}", xci.productCode);
            AddListBoxItem("{prodcode}", xci.productCode);
            AddListBoxItem("{size}", Helpers.XciHelper.ReadableFileSize(xci.gameSize));
            AddListBoxItem("{developer}", xci.gameDeveloper);
            AddListBoxItem("{dev}", xci.gameDeveloper);
            AddListBoxItem("{trimmed}", xci.isXciTrimmed ? "trimmed" : "");
            AddListBoxItem("{uniquecert}", xci.isUniqueCert ? "unique" : "");
            AddListBoxItem("{cert}", xci.isUniqueCert ? "unique" : "");
            AddListBoxItem("{revision}", xci.gameRevision);
            AddListBoxItem("{gamerev}", xci.gameRevision);
            AddListBoxItem("{rev}", xci.gameRevision);
            AddListBoxItem("{masterkey}", xci.masterKeyRevision);
            AddListBoxItem("{keyrev}", xci.masterKeyRevision);
            AddListBoxItem("{keyrevision}", xci.masterKeyRevision);
            listSelect.SelectedIndex = 0;
        }

        private void formRenameHelper_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }

        private void AddListBoxItem(string name, string example)
        {
            listSelect.Items.Add(name);
            exampleDict.Add(name, example);
        }

        private void listSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            labelFilter.Text = listSelect.Text;
            labelDesc.Text = exampleDict[listSelect.Text];
        }

        private void buttonAddFilter_Click(object sender, EventArgs e)
        {
            if (formRenamer.textBoxFilter.Text.Length >= 0) formRenamer.textBoxFilter.Text += " "; 
            formRenamer.textBoxFilter.Text += listSelect.Text;
            formRenamer.ProcessNameFilter(formRenamer.textBoxFilter.Text);
        }
    }
}
