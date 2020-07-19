using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static hacbuild.XCIManager;

namespace hacbuild
{
    public class XCI
    {
        public byte[] NcaHeaderEncryptionKey1_Prod;
        public byte[] NcaHeaderEncryptionKey2_Prod;
        public string Mkey;
        public static Random Rand = new Random();
        public static SHA256 SHA256 = SHA256CryptoServiceProvider.Create();
        public static Aes AES128CBC = Aes.Create();


        public XCI()
        {
            
            if (LoadKeys())
            {
                Console.WriteLine("XCI Header Key loaded successfully:\n{0}", BitConverter.ToString(XCIManager.XCI_GAMECARDINFO_KEY));

                byte[] keyHash = SHA256.ComputeHash(XCIManager.XCI_GAMECARDINFO_KEY);

                if (Enumerable.SequenceEqual<byte>(keyHash, XCIManager.XCI_GAMECARD_KEY_SHA256))
                {
                    Console.WriteLine("XCI Header Key is correct!");
                }
                else
                {
                    Console.WriteLine("[WARN] Invalid XCI Header Key");
                }

            }
            else
            {
                MessageBox.Show("[WARN] Could not load xci_header_key from keys.txt");
            }

            AES128CBC.BlockSize = 128;
            AES128CBC.Mode = CipherMode.CBC;
            AES128CBC.Padding = PaddingMode.Zeros;

        }

        public xci_header GetXCIHeader(string fileName)
        {
            if (!File.Exists(fileName))
                return new xci_header();

            XCIManager.xci_header header = XCIManager.GetXCIHeader(fileName);

            return header;
        }

        public gamecard_info GetGamecardInfo(xci_header header)
        {
            XCIManager.gamecard_info gcInfo = XCIManager.DecryptGamecardInfo(header, AES128CBC);

            return gcInfo;
        }

        private bool LoadKeys()
        {
            bool ret = false;
            try
            {
                StreamReader file = new StreamReader("keys.txt");

                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length < 2) continue;

                    string name = parts[0].Trim(" \0\n\r\t".ToCharArray());
                    string key = parts[1].Trim(" \0\n\r\t".ToCharArray());

                    //Console.WriteLine("{0} = {1}", name, key);

                    if (name == "xci_header_key")
                    {
                        XCIManager.XCI_GAMECARDINFO_KEY = Utils.StringToByteArray(key);
                        ret = true;
                    }
                    /*
                     * NCAHeader has TitleID/SDKVersion/MasterKeyRevision
                    if (name == "header_key")
                    {
                        NcaHeaderEncryptionKey1_Prod = Utils.StringToByteArray(key.Remove(32, 32));
                        NcaHeaderEncryptionKey2_Prod = Utils.StringToByteArray(key.Remove(0, 32));
                    }
                    */
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERR] keys.txt is either missing or unaccessible.");
                ret = false;
            }
            return ret;

        }
    }
}
