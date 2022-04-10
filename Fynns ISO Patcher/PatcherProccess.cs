using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Fynns_ISO_Patcher
{
    public class PatcherProccess
    {
        public static void ExtractFile(string input, string wit)
        {
            Console.WriteLine();
            Commands.System(wit, @$"extract ""{input}"" workdir.tmp -o -vvv --psel DATA");
        }
        public static void Start(string wszst, bool riiv)
        {
            Console.WriteLine("\n* Patch Menu Files [LE-CODE]");
            File.Copy(@".\workdir.tmp\files\Scene\UI\Channel.szs", @".\patch\lecode\ui\Channel.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\Globe.szs", @".\patch\lecode\ui\Globe.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\MenuMulti.szs", @".\patch\lecode\ui\MenuMulti.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\MenuOther.szs", @".\patch\lecode\ui\MenuOther.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\MenuSingle.szs", @".\patch\lecode\ui\MenuSingle.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\Present.szs", @".\patch\lecode\ui\Present.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\Race.szs", @".\patch\lecode\ui\Race.szs", true);
            File.Copy(@".\workdir.tmp\files\Scene\UI\Title.szs", @".\patch\lecode\ui\Title.szs", true);
            Commands.System(wszst, "extract ./patch/lecode/ui/*.szs -o -q");
            Tools.DeleteFiles(@".\patch\lecode\ui\", "*.szs");
            Tools.DirectoryCopy(@".\patch\lecode\ui\Channel", @".\patch\lecode\ui\Channel.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\Globe", @".\patch\lecode\ui\Globe.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\MenuMulti", @".\patch\lecode\ui\MenuMulti.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\MenuOther", @".\patch\lecode\ui\MenuOther.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\MenuSingle", @".\patch\lecode\ui\MenuSingle.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\Present", @".\patch\lecode\ui\Present.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\Race", @".\patch\lecode\ui\Race.d");
            Tools.DirectoryCopy(@".\patch\lecode\ui\Title", @".\patch\lecode\ui\Title.d");
            Commands.System(wszst, "c ./patch/lecode/ui/*.d -o -q");
            Tools.CopyFilesSZS(@".\patch\lecode\ui\", @".\workdir.tmp\files\Scene\UI\");
            Console.WriteLine("* Prepare main.dol and StaticR.rel [LE-CODE]");
            File.Copy(@".\workdir.tmp\sys\main.dol", @".\patch\sys\main.dol", true);
            File.Copy(@".\workdir.tmp\files\rel\StaticR.rel", @".\patch\sys\StaticR.rel", true);
            Console.WriteLine("* Create Patch Files");
            Commands.System(wszst, "autoadd ./workdir.tmp/files/Race/Course -D ./bin/tools/auto-add -q -o");
            Console.WriteLine("* Convert WBZ Files");
            Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            TimeSpan ts = sw.Elapsed;
            string[] wbzfiles = Directory.GetFiles(".\\patch\\wbz-files", "*.wbz");
            int count = 0;
            int filecount = wbzfiles.Length;
            foreach (string wbzfile in wbzfiles)
            {
                ts = sw.Elapsed;
                string seconds = ts.Seconds.ToString();
                if (ts.Seconds < 10)
                {
                    seconds = "0" + seconds;
                }
                Console.Write($"Progress: {count}/{filecount} ({ts.Minutes}m:{seconds}s)\r");
                Commands.System(wszst, $"compress --szs \"{wbzfile}\" -E$ --dest ./workdir.tmp/files/Race/Course/$N.szs -q -o");
                count++;
            }
            sw.Stop();
            Console.WriteLine("* Patch BMG Messages         ");
            Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_?.szs --patch-bmg \"overwrite=./patch/bmg/Common.bmg\" -q");
            Console.WriteLine("* Patch Strap Images");
            string REG = null;
            if (File.Exists(@".\workdir.tmp\files\Scene\UI\MenuSingle_E.szs"))
            {
                REG = "PAL";
            }
            else if (File.Exists(@".\workdir.tmp\files\Scene\UI\MenuSingle_U.szs"))
            {
                REG = "USA";
            }
            else if (File.Exists(@".\workdir.tmp\files\Scene\UI\MenuSingle_R.szs"))
            {
                REG = "KOR";
            }
            else if (File.Exists(@".\workdir.tmp\files\Scene\UI\MenuSingle_J.szs"))
            {
                REG = "JAP";
            }
            if(REG == "PAL")
            {
                Commands.System(wszst, "xall ./workdir.tmp/files/Boot/Strap/eu/*.szs --no-mm -o -q");
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Dutch.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Dutch.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Dutch.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Dutch.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\English.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\English.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\English.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\English.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\French.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\French.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\French.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\French.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\German.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\German.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\German.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\German.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Italian.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Italian.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Italian.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Italian.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Spanish_EU.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\eu\Spanish_EU.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Spanish_EU.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\eu\Spanish_EU.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                Commands.System(wszst, "create ./workdir.tmp/files/Boot/Strap/eu/*.d -o -a -q");
            }
            else if(REG == "USA")
            {
                Commands.System(wszst, "xall ./workdir.tmp/files/Boot/Strap/us/*.szs --no-mm -o -q");
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\English.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\English.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\English.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\English.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\French.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\French.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\French.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\French.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\Spanish_US.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\us\Spanish_US.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\Spanish_US.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\us\Spanish_US.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                Commands.System(wszst, "create ./workdir.tmp/files/Boot/Strap/us/*.d -o -a -q");
            }
            else if (REG == "KOR")
            {
                Commands.System(wszst, "xall ./workdir.tmp/files/Boot/Strap/kr/*.szs --no-mm -o -q");
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\kr\Korean.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\kr\Korean.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\kr\Korean.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\kr\Korean.szs.d\Textures(NW4R)\strapB_608x456.png", true);
            }
            else if(REG == "JAP")
            {
                Commands.System(wszst, "xall ./workdir.tmp/files/Boot/Strap/jp/*.szs --no-mm -o -q");
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\jp\jp.szs.d\Textures(NW4R)\strapA_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap1.png", @".\workdir.tmp\files\Boot\Strap\jp\jp.szs.d\Textures(NW4R)\strapA_608x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\jp\jp.szs.d\Textures(NW4R)\strapB_16_9_832x456.png", true);
                File.Copy(@".\patch\img-patch\strap\strap2.png", @".\workdir.tmp\files\Boot\Strap\jp\jp.szs.d\Textures(NW4R)\strapB_608x456.png", true);
                Commands.System(wszst, "create ./workdir.tmp/files/Boot/Strap/jp/*.d -o -a -q");
            }
            Console.WriteLine("* Patch Title Images");
            Tools.CopyFilesTitle(@".\workdir.tmp\files\Scene\UI\", @".\patch\img-patch\title\");
            Commands.System(wszst, "xall ./patch/img-patch/title/Title*.szs --no-mm -o -q");
            if(REG == "PAL")
            {
                File.Copy(@".\patch\img-patch\title\title_full.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_E.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_E.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_E.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_E.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_F.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_F.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_F.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_F.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_G.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_G.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_G.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_G.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_I.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_I.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_I.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_I.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_S.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_S.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_S.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_S.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                Commands.System(wszst, "create ./patch/img-patch/title/*.d -o -a -q");
            }
            else if (REG == "USA")
            {
                File.Copy(@".\patch\img-patch\title\title_full.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_M.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_M.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_M.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_M.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_Q.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_Q.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_Q.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_Q.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_U.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_U.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_U.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_U.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png, true");
                Commands.System(wszst, "create ./patch/img-patch/title/*.d -o -a -q");
            }
            else if (REG == "KOR")
            {
                File.Copy(@".\patch\img-patch\title\title_full.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_K.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_K.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_K.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_K.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                Commands.System(wszst, "create ./patch/img-patch/title/*.d -o -a -q");
            }
            else if(REG == "JAP")
            {
                File.Copy(@".\patch\img-patch\title\title_full.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_koopa_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_luigi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario2_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_mario_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title.d\title\timg\tt_title_screen_peachi_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_J.d\title\timg\tt_title_screen_mario0.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_bottom.png", @".\patch\img-patch\title\Title_J.d\title\timg\tt_title_screen_mario0_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_J.d\title\timg\tt_title_screen_title_rogo_bokeboke.tpl.png", true);
                File.Copy(@".\patch\img-patch\title\title_top.png", @".\patch\img-patch\title\Title_J.d\title\timg\tt_title_screen_title_rogo_tm_only.tpl.png", true);
                Commands.System(wszst, "create ./patch/img-patch/title/*.d -o -a -q");
            }
            Tools.CopyFilesSZS(@".\patch\img-patch\title\", @".\workdir.tmp\files\Scene\UI\");
            Console.WriteLine("* Replace videos to reduce image size");
            Tools.CopyFilesTHP(@".\patch\thp\vids\course\", @".\workdir.tmp\files\thp\course");
            Tools.CopyFilesTHP(@".\patch\thp\vids\battle\", @".\workdir.tmp\files\thp\battle");
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\title\title.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\title\title_50.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\title\title_SD.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\title\title_SD_50.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\ending\ending_normal.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\ending\ending_normal_50.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\ending\ending_true.thp", true);
            File.Copy(@".\workdir.tmp\files\thp\title\top_menu.thp", @".\workdir.tmp\files\thp\ending\ending_true_50.thp", true);
            Console.WriteLine("* Remove unneeded files");
            Tools.DeleteFiles(@".\workdir.tmp\files\Race\Course\", "old_mario_gc_*.szs");
            Console.WriteLine("* Patch main.dol and StaticR.rel");
            Commands.System(wszst, "wstrt patch ./patch/sys/main.dol ./patch/sys/StaticR.rel --clean-dol --add-lecode --wiimmfi --region 20222 --all-ranks --add-section ./patch/pack-@.gct -q");
            File.Copy(@".\patch\sys\main.dol", @".\workdir.tmp\sys\main.dol", true);
            File.Copy(@".\patch\sys\StaticR.rel", @".\workdir.tmp\files\rel\StaticR.rel", true);
            Console.WriteLine("* Patch LE-CODE [LE-CODE]");
            Commands.System(wszst, "wlect patch ./patch/lecode/bin/*.bin --le-define ./patch/lecode/le-define.txt --lpar ./patch/lecode/lecode-param.txt --track-dir ./workdir.tmp/files/Race/Course --move-tracks ./workdir.tmp/files/Race/Course -q");
            Tools.CopyLECODE(@".\patch\lecode\bin", @".\workdir.tmp\files\rel");

            if (riiv == true)
            {
                ConsoleColor origColor = Console.ForegroundColor;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\nPreparing Riivolution Generation\n\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = origColor;
                Console.WriteLine("* Copying Files");
                if(Directory.Exists("riiv-sd"))
                {
                    Directory.Delete("riiv-sd", true);
                }
                Directory.CreateDirectory("riiv-sd");
                string pkgname = File.ReadAllText(".\\patch\\riiv\\pkgname.txt");
                Directory.Move(".\\patch\\riiv\\template\\riiv-sd-stp\\%PKGNAME%", $".\\patch\\riiv\\template\\riiv-sd-stp\\{pkgname}");
                Tools.DirectoryCopy(".\\patch\\riiv\\template\\riiv-sd-stp", ".\\riiv-sd");
                Console.WriteLine("* Patching Files");
                foreach(string file in Directory.EnumerateFiles(".\\workdir.tmp\\files\\Race\\Course", "*.szs"))
                {
                    Commands.System(".\\bin\\tools\\node.exe", $"./bin/tools/riiv-track-gen.js {file} {pkgname} >> .\\patch\\riiv\\template\\tmp.xml");
                }
                if (REG == "PAL")
                {
                    File.WriteAllText($".\\riiv-sd\\riivolution\\{pkgname}.xml", $"{File.ReadAllText(".\\patch\\riiv\\template\\default0p.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\tmp.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\default1.xml")}");
                }
                if (REG == "USA")
                {
                    File.WriteAllText($".\\riiv-sd\\riivolution\\{pkgname}.xml", $"{File.ReadAllText(".\\patch\\riiv\\template\\default0u.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\tmp.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\default1.xml")}");
                }
                if (REG == "JAP")
                {
                    File.WriteAllText($".\\riiv-sd\\riivolution\\{pkgname}.xml", $"{File.ReadAllText(".\\patch\\riiv\\template\\default0j.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\tmp.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\default1.xml")}");
                }
                if (REG == "KOR")
                {
                    File.WriteAllText($".\\riiv-sd\\riivolution\\{pkgname}.xml", $"{File.ReadAllText(".\\patch\\riiv\\template\\default0k.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\tmp.xml")}{File.ReadAllText(".\\patch\\riiv\\template\\default1.xml")}");
                }
                File.Delete(".\\patch\\riiv\\template\\tmp.xml");
                File.Copy(".\\workdir.tmp\\sys\\main.dol", $".\\riiv-sd\\{pkgname}\\sys\\main.dol", true);
                Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Scene\\UI", $".\\riiv-sd\\{pkgname}\\Scene\\UI");
                Tools.CopyFilesFilter(".\\workdir.tmp\\files\\rel", $".\\riiv-sd\\{pkgname}\\rel", "*.*");
                Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Race\\Course", $".\\riiv-sd\\{pkgname}\\Race\\Course");
                if (REG == "PAL")
                {
                    Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Boot\\Strap\\eu", $".\\riiv-sd\\{pkgname}\\Boot\\Strap\\eu");
                }
                if (REG == "USA")
                {
                    Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Boot\\Strap\\us", $".\\riiv-sd\\{pkgname}\\Boot\\Strap\\us");
                }
                if (REG == "JAP")
                {
                    Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Boot\\Strap\\jp", $".\\riiv-sd\\{pkgname}\\Boot\\Strap\\jp");
                }
                if (REG == "KOR")
                {
                    Tools.CopyFilesSZS(".\\workdir.tmp\\files\\Boot\\Strap\\kr", $".\\riiv-sd\\{pkgname}\\Boot\\Strap\\kr");
                }
                Commands.System(".\\bin\\tools\\replace.vbs", $"./riiv-sd/riivolution/{pkgname}.xml $DISTNAME$ {pkgname}");
                Commands.System(".\\bin\\tools\\replace.vbs", $"./riiv-sd/riivolution/{pkgname}.xml $TITLE$ {pkgname}");
                Commands.System(".\\bin\\tools\\replace.vbs", $"./riiv-sd/riivolution/{pkgname}.xml \".\\workdir.tmp\\files\\Race\\Course\\\" \"\"");
                Console.WriteLine("* Clean Up");
                Commands.System("start", "\"\" \".\\bin\\tools\\rm-tmp\"");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Done Creating the Riivolution Image.\nCreated one (1) Riivolution image!\n");
                Console.ForegroundColor = origColor;
                Commands.System("pause", "");
            }
        }

        public static void CreateFile(string output, string format, string wit, string pn)
        {
            if (String.IsNullOrEmpty(output))
            {
                output = pn;
            }
            if(String.IsNullOrEmpty(format))
            {
                format = "wbfs";
            }
            Commands.System(wit, $"cp \"workdir.tmp\" \"{output}.{format}\" -o -vvv --id NAAS02");
        }

        public static void CleanUp()
        {
            Directory.Delete("patch", true);
            Directory.Delete("workdir.tmp", true);
        }
    }
}
