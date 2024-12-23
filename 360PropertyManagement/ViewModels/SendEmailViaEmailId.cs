using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class SendEmailViaEmailId
    {
        [Required(ErrorMessage="Please Enter Email Id")]
        public string emailid { get; set; }
        public string ccemailid{get;set;}
        public string bccemailid{get;set;}
        [Required(ErrorMessage="Please Enter Email Subject")]
        public string emailsubject { get; set; }
        [Required(ErrorMessage="Please Enter Email Message")]
        public string emailmsg { get; set; }

    }
}