using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class MsgReplyViewModle
    {
        
        public string MsgDetails { get; set; }
        public int msgid { get; set; }
        public string msgsubject { get; set; }
        public int accountid { get; set; }
        

        public MsgReplyViewModle()
        {
            MsgDetails = this.MsgDetails;
            msgid = this.msgid;
            msgsubject = this.msgsubject;
            accountid = this.accountid;
            
        }
        public MsgReplyViewModle(SendMessages msg)
        {
            msgid = msg.MessageId;
            msgsubject = msg.MessageSubject;
           
            
        }
    }
}