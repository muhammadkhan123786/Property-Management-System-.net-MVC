using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LicenseContactInfo
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

        public int ContactId { get; set; }

        public LicenseContactInfo()
        {
            MobileOne = this.MobileOne;
            MobileTwo = this.MobileTwo;
            PhoneOne = this.PhoneOne;
            PhoneTwo = this.PhoneTwo;
            Website = this.Website;
            ContactId = this.ContactId;

        }

        public LicenseContactInfo(Contacts con)
        {
            MobileOne = con.MobileOne;
            MobileTwo = con.MobileTwo;
            PhoneOne = con.PhoneOne;
            PhoneTwo = con.PhoneTwo;
            Website = con.Website;
            ContactId = con.ContactId;
            
        }
    }
}