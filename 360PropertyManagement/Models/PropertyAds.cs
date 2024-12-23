using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblNewProperty")]
    public class PropertyAds
    {
        [Key]
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; }
        public decimal? MaximumPrice { get; set; }
        public decimal? MinimumPrice { get; set; }
        public bool IsFurnished { get; set; }
        public decimal SqrFoot { get; set; }
        public string PropertyDescription { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string PersonName { get; set; }
      

        //Virtually Models
        public int? ImageId { get; set; }
        public int? TopCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? RoomsId { get; set; }

        public virtual Images image { get; set; }
        public virtual TopCategory topcategory { get; set; }
        public virtual PropertySubCategories subcategory { get; set; }
        public virtual PropertyRooms rooms { get; set; }


    }
}