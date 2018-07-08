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
    public class Settings
    {

        public List<string> localXciFolders = new List<string>();
        public string sdDriveLetter;
        public byte[] olvState;
        public int olvIconSize;
        public bool confirmMultiActions;
        public bool encryptCertificates;

        [JsonIgnore]
        public SecureString encryptPassword;
        
        public static Settings LoadSettings(string filename)
        {
            throw new NotImplementedException();
        }

        public static List<string> DiscoverExternalStorage()
        {
            throw new NotImplementedException();
        }


        
    }
}
