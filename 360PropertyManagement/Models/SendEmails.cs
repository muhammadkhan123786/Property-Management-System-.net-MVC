using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblSentEmails")]
    public class SendEmails
    {
        [Key]
        public int SendEmailId { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public DateTime SendOnDate { get; set; }
        public string EmailId { get; set; }
        public bool IsDeleted { get; set; }

        public int? AccountId { get; set; }
        public virtual Accounts account { get; set; }
    }
}