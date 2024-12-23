using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class PasswordResetViewModel
    {
        [Required(ErrorMessage = "Please Enter Email_Id!!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email_Id")]
        public string AccountEmailId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password!")]
        public string AccountPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Repeat Password!")]
        [Compare("AccountPassword", ErrorMessage = "Password & Repeat Password Must Be Same!")]
        public string AccountConfirmPassword { get; set; }

        public string forgetpasswordlink { get; set; }
    }
}