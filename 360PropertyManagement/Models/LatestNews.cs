using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblLatestNews")]
    public class LatestNews
    {
        [Key]
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string Newsdescription { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }

        public int? ImageId { get; set; }
        public virtual Images image { get; set; }
    }
}