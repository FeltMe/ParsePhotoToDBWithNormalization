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

            List<string> FullFiles = SafeEnumerateFiles(@"C:\Users\", "*.JPG", SearchOption.AllDirectories).ToList();
            Console.WriteLine("Go");
            //foreach (var item in FullFiles)
            //{
            //    Console.WriteLine(item);
            //}
            //Console.Read();


            using (var pcFiles = new PcFiles())




            {




                foreach (var item in FullFiles)
                {
                    string type = " ";
                    string FName = "";
                    for (int i = 0; i < item.Length; i++)
                    {
                        if (item[i] == '.')
                        {
                            type = item.Substring(item.Length - 4);
                            FName = item.Substring(item.Length - 4);
                        }
                    }
                    try
                    {
                        MyFileType myFileType = new MyFileType { Type = type };
                        pcFiles.MiesType.Add(myFileType);
                        pcFiles.SaveChanges();
                    }
                    catch (Exception)
                    {

                    }

                    var l = pcFiles.MiesType.ToList();

                    foreach (var t in l)
                    {
                    if (type == t.Type)
                    {
                        MyFile file = new MyFile { Name = "FName33", Id_type = 841 };
                        Console.WriteLine(pcFiles.Entry(file).State.ToString());

                        pcFiles.Mies.Add(file);
                        Console.WriteLine(pcFiles.Entry(file).State.ToString());
                        Console.WriteLine(file.Name);
                        Console.WriteLine(file.Id_type);
                        pcFiles.SaveChanges();
                        Console.WriteLine(pcFiles.Entry(file).State.ToString());
                    }
                    else
                    {
                        MyFile file = new MyFile { Name = FName };
                        pcFiles.Mies.Add(file);
                        pcFiles.SaveChanges();
                    }
                    }
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
