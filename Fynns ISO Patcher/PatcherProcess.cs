﻿using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Http;

namespace Fynns_ISO_Patcher
{
    public class PatcherProcess
    {
        public static void ExtractFile(string input, string wit)
        {
            Console.WriteLine();
            Commands.System(wit, @$"extract ""{input}"" workdir.tmp -o -vvv --psel DATA");
        }
        public static void Start(string wszst, bool riiv, string pkgname, string region)
        {
            Console.WriteLine("* Create Cache");
            Directory.CreateDirectory("fip-cache");
            Directory.CreateDirectory("fip-cache\\tracks");
            Console.WriteLine("* Patch Menu Files [LE-CODE]");
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
            Stopwatch eta = Stopwatch.StartNew();
            TimeSpan ts = sw.Elapsed;
            TimeSpan etatime = eta.Elapsed;
            TimeSpan etat;
            string[] wbzfiles = Directory.GetFiles(".\\patch\\wbz-files", "*.wbz");
            /*if (Directory.Exists(".\\fip-cache\\tracks"))
            {
                string[] cachefiles = Directory.GetFiles(".\\fip-cache\\tracks", "*.szs");
                int ccount = 0;
                if (cachefiles[0] != "")
                {
                    foreach (string wbzfile in wbzfiles)
                    {
                        if (cachefiles[ccount].Replace(".szs", "") == wbzfiles[ccount].Replace(".wbz", ""))
                        {
                            string wbzfilename = wbzfile.Replace(".wbz", "");
                            string szsfilename = cachefiles[ccount].Replace(".szs", "");
                            ccount++;
                        }
                    }
                    if (ccount == wbzfiles.Length)
                        Tools.CopyFilesSZS(".\\fip-cache\\tracks", ".\\workdir.tmp\\files\\Race\\Course");
                }
            }*/
            
            int count = 1;
            int filecount = wbzfiles.Length;
            int etai = 0;
            
            foreach (string wbzfile in wbzfiles)
            {
                ts = sw.Elapsed;
                etatime = eta.Elapsed;
                string seconds = ts.Seconds.ToString();
                string etas = etatime.Seconds.ToString();
                if (ts.Seconds < 10)
                {
                    seconds = "0" + seconds;
                }
                if (etatime.Seconds < 10)
                {
                    etas = "0" + etas;
                }
                etai = (int)etatime.TotalSeconds / count * (filecount - count);
                etat = TimeSpan.FromSeconds(etai);
                if (count < 10)
                    Console.Write($"Progress: {count}/{filecount} ({ts.Minutes}m:{seconds}s)\r");
                else if (count >= 11)
                    Console.Write($"Progress: {count}/{filecount} ({ts.Minutes}m:{seconds}s) | eta: ({etat.Minutes}m:{etas}s)   \r");
                Commands.System(wszst, $"compress --szs \"{wbzfile}\" -E$ -qod ./fip-cache/tracks/$N.szs");
                File.Copy($".\\fip-cache\\tracks\\{Path.GetFileNameWithoutExtension(wbzfile)}.szs", $".\\workdir.tmp\\files\\Race\\Course\\{Path.GetFileName(wbzfile)}", true);
                count++;
            }
            sw.Stop();
            string REG = "";
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
            Console.WriteLine("* Patch BMG Messages                            ");
            Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_?.szs --patch-bmg \"overwrite=./patch/bmg/Common.txt\" -q");
            if (REG == "PAL")
            {
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_E.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_E.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_E.szs --patch-bmg \"overwrite=./patch/bmg/Number_E.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_E.szs --patch-bmg \"overwrite=./patch/bmg/Menu_E.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_E.szs --patch-bmg \"overwrite=./patch/bmg/Race_E.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_F.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_F.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_F.szs --patch-bmg \"overwrite=./patch/bmg/Number_F.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_F.szs --patch-bmg \"overwrite=./patch/bmg/Menu_F.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_F.szs --patch-bmg \"overwrite=./patch/bmg/Race_F.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_G.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_G.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_G.szs --patch-bmg \"overwrite=./patch/bmg/Number_G.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_G.szs --patch-bmg \"overwrite=./patch/bmg/Menu_G.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_G.szs --patch-bmg \"overwrite=./patch/bmg/Race_G.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_I.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_I.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_I.szs --patch-bmg \"overwrite=./patch/bmg/Number_I.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_I.szs --patch-bmg \"overwrite=./patch/bmg/Menu_I.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_I.szs --patch-bmg \"overwrite=./patch/bmg/Race_I.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_S.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_S.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_S.szs --patch-bmg \"overwrite=./patch/bmg/Number_S.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_S.szs --patch-bmg \"overwrite=./patch/bmg/Menu_S.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_S.szs --patch-bmg \"overwrite=./patch/bmg/Race_S.txt\" -q");
            }
            else if (REG == "USA")
            {
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_M.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_M.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_M.szs --patch-bmg \"overwrite=./patch/bmg/Number_M.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_M.szs --patch-bmg \"overwrite=./patch/bmg/Menu_M.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_M.szs --patch-bmg \"overwrite=./patch/bmg/Race_M.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_Q.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_Q.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_Q.szs --patch-bmg \"overwrite=./patch/bmg/Number_Q.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_Q.szs --patch-bmg \"overwrite=./patch/bmg/Menu_Q.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_Q.szs --patch-bmg \"overwrite=./patch/bmg/Race_Q.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_U.szs --patch-bmg \"overwrite=./patch/bmg/Wiimmfi_U.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_U.szs --patch-bmg \"overwrite=./patch/bmg/Number_U.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_U.szs --patch-bmg \"overwrite=./patch/bmg/Menu_U.txt\" -q");
                Commands.System(wszst, "patch ./workdir.tmp/files/Scene/UI/*_U.szs --patch-bmg \"overwrite=./patch/bmg/Race_U.txt\" -q");
            }
            Console.WriteLine("* Patch Strap Images");
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
            Commands.System(wszst, $"wstrt patch ./patch/sys/main.dol ./patch/sys/StaticR.rel --clean-dol --add-lecode --region {region} --all-ranks --add-section ./patch/pack-@.gct -q");
            File.Copy(@".\patch\sys\main.dol", @".\workdir.tmp\sys\main.dol", true);
            File.Copy(@".\patch\sys\StaticR.rel", @".\workdir.tmp\files\rel\StaticR.rel", true);
            Console.WriteLine("* Patch LE-CODE [LE-CODE]");
            Commands.System(wszst, "wlect patch ./patch/lecode/bin/*.bin --le-define ./patch/lecode/le-define.txt --lpar ./patch/lecode/lecode-param.txt --track-dir ./workdir.tmp/files/Race/Course --move-tracks ./workdir.tmp/files/Race/Course -q");
            Tools.CopyLECODE(@".\patch\lecode\bin", @".\workdir.tmp\files\rel");
            Console.WriteLine("* Extract Common Files");
            Commands.System(wszst, "xcommon ./workdir.tmp/files/Race/Course/*.szs -qiod ./workdir.tmp/Race/Common/$N -E$");

            if (riiv == true)
            {
                ConsoleColor origColor = Console.ForegroundColor;
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("\nPreparing Riivolution Generation\n");
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = origColor;
                Console.WriteLine("* Copying Files");
                if(Directory.Exists("riiv-sd"))
                {
                    Directory.Delete("riiv-sd", true);
                }
                Directory.CreateDirectory("riiv-sd");
                if(string.IsNullOrEmpty(pkgname)) pkgname = File.ReadAllText(".\\patch\\riiv\\pkgname.txt");
                Directory.Move(".\\patch\\riiv\\template\\riiv-sd-stp\\%PKGNAME%", $".\\patch\\riiv\\template\\riiv-sd-stp\\{pkgname}");
                Tools.DirectoryCopy(".\\patch\\riiv\\template\\riiv-sd-stp", ".\\riiv-sd");
                Console.WriteLine("* Patching Files");
                foreach(string file in Directory.GetFiles(".\\workdir.tmp\\files\\Race\\Course", "*.szs"))
                {
                    File.AppendAllText(".\\patch\\riiv\\template\\tmp.xml", $"  <file disc=\"/Race/Course/{file}\" external=\"/{pkgname}/Race/Course/{file}\" create=\"true\" />");
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
                Console.WriteLine("Press any key to exit.");
                Commands.System("pause", "");
            }
        }

        public static void CreateFile(string output, string format, string wit, string pn, string id)
        {
            if (String.IsNullOrEmpty(output))
            {
                output = pn;
            }
            if(String.IsNullOrEmpty(format))
            {
                format = "wbfs";
            }
            Commands.System(wit, $"cp \"workdir.tmp\" \"{output}.{format}\" -o -vvv --id {id}");
        }

        public static void CleanUp()
        {
            Directory.Delete("patch", true);
            Directory.Delete("workdir.tmp", true);
        }
    }
}