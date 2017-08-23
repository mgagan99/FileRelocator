using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRelocator
{
    class Program
    {
        //generate list of subfolders in the top folder
        static IEnumerable<string> GetSubfolders(string path)
        {
            IEnumerable<string> subfolders = System.IO.Directory.EnumerateDirectories(path, "*", System.IO.SearchOption.AllDirectories);
            return subfolders;
        }

        //Get all files in the folder
        static IEnumerable<string> GetFiles(string path)
        {
            IEnumerable<string> files = System.IO.Directory.EnumerateFiles(path);
            return files;
        }

        private static void RelocateAllFiles(string fromFolder, string toFolder)
        {
            IEnumerable<string> files = GetFiles(fromFolder);
            foreach (string filename in files)
            {
                DateTime createTime = System.IO.File.GetLastWriteTime(filename);
                int year = createTime.Year;
                Console.WriteLine("Found file [{0}] from year [{1}]", filename, year);

                //create desination folder if needed
                string destFolder = toFolder + year.ToString();
                if (!System.IO.Directory.Exists(destFolder))
                {
                    System.IO.Directory.CreateDirectory(destFolder);
                }

                //Move file to that location
                string destFilename = destFolder + "\\" + System.IO.Path.GetFileName(filename);
                if (!File.Exists(destFilename))
                {
                    System.IO.File.Move(filename, destFilename);
                }
                Console.WriteLine("Relocated to [{0}]", destFilename);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine(" Usage: FileLocator.exe <source> <destination>");
                Console.WriteLine("\t source is location where the files are");
                Console.WriteLine("\t destination is location where files need to be moved (organized by year)");
            }

            string source = args[0];
            string destination = args[1];

            //Move files in top folder
            RelocateAllFiles(source, destination);

            //Move files in subfolders, process one folder at a time
            IEnumerable<string> subFolders = GetSubfolders(source);
            foreach (string subfolder in subFolders)
            {
                RelocateAllFiles(subfolder, destination);
            }
        }
    }
}
