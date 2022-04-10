using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Fynns_ISO_Patcher
{
    public class Tools
    {
        public static string FindExePath(string exe)
        {
            exe = Environment.ExpandEnvironmentVariables(exe);
            if (!File.Exists(exe))
            {
                if (Path.GetDirectoryName(exe) == String.Empty)
                {
                    foreach (string test in (Environment.GetEnvironmentVariable("PATH") ?? "").Split(';'))
                    {
                        string path = test.Trim();
                        if (!String.IsNullOrEmpty(path) && File.Exists(path = Path.Combine(path, exe)))
                            return Path.GetFullPath(path);
                    }
                }
                throw new FileNotFoundException(new FileNotFoundException().Message, exe);
            }
            return Path.GetFullPath(exe);
        }

        public static void WriteFile(string filename, string text)
        {
            using StreamWriter writetext = new(filename);
            writetext.WriteLine(text);
            writetext.Close();
        }

        public static void AppendFile(string filename, string text)
        {
            using StreamWriter w = File.AppendText(filename);
            w.WriteLine(text);
            w.Close();
        }

        public static void DeleteFile(string file)
        {
            File.Delete(file);
        }

        public static void DirectoryCopy(string sourceDirName, string destDirName)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, tempPath);
            }
        }
        public static void DeleteFiles(string path, string files)
        {
            string[] file = Directory.GetFiles(path, files, SearchOption.AllDirectories);
            foreach (string a in file)
            {
                File.Delete(a);
            }
        }
        public static void CopyFilesSZS(string sourcePath, string targetPath)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.szs", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }
        public static void CopyFilesTitle(string sourcePath, string targetPath)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                if (newPath.Contains("Title")) {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                }
            }
        }
        public static void CopyFilesTHP(string sourcePath, string targetPath)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, "*.thp", SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

        public static void CopyLECODE(string sourcePath, string targetPath)
        {
            File.Copy(sourcePath + "\\lecode-PAL.bin", targetPath + "\\lecode-PAL.bin", true);
            File.Copy(sourcePath + "\\lecode-USA.bin", targetPath + "\\lecode-USA.bin", true);
            File.Copy(sourcePath + "\\lecode-JAP.bin", targetPath + "\\lecode-JAP.bin", true);
            File.Copy(sourcePath + "\\lecode-KOR.bin", targetPath + "\\lecode-KOR.bin", true);
        }

        public static void CopyFilesFilter(string sourcePath, string targetPath, string filter)
        {
            foreach (string newPath in Directory.GetFiles(sourcePath, filter, SearchOption.AllDirectories))
            {
                File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
            }
        }

    }
}