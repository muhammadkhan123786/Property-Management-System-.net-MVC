using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblSubCategory")]
    public class PropertySubCategories
    {
        [Key]
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Remarks { get; set; }

        public int? TopCategoryId { get; set; }



    }
}