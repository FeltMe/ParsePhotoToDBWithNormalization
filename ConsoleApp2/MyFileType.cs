using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    public class MyFileType
    {
        [Key]
        public int Id { get; set; }
        [StringLength(450)]
        [Index(IsUnique = true)]
        public string Type { get; set; }
    }
}
