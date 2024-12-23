using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblContacts")]
    public class Contacts
    {
        [Key]
        public int ContactId { get; set; }
        public string MobileOne { get; set; }
        public string MobileTwo { get; set; }
        public string PhoneOne { get; set; }
        public string PhoneTwo { get; set; }
        public string Website { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? PersonId { get; set; }
        public int? AccountId { get; set; }

        public virtual Persons person { get; set; }
        public virtual Accounts account { get; set; }
    }
}