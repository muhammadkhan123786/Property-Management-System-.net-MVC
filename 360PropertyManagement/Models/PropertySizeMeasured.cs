using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblPropertySize")]
    public class PropertySizeMeasured
    {
        [Key]
        public int SqrftId { get; set; }
        public string SqrftMinimum { get; set; }
        public string sqrtfMaximum { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Remarks { get; set; }
    }
}