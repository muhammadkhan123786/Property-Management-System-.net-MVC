using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class ProfileChangePaasswordViewmodel
    {
        [Required(ErrorMessage="Please Enter Previous Password")]
        public string PreviousPassword {get;set;}
        [Required(ErrorMessage="Please Enter Password")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Please Enter Repeat Password")]
        [Compare("NewPassword",ErrorMessage="Password and repeat password must match")]
        public string RepeatPassword { get; set; }
        
        
    }

}