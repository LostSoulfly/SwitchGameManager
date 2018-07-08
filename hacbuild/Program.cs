
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace hacbuild
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.WriteLine("HACbuild - {0}", Assembly.GetExecutingAssembly().GetName().Version);

            // TODO Decent command line argument parsing (this is... ugly).
            /*
            switch (args[0])
            {
                
                case "hfs0":
                    HFS0Manager.BuildHFS0(args[1], args[2]);
                    break;
                case "xci":
                    XCIManager.BuildXCI(args[1], args[2]);
                    break;
                case "xci_auto":
                    string inPath = Path.Combine(Environment.CurrentDirectory,  args[1]);
                    string outPath = Path.Combine(Environment.CurrentDirectory, args[2]);
                    string tmpPath = Path.Combine(inPath, "root_tmp");
                    Directory.CreateDirectory(tmpPath);

                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "secure"), Path.Combine(tmpPath, "secure"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "normal"), Path.Combine(tmpPath, "normal"));
                    HFS0Manager.BuildHFS0(Path.Combine(inPath, "update"), Path.Combine(tmpPath, "update"));
                    if(Directory.Exists(Path.Combine(inPath, "logo")))
                        HFS0Manager.BuildHFS0(Path.Combine(inPath, "logo"), Path.Combine(tmpPath, "logo"));
                    HFS0Manager.BuildHFS0(tmpPath, Path.Combine(inPath, "root.hfs0"));

                    XCIManager.BuildXCI(inPath, outPath);

                    File.Delete(Path.Combine(inPath, "root.hfs0"));
                    Directory.Delete(tmpPath, true);
                    break;
                default:
                    PrintUsage();
                    break;
            }
            */

           
        }
        
    }
}
