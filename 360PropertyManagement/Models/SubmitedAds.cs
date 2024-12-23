using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblPropertyAds")]
    public class SubmitedAds
    {
        [Key]
        public int AdId { get; set; }
        public DateTime DateNTime { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? NumberOfViews { get; set; }

        //virtually 
        public int? AccountId { get; set; }
        public int? PropertyId { get; set; }
        public int? MyAreaId { get; set; }

        public virtual Accounts account { get; set; }
        public virtual PropertyAds propertyad { get; set; }
        public virtual MyAreasAdss areaads { get; set; }

    }
}