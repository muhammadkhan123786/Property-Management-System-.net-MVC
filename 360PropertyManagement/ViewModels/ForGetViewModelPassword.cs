using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class ForGetViewModelPassword
    {
        [Required(ErrorMessage = "Please Enter Email Id")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Id")]
        public string emailId { get; set; }

    }
}