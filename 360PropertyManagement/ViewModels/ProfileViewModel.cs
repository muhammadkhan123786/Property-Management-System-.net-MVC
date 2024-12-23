using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage="Please Enter Your First Name")]
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        [Required(ErrorMessage="Please select status of personal info!")]
        public bool? PersonalInfoStatus { get; set; }

        [Required(ErrorMessage="Please Select Gender!")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage="Please Select Occupation!")]
        public int? OccupationId { get; set; }
        public int? ImageId { get; set; }

        [Required(ErrorMessage="Please select address-one!")]
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        [Required(ErrorMessage="Please select status of your address!")]
        public bool? AddressStatus { get; set; }

        [Required(ErrorMessage="Please select your country!!")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage="Please select City")]
        public int? CityId { get; set; }
        [Required(ErrorMessage="Please select your state!!")]
        public int? StateId { get; set; }
       
        [Required(ErrorMessage="Please Enter Mobile-One")]
        public string MobileOne { get; set; }
        public string MobileTwo { get; set; }
        public string PhoneOne { get; set; }
        public string PhoneTwo { get; set; }
        [EmailAddress(ErrorMessage="Please Enter Valid Email-Id")]
        public string Email_Id { get; set; }
        public string Website { get; set; }
        [Required(ErrorMessage="Please select status of contact info!")]
        public bool? ContactInfoStatus { get; set; }

       


    }
}