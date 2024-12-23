using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class PersonViewModel
    {
        [Required(ErrorMessage="Please Enter Person Name!!")]
        public string PersonFirstName { get; set; }
        public string personmiddlename { get; set; }
        public string personlastname { get; set; }
        [Required(ErrorMessage="Please select status!")]
        public bool Status { get; set; }

        [EmailAddress(ErrorMessage="Please Enter Valid Email Id")]
        public string EmailId { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Photo { get; set; }

        public string Password { get; set; }

        public string Confirmpassword { get; set; }

        [Required(ErrorMessage="Please Select Gender!")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage="Please Select Occupation!!")]
        public int? OccupationId { get; set; }
        public int? AccountId { get; set; }

        
        public int? ImageId { get; set; }

        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }
        public virtual Accounts account { get; set; }
        public virtual Images image { get; set; }

        public PersonViewModel()
        {
            PersonFirstName = this.PersonFirstName;
            personmiddlename = this.personmiddlename;
            personlastname = this.personlastname;
            Status = this.Status;
            GenderId = this.GenderId;
            OccupationId = this.OccupationId;
            ImageId = this.ImageId;
            EmailId = this.EmailId;
            Password = this.Password;
            Confirmpassword = this.Confirmpassword;
            Photo = this.Photo;
        }
        public PersonViewModel(Persons person)
        {
            PersonFirstName = person.PersonFirstName;
            personmiddlename = person.PersonMiddleName;
            personlastname = person.PersonLastName;
            Status = person.Status;
            GenderId = person.GenderId;
            OccupationId = person.OccupationId;
            EmailId = person.account.AccountEmailId;
            
        }

    }
}