using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblPropertyTopCategory")]
    public class TopCategory
    {
        [Key]
        public int TopCategoryId { get; set; }
        public string TopCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsShowSubCategory { get; set; }
        public bool IsShowArea { get; set; }
        public bool IsShowFurnished { get; set; }
        public bool IsShowRooms { get; set; }

        public string Remarks { get; set; }

        public int? ImageId { get; set; }
        public virtual Images image { get; set; }

    }
}