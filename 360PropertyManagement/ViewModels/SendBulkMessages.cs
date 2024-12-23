using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class SendBulkMessages
    {
        [Required(ErrorMessage = "Please Enter message Subject")]
        public string MessageSubject { get; set; }
        [Required(ErrorMessage = "Please Enter message Details")]
        public string Message { get; set; }
    }
}