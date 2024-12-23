using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;
using System.Web.Mvc;

namespace _360PropertyManagement.ViewModels
{
    public class ContactViewModel
    {
        //Contact Info 
        [Required(ErrorMessage="Please Enter Mobile-One")]
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

        //person info
        [Required(ErrorMessage="Please Enter Firt name")]
        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public string lastname { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Photo { get; set; }
        //Account Info
        [Required(ErrorMessage="Account Email Id is required! If Email not avaiable please type(unknown@unknown.com)")]
        [EmailAddress(ErrorMessage="Please Enter Valid Email")]
        public string Email_Id { get; set; }

        [Required(ErrorMessage="Please Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage="Please Enter Confirm Password")]
        
        public string Confimpassword { get; set; }

        //virtually values
        [Required(ErrorMessage="Select Gender")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage="Select Occupation")]
        public int? OccupationId { get; set; }

        public virtual Genders gender { get; set; }

        public virtual Occupations occupation { get; set; }
         
        public ContactViewModel(Contacts con)
        {
            MobileOne = con.MobileOne;
            MobileTwo = con.MobileTwo;
            PhoneOne = con.PhoneOne;
            PhoneTwo = con.PhoneTwo;
            Website = con.Website;
            Status = con.Status;
            
        }

        public ContactViewModel(Persons person)
        {
            Firstname = person.PersonFirstName;
            Secondname = person.PersonMiddleName;
            lastname = person.PersonLastName;
            GenderId = person.GenderId;
            OccupationId = person.OccupationId;
           
        }
        
        public ContactViewModel()
        {
            Firstname = this.Firstname;
            Secondname = this.Secondname;
            lastname = this.lastname;
            Website = this.Website;
            PhoneTwo = this.PhoneTwo;
            PhoneOne = this.PhoneOne;
            MobileOne = this.MobileOne;
            MobileTwo = this.MobileTwo;
            Email_Id = this.Email_Id;
            Status = this.Status;
            GenderId = this.GenderId;
            OccupationId = this.OccupationId;
            Photo = this.Photo;
            Password = this.Password;
            Confimpassword = this.Confimpassword; 
        }

    }
}