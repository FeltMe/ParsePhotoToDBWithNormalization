using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.Sql;
using System.Configuration;
using System.Data.Entity;

namespace ConsoleApp2
{
    public class Program
    {

        private static IEnumerable<string> SafeEnumerateFiles(string path, string searchPattern = "*.*", SearchOption searchOption = SearchOption.AllDirectories)
        {
            var dirs = new Stack<string>();
            dirs.Push(path);

            while (dirs.Count > 0)
            {
                string currentDirPath = dirs.Pop();
                if (searchOption == SearchOption.AllDirectories)
                {
                    try
                    {
                        string[] subDirs = Directory.GetDirectories(currentDirPath);
                        foreach (string subDirPath in subDirs)
                        {
                            dirs.Push(subDirPath);
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        continue;
                    }
                }

                string[] files = null;
                try
                {
                    files = Directory.GetFiles(currentDirPath, searchPattern);
                }
                catch (UnauthorizedAccessException)
                {
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    continue;
                }

                foreach (string filePath in files)
                {
                    yield return filePath;
                }
            }
        }

        static private List<string> Foldrs = new List<string>();


        static void Main(string[] args)
        {
            Start();
        }

        static protected void FileAdd(string type, string Fname, string folder)
        {
            Fname = Path.GetFileName(Fname);
            int id_type = 0;
            bool IsHave = false;
            CheckForTypes(type, ref IsHave, ref id_type);

            using (var pcFiles = new PcFiles())
            {                
                foreach (var fol in pcFiles.MyFolders.ToList())
                {
                    if (folder == fol.Name)
                    {
                        if (IsHave == true)
                        {
                            var file = new MyFile { Name = Fname, Id_type = id_type, Id_folder = fol.Id };
                            pcFiles.Mies.Add(file);
                            pcFiles.SaveChanges();
                        }
                    }
                }
            }
        }
        static protected void AddType(string type)
        {
            using (var pcFiles = new PcFiles())
            {
                bool IsHaveThisType = false;

                foreach (var my in pcFiles.MiesType.ToList())
                {
                    if (my.Type == type)
                    {
                        IsHaveThisType = true;
                    }
                }
                if (IsHaveThisType == false)
                {
                    var myFileType = new MyFileType { Type = type };
                    pcFiles.MiesType.Add(myFileType);
                    pcFiles.SaveChanges();
                }
            }

        }
        static protected void PritAll(List<string> FullFiles)
        {
            try
            {
                foreach (var item in FullFiles)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Writing Exception");
            }
        }
        static protected void AddFolder(List<string> Foldes, string ppath)
        {
            using (var pcFiles = new PcFiles())
            {
                ppath = Path.GetDirectoryName(ppath);
                if (Foldes.Contains(ppath) == false)
                {
                    var myFolder = new MyFolder { Name = ppath };
                    Foldrs.Add(ppath);
                    pcFiles.MyFolders.Add(myFolder);
                    pcFiles.SaveChanges();
                }
            }
        }
        static protected void CheckForTypes(string type, ref bool have, ref int id_type)
        {
            using (PcFiles pcFiles = new PcFiles())
            {
                foreach (var myFile in pcFiles.MiesType.ToList())
                {
                    if (type == myFile.Type)
                    {
                        id_type = myFile.Id;
                        have = true;
                    }
                }
            }
        }
        static public void Start()
        {
            var FullFiles = SafeEnumerateFiles(@"C:\Users\sddrozd", "*.JPG", SearchOption.AllDirectories).ToList();
            Console.WriteLine("\nGo");
            foreach (var item in FullFiles)
            {
                string type = " ";
                string folder = Path.GetDirectoryName(item);
                var Fname = Path.GetFileName(item);

                type = item.Substring(item.Length - 4);
                Fname = item.Substring(0, item.Length - 4);

                AddFolder(Foldrs, item);
                AddType(type);
                FileAdd(type, Fname, folder);
            }
            PritAll(FullFiles);
            Console.WriteLine("Added");
        }
    }
}
