using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _360PropertyManagement.Models;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class SendMessageViewModel
    {
        [Required(ErrorMessage = "Please Enter message Subject")]
        public string MessageSubject { get; set; }
        [Required(ErrorMessage = "Please Enter message Details")]
        public string Message { get; set; }
        public bool Read { get; set; }

        [Required(ErrorMessage = "Please Select Account to send message")]
        public int? AccountId { get; set; }
        
        public int? RoleId { get; set; }

        public int? MessageId { get; set; }

        public virtual Accounts acc { get; set; }
        public virtual Roles role { get; set; }
        public virtual SendMessages sendmsg { get; set; }

        public SendMessageViewModel()
        {
            MessageSubject = this.MessageSubject;
            Message = this.Message;
            Read = this.Read;
            AccountId = this.AccountId;
            RoleId = this.RoleId;
            MessageId = this.MessageId;
        }

        public SendMessageViewModel(MsgDetails msg)
        {
            MessageSubject = msg.msg.MessageSubject;
            Message = msg.MessageDetails;


           
        }

    }
}