using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblOccupations")]
    public class Occupations
    {
        [Key]
        public int OccupationId { get; set; }
        public string OccupationName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

    }
}