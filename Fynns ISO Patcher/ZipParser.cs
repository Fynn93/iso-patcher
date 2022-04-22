using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace Fynns_ISO_Patcher
{
    public class ZipParser
    {
        public static void DecompressFile(string inFile, string outFile)
        {
            if (Directory.Exists("patch")) { Directory.Delete("patch", true); }
            Console.WriteLine("\n* Extract Patch File");
            ZipFile.ExtractToDirectory(inFile, outFile, true);
        }
    }
}
