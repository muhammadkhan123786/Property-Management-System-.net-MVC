using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblProfiles")]
    public class Profiles
    {
        [Key]
        public int ProfileId { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? AddressId { get; set; }
        public int? ContactId { get; set; }
        public int? PersonId { get; set; }
        public int? AccountId { get; set; }

        public virtual Addresses address { get; set; }
        public virtual Contacts contact { get; set; }
        public virtual Persons person { get; set; }
        public virtual Accounts account { get; set; }

    }
}