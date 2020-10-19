using HangFire.Domain.Seedwork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire.Models
{
    public class ScanningUrl:Entity
    {
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
