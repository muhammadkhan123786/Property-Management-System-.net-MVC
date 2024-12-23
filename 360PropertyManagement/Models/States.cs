using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblStates")]
    public class States
    {
        [Key]
        public int StateId { get; set; }
        public string StateName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? CountryId { get; set; }
        public virtual Countries country { get; set; }
    }
}