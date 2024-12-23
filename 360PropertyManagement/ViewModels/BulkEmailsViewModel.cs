using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class BulkEmailsViewModel
    {
        [Required(ErrorMessage = "Please Enter Email Subject")]
        public string EmailSubject { get; set; }
        [Required(ErrorMessage = "Please Enter Email Details")]
        public string EmailMessage { get; set; }

    }
}