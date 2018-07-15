using System;
using System.Windows.Forms;

namespace XCI_Explorer
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

        [STAThread]
        public static MainForm Startup(bool visible = true)
        {
            MainForm main = new MainForm(visible);
            Application.Run(main);
            return main;
        }
    }
}
