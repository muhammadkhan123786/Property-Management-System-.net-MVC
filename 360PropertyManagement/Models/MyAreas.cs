using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblMyAreas")]
    public class MyAreas
    {
        [Key]
        public int MyAreaId { get; set; }
        public string Location { get; set; }
        public DateTime DateNTime { get; set; }
        public bool IsDeleted { get;set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string ZipCode { get; set; }

        //virtual 
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? RoleId { get; set; }
        public int? AccountId { get; set; }

        public virtual Countries country { get; set; }
        public virtual States state { get; set; }
        public virtual Cities city { get; set; }
        public virtual Roles role { get; set; }
        public virtual Accounts account { get; set; }
    }
}