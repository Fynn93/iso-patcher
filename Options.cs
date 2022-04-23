using CommandLine;

namespace Fynns_ISO_Patcher
{
    public class Options
    {
        [Option('f', "format", HelpText = "Define the Output Format ex: \" -f wdf\" (not required when using riivolution)")]
        public string Format { get; set; }
        [Option('o', "output", HelpText = "Define Output file ex: \"-o foo\"")]
        public string Output { get; set; }
        [Option('i', "input", Required = true, HelpText = "Define an Input File ex: \"-i bar.wbfs\"")]
        public string Input { get; set; }
        [Option('r', "riivolution", HelpText = "The output will be a Riivolution Distribution")]
        public bool Riivolution { get; set; }
    }
}