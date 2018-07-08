using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SwitchGameManager.Helpers
{

    public class Config
    {
        public List<string> localXciFolders = new List<string>();
        public string sdDriveLetter;
        public byte[] olvState;
        public int olvIconSize;
        public bool confirmMultiActions;
        public bool encryptCertificates;

        [JsonIgnore]
        public SecureString encryptPassword;
    }

    public class Settings
    {

        public static Config config;

        
        public static void LoadSettings(string filename)
        {
            Settings.config = new Config();

            config.confirmMultiActions = true;
            config.encryptCertificates = false;
            //config.encryptPassword = "";
            config.localXciFolders.Add(@"F:\Games\Emulation\Switch\Games");
            config.localXciFolders.Add(@"F:\Games\Emulation\Switch\MINE");
            config.sdDriveLetter = @"E:\";
        }

        public static List<string> DiscoverExternalStorage()
        {
            throw new NotImplementedException();
        }

    }
}
