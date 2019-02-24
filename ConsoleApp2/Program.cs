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

            List<string> FullFiles = SafeEnumerateFiles("E:\\Selebraite\\new", "*.JPG" , SearchOption.AllDirectories).ToList();
            //List<string> 
            var s = Path.GetDirectoryName("E:\\Selebraite\\new");
            Console.WriteLine(s);
            using (PcFiles pcFiles = new PcFiles())
            {
                foreach (var item in FullFiles)
                {
                    bool IsOlredyType = false;
                    string type = " ";
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i] == '.')
                        {
                            IsOlredyType = true;
                        }
                        if(IsOlredyType == true)
                        {
                            type += item[i];
                        }
                    }
                    MyFile file = new MyFile { Name = item };
                    MyFileType myFileType = new MyFileType { Type = type };
                    pcFiles.MiesType.Add(myFileType);
                    //pcFiles.Mies.Add(file);
                    pcFiles.SaveChanges();
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
            //catch (Exception e)
            //{
            //    e.ToString();
            //}
        }
    }
}
