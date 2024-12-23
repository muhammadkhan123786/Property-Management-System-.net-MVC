using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblGenders")]
    public class Genders
    {
        [Key]
        public int GenderId { get; set; }
        public string GenderName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

    }
}