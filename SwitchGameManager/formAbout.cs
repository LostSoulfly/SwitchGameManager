using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SwitchGameManager
{
    public partial class formAbout : Form
    {
        public formAbout()
        {
            InitializeComponent();
            richTextBoxAbout.LinkClicked += RichTextBoxAbout_LinkClicked;
        }

        private void RichTextBoxAbout_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            LaunchWeblink(e.LinkText);
        }

        private void LaunchWeblink(string url)
        {
            if (IsHttpURL(url)) Process.Start(url);
        }

        private bool IsHttpURL(string url)
        {
            return
                ((!string.IsNullOrWhiteSpace(url)) &&
                (url.ToLower().StartsWith("http", StringComparison.Ordinal)));
        }

        private void formAbout_Load(object sender, EventArgs e)
        {
            string about = string.Empty;

            about = "SwitchGameManager " + Application.ProductVersion + Environment.NewLine;

            about += @"Programmed by LostSoulfly

Programmed off and on in under two weeks for fun. If you find any issues, please report them on GitHub!

Project Repository
https://github.com/LostSoulfly/SwitchGameManager


Credits
http://objectlistview.sourceforge.net/cs/index.html
https://stackoverflow.com/a/6055385
https://github.com/LucaFraga/hacbuild
https://github.com/StudentBlake/XCI-Explorer
https://www.nuget.org/packages/Be.Windows.Forms.HexBox/
Newtonsoft.Json
https://www.iconfinder.com/icons/3151574/game_nintendo_switch_video_icon
Lots of people on StackExchange/Overflow

And probably a few I'm missing.


Donations
BTC: 1QDVJmxyqMzA5nQghKMBCFVk8K41nSoz5b
ETH: 0xa62a11710cc44Bd54D66CbCcF710a36716BF04CE
Monero: 43tVLRGvcaadfw4HrkUcpEKmZd9Y841rGKvsLZW8XvEVSBX1GrGezWvQYDdoNwNHAwTqSyK7iqyyqMSpDoUVKQmM43nzT72
UBQ: 0x0c0ff71b06413865fe9fE9a4C40396c136a62980
DCR: DsfPh3tpa7nd8sExYvxWbijzjUH1zJ34dgu
HUSH: t1ZHrvmtgd3129iYEcFm21XMv5ojdh2xmsf
ZEN: znTmG8nid2LEYgw8cub17Q7briGATan4c68
";

            richTextBoxAbout.Text += about;
        }
    }
}
