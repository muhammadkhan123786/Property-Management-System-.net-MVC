using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblMessages")]
    public class SendMessages
    {
        [Key]
        public int MessageId { get; set; }

        public string MessageSubject { get; set; }
        
      }
}