using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblPersons")]
    public class Persons
    {
        [Key]
        public int PersonId { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMiddleName { get; set; }
        public string PersonLastName { get; set; }
        [NotMapped]
        public virtual String PersonFullName
        {
            get
            {
                return PersonFirstName + " " + PersonMiddleName + " " + PersonLastName;
            }
        }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? GenderId { get; set; }
        public int? OccupationId { get; set; }
        public int? AccountId { get; set; }
        public int? ImageId { get; set; }

        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }
        public virtual Accounts account { get; set; }
        public virtual Images image { get; set; }
    }
}