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
            List<string> FullFiles = SafeEnumerateFiles(@"E:\ ", "*.JPG", SearchOption.AllDirectories).ToList();
            Console.WriteLine("Go");

            using (var pcFiles = new PcFiles())
            {
                foreach (var item in FullFiles)
                {
                    string type = " ";
                    //var Fname = Path.GetFileName(item);
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i] == '.')
                        {
                            type = item.Substring(item.Length - 4);
                            //Fname = item.Substring(0, item.Length - 4);
                        }
                    }
                    MyFileType myFileType = new MyFileType { Type = type };
                    bool temp = pcFiles.MiesType.ToList().Contains(myFileType);
                    if (temp == false)
                    {
                        pcFiles.MiesType.ToList().Add(myFileType);
                        pcFiles.SaveChanges();
                        Console.WriteLine("ke1111k");

                    }
                         Console.WriteLine("kek");

                    //foreach (var myFile in pcFiles.MiesType.ToList())
                    //{
                    //
                    //    if (type == myFile.Type)
                    //    {
                    //        MyFile file1 = new MyFile { Name = Fname, Id_type = myFile.Id };
                    //        pcFiles.Mies.Add(file1);
                    //    }
                    //    else
                    //    {
                    //        MyFile file2 = new MyFile { Name = Fname };
                    //        pcFiles.Mies.Add(file2);
                    //    }
                    //    pcFiles.SaveChanges();
                    //
                    //}
                }
                Console.WriteLine("Added");
            }
           //try
           //{
           //    foreach (var item in FullFiles)
           //    {
           //        Console.WriteLine(item);
           //    }
           //    Console.ReadLine();
           //}
           //catch (Exception)
           //{
           //     Console.WriteLine("Write exep");
           //}
        }
    }
}
