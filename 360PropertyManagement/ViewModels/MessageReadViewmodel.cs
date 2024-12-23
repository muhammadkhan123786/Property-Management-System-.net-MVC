using System;
using System.Collections.Generic;
using _360PropertyManagement.Models;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class MessageReadViewmodel
    {
        public string MessageDetails { get; set; }

        public string msgsubject { get; set; }

        public string replytext { get; set; }

        public DateTime DateNTime { get; set; }

        public int accountid { get; set; }

        public int msgid { get; set; }

        public int senderid { get; set; }

        public MessageReadViewmodel()
        {
            MessageDetails = this.MessageDetails;
            DateNTime = this.DateNTime;
            msgid = this.msgid;
            msgsubject = this.msgsubject;
            senderid = this.senderid;
            replytext = this.replytext;
        }

        public MessageReadViewmodel(MsgReceiver msg)
        {
            MessageDetails = msg.msgdetails.MessageDetails;
            msgsubject = msg.msgdetails.msg.MessageSubject;
            DateNTime = msg.msgdetails.DateNTime;
            msgid =Convert.ToInt32(msg.msgdetails.MessageId);
            senderid = Convert.ToInt32(msg.MessageSenderId);
            accountid =Convert.ToInt32(msg.AccountId);
        }

    }
}