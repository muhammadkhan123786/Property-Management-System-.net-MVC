using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage="Please Enter Email Id!!")]
        [EmailAddress(ErrorMessage="Please Enter Valid Email-Id")]
        public string EmailId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Please Enter Password")]
        public string Password { get; set; }
    }
}