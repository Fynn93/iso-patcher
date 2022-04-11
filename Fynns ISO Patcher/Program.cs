using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using CommandLine;
using System.Reflection;

namespace Fynns_ISO_Patcher
{
    class Code
    {
        public static void ResetConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static async Task Main(string[] args)
        {
            var options = new Options(); Parser.Default.ParseArguments<Options>(args).WithParsed(parsed => options = parsed); if(String.IsNullOrEmpty(options.Input)) { Commands.System("pause", ""); Environment.Exit(1); } string Input = options.Input; string Output = options.Output; string Format = options.Format; bool Riivolution = options.Riivolution;
            string CUR_DIR = AppDomain.CurrentDomain.BaseDirectory;
            string[] wszstfiles = Directory.GetFiles(CUR_DIR, "wszst", SearchOption.AllDirectories);
            string[] witfiles = Directory.GetFiles(CUR_DIR, "wit", SearchOption.AllDirectories);
            string WIT = ".\\bin\\tools\\wit.exe";
            string WSZST = ".\\bin\\tools\\wszst.exe";
            ConsoleColor origColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@$"
  ______                       _____  _____  ____    _____      _       _               
 |  ____|                     |_   _|/ ____|/ __ \  |  __ \    | |     | |              
 | |__ _   _ _ __  _ __  ___    | | | (___ | |  | | | |__) |_ _| |_ ___| |__   ___ _ __ 
 |  __| | | | '_ \| '_ \/ __|   | |  \___ \| |  | | |  ___/ _` | __/ __| '_ \ / _ \ '__|
 | |  | |_| | | | | | | \__ \  _| |_ ____) | |__| | | |  | (_| | || (__| | | |  __/ |   
 |_|   \__, |_| |_|_| |_|___/ |_____|_____/ \____/  |_|   \__,_|\__\___|_| |_|\___|_|   
        __/ |                                                                           
       |___/
 Version: {Assembly.GetExecutingAssembly().GetName().Version}
");
            Console.Title = "Fynns ISO Patcher v2.0 | C# Version";
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("Setup...\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================================================================================================================");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" WIT   = {0}bin\\tools\\wit.exe", CUR_DIR);
            Console.WriteLine(" WSZST = {0}bin\\tools\\wszst.exe", CUR_DIR);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("============================================================================================================================\n");
            ResetConsole();
            string df = Variables.DataFile();
            string pn = Variables.ProjectName();
            string uu = Variables.UpdateURL();
            string v  = Variables.Version();
            bool mkdi = Variables.MKDI();
            if (mkdi) Console.WriteLine("MKDI Support enabled (BETA!)");
            Console.WriteLine("Checking for Updates...");
            await Updater.Update(uu, v);
            PatcherProccess.ExtractFile(options.Input, WIT);
            ZipParser.DecompressFile(df, "patch");
            if(Riivolution == true)
            {
                PatcherProccess.Start(WSZST, true, pn);
            }
            else
            {
                PatcherProccess.Start(WSZST, false, pn);
                PatcherProccess.CreateFile(options.Output, options.Format, WIT, pn);
            }
            PatcherProccess.CleanUp();
            Environment.Exit(0);
        }
    }
}