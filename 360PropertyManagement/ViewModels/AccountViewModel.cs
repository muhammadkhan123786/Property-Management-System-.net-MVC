using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;

namespace _360PropertyManagement.ViewModels
{
    public class AccountViewModel
    {
        [Required(ErrorMessage="Please Enter Email_Id!!")]
        [EmailAddress(ErrorMessage="Please Enter Valid Email_Id")]
        public string AccountEmailId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Please Enter Password!")]
        public string AccountPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage="Please Enter Repeat Password!")]
        [Compare("AccountPassword",ErrorMessage="Password & Repeat Password are not same please check!")]
        public string AccountConfirmPassword { get; set; }
        public bool? Isactive;
        [Required(ErrorMessage="Please Select Role")]
        public int? RoleId { get; set; }
        public virtual Roles role { get; set; }
        public AccountViewModel(Accounts acc)
        {
            AccountEmailId = acc.AccountEmailId;
            Isactive = acc.IsActive;
            AccountPassword = acc.AccountPassword;
            RoleId = acc.role.RoleId;
        }
        
        public AccountViewModel()
        {
            AccountEmailId = this.AccountEmailId;
            AccountPassword = this.AccountPassword;
            AccountConfirmPassword = this.AccountConfirmPassword;
            RoleId = this.RoleId;
        }
    }
}