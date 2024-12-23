using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblCountries")]
    public class Countries
    {
        [Key]
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

      //  public virtual ICollection<Accounts> accounts { get; set; }
    }
}