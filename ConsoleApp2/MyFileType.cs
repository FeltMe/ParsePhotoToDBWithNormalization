using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class MyFileType
    {
        public int Id { get; set; }
        [Index(IsUnique = true)]
        public string Type { get; set; }
    }
}
