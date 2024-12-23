using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblLocationsFilterAds")]
    public class MyAreasAdss
    {
        [Key]
        public int MyAreaId { get; set; }
        public string ZipCode { get; set; }
        public string SeoKeyWords { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFlexible { get; set; }
        public string Location { get; set; }

        //Virtual 
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? AccountId { get; set; }

        //virtual classes
        public virtual Countries country { get; set; }
        public virtual States state { get; set; }
        public virtual Cities city { get; set; }
        public virtual Accounts acc { get; set; }


    }
}