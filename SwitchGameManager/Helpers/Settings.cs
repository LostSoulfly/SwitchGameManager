using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows.Forms;

namespace SwitchGameManager.Helpers
{
    public class Config
    {
        [JsonIgnore]
        public string cacheFileName = "Cache.json";

        [JsonIgnore]
        public string configFileName = "Config.json";

        //public bool confirmMultiActions;
        public bool encryptCertificates;

        [JsonIgnore]
        public SecureString encryptPassword;

        public int formHeight = 475;
        public int formWidth = 975;
        public int listIconSize = 1;
        public List<string> localXciFolders = new List<string>();
        public byte[] olvState;
        public string sdDriveLetter = string.Empty;
        //public int listDisplayMode = 4;
    }

    public class Settings
    {
        public static Config config;
        public static List<XciItem> xciCache = new List<XciItem>();

        internal static void RebuildCache()
        {
            File.Delete(config.cacheFileName);
            XciHelper.formMain.PopulateXciList();
        }

        public static bool LoadSettings(string fileName = "")
        {
            Settings.config = new Config();

            if (fileName.Length == 0)
                fileName = config.configFileName;

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

        public static bool SaveSettings(string fileName = "")
        {
            try
            {
                if (fileName.Length == 0)
                    fileName = config.configFileName;

                File.WriteAllText(fileName, JsonConvert.SerializeObject(config, formatting: Formatting.Indented));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save {fileName}: {ex.Message}");
            }

            return false;
        }
    }
}