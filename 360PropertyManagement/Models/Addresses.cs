using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblAddresses")]
    public class Addresses
    {
        [Key]
        public int AddressId { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public int? StateId { get; set; }
        public int? PersonId { get; set; }
        public int? AccountId { get; set; }

        public virtual Countries country { get; set; }
        public virtual Cities city { get; set; }
        public virtual States state { get; set; }
        public virtual Persons person { get; set; }
        public virtual Accounts account { get; set; }

    }
}