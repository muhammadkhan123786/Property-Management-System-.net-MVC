using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class ProfileCustomContactViewModel
    {
        [Required(ErrorMessage = "Please Enter Mobile-One")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string MobileOne { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string MobileTwo { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string PhoneOne { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string PhoneTwo { get; set; }

        public string Website { get; set; }

        public bool Status { get; set; }

        public string emailid { get; set; }

        public int ContactInfoId { get; set; }

        public ProfileCustomContactViewModel()
        {
            MobileOne = this.MobileOne;
            MobileTwo = this.MobileTwo;
            PhoneOne = this.PhoneOne;
            PhoneTwo = this.PhoneTwo;
            Status = this.Status;
            Website = this.Website;
            emailid = this.emailid;
            ContactInfoId = this.ContactInfoId; 
        }

        public ProfileCustomContactViewModel(Contacts con)
        {
            MobileOne = con.MobileOne;
            MobileTwo = con.MobileTwo;
            PhoneOne = con.PhoneOne;
            PhoneTwo = con.PhoneTwo;
            Status = con.Status;
            Website = con.Website;
            emailid = con.account.AccountEmailId;
            ContactInfoId = con.ContactId;

        }
        
    }
}