using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Fynns_ISO_Patcher
{
    class Commands
    {
        /// <summary>
        /// System command from C/C++
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [DllImport("msvcrt.dll")]
        public static extern int system(string format);
        public static void System(string filename, string args)
        {
            system($"{filename} {args}");
        }
    }
}