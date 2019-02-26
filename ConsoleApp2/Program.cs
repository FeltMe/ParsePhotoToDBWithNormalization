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



        static void Main(string[] args)
        {
            var FullFiles = SafeEnumerateFiles(@"C:\Users\sddrozd", "*.JPG", SearchOption.AllDirectories).ToList();
            var Foldrs = new List<string>();
            Console.WriteLine("Go");


            foreach (var item in FullFiles)
            {
                string type = " ";
                var Fname = Path.GetFileName(item);

                type = item.Substring(item.Length - 4);
                Fname = item.Substring(0, item.Length - 4);
                Console.WriteLine(Fname);
                var s = item.Substring(0, (Fname.Length + type.Length));

                Console.WriteLine(s);

                AddFolder(FullFiles, item);
                AddType(type);
                FileAdd(type, Fname);
                PritAll(FullFiles);

                Console.WriteLine("kek");
            }
            Console.WriteLine("Added");
        }

        static protected void FileAdd(string type, string Fname)
        {
            Fname = Path.GetFileName(Fname);
            var pcFiles = new PcFiles();
            foreach (var myFile in pcFiles.MiesType.ToList())
            {

                if (type == myFile.Type)
                {
                    var file1 = new MyFile { Name = Fname, Id_type = myFile.Id };
                    pcFiles.Mies.Add(file1);
                    pcFiles.SaveChanges();
                }
                else
                {
                    var file2 = new MyFile { Name = Fname };
                    pcFiles.Mies.Add(file2);
                    pcFiles.SaveChanges();
                }

            }
        }
        static protected void AddType(string type)
        {
            var pcFiles = new PcFiles();
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
                Console.WriteLine("ke1111k");
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
            var pcFiles = new PcFiles();
            ppath = ppath.Substring(0, ppath.Length - 4);
            if (Foldes.Contains(ppath) == false)
            {
                var myFolder = new MyFolder { Name = ppath };
                Foldes.Add(ppath);
                pcFiles.MyFolders.Add(myFolder);
                pcFiles.SaveChanges();
            }

        }

    }
}
