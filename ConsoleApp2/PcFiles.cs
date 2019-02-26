using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApp2
{
    public class PcFiles : DbContext
    {
        public DbSet<MyFile> Mies { get; set; }
        public DbSet<MyFileType> MiesType { get; set; }
        public DbSet<MyFolder> MyFolders { get; set; }
        public PcFiles() : base(ConfigurationManager.ConnectionStrings["ConStr"].ConnectionString)
        {

        }
    }
}
