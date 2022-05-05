using Sharprompt;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Wrapper
{
    class Code
    {
        [DllImport("msvcrt.dll")]
        public static extern int system(string format);
        public static void System(string filename, string args)
        {
            system($"{filename} {args}");
        }

        static void Main(string[] args)
        {
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
            Console.ForegroundColor = origColor;

            int pagesize = 20;
            int count = 0;
            string[] filearray = new string[Directory.GetFiles(".", "*.wbfs", SearchOption.AllDirectories).Length + Directory.GetFiles(".", "*.iso", SearchOption.AllDirectories).Length + Directory.GetFiles(".", "*.wdf", SearchOption.AllDirectories).Length + Directory.GetFiles(".", "*.wia", SearchOption.AllDirectories).Length];

            foreach (string file in Directory.GetFiles(".", "*.wbfs", SearchOption.AllDirectories))
            {
                filearray[count] = file;
                count++;
            }

            foreach (string file in Directory.GetFiles(".", "*.iso", SearchOption.AllDirectories))
            {
                filearray[count] = file;
                count++;
            }

            foreach (string file in Directory.GetFiles(".", "*.wdf", SearchOption.AllDirectories))
            {
                filearray[count] = file;
                count++;
            }

            foreach (string file in Directory.GetFiles(".", "*.wia", SearchOption.AllDirectories))
            {
                filearray[count] = file;
                count++;
            }

            string extraargs = "";

            string selectedfile = Prompt.Select("Select file to patch", filearray, pagesize);
            extraargs += $"-i {selectedfile} ";

            string format = Prompt.Select("Select Patching mode", new[] { $"Patch {selectedfile}", "Generate Riivolution files" });
            string outputformat = "wbfs";
            string outputfile = "";

            if (format == "Generate Riivolution files")
            {
                extraargs += "-r ";
            }
            else
            {
                outputformat = Prompt.Select("Select output format", new[] { "wbfs", "iso", "wdf", "wia" });
                extraargs += $"-f {outputformat} ";

                outputfile = Prompt.Input<string>("Type output file name (without extension!)");
                extraargs += $"-o {outputfile}";
            }

            

            if (format == "Generate Riivolution files")
                Console.WriteLine($@"
Selected File  : {selectedfile}
Patching Mode  : {format}
");
            else
                Console.WriteLine($@"
Selected File  : {selectedfile}
Patching Mode  : {format}
Output Format  : {outputformat}
Output Filename: {outputfile}
");
            bool answer = Prompt.Confirm("Are you ready?", defaultValue: true);
            if (answer)
                System("\"bin\\tools\\Fynns ISO Patcher\"", extraargs);
            else
                Console.WriteLine("Goodbye!");
                Environment.Exit(0);
        }
    }
}
