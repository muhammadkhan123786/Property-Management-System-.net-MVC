using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblMsgdetails")]
    public class MsgDetails
    {
        [Key]
        public int MessagesDetailsId { get; set; }

        public string MessageDetails { get; set; }

        public DateTime DateNTime { get; set; }

        public bool IsDeleted { get; set; }

        public int? MessageId { get; set; }

        public virtual SendMessages msg { get; set; }
    }
}