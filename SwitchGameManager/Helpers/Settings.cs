using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SwitchGameManager.Helpers
{

    public class Config
    {
        public List<string> localXciFolders = new List<string>();
        public string sdDriveLetter = string.Empty;
        public byte[] olvState;
        public int olvIconSize;
        public bool confirmMultiActions;
        public bool encryptCertificates;
        public int formHeight;
        public int formWidth;

        [JsonIgnore]
        public SecureString encryptPassword;
    }

    public class Settings
    {

        public static Config config;

        
        public static bool LoadSettings(string fileName)
        {
            Settings.config = new Config();

            try
            {
                config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(fileName));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to load {fileName}: {ex.Message}");
            }

            return false;
        }

        public static bool SaveSettings(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(config, formatting: Formatting.Indented));
                return true;
            } catch (Exception ex)
            {
                MessageBox.Show($"Unable to save {fileName}: {ex.Message}");
            }

            return false;
        }

    }
}
