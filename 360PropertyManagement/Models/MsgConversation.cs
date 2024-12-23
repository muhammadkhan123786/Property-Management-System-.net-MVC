using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblMsgsConversations")]
    public class MsgConversation
    {
        [Key]
        public int ConversationId { get; set; }

        public bool IsDeleted { get; set; }

        public int? MessageSenderId { get; set; }
        public int? MessagesDetailsId { get; set; }
        public int? MessageId { get; set; }

        public virtual MsgSender msgsender { get; set; }
        public virtual MsgDetails msgdetials { get; set; }
        public virtual SendMessages msg { get; set; }
    }
}