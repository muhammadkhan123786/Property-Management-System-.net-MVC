using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblMessageReceiver")]
    public class MsgReceiver
    {
        [Key]
        public int MeesageReceiverId { get; set; }

        public bool IsRead { get; set; }

        public bool IsDeleted { get; set; }

        public int? AccountId { get; set; }
        public int? MessagesDetailsId { get; set; }
        public int? MessageSenderId { get; set; }

        public virtual Accounts acc { get; set; }
        public virtual MsgDetails msgdetails { get; set; }
        public virtual MsgSender msgsender { get; set; }
    }
}