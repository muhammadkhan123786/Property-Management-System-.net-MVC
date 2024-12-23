using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class EditPersonWithoutAccount
    {
        [Required(ErrorMessage = "Please Enter Person Name!!")]
        public string PersonFirstName { get; set; }
        public string personmiddlename { get; set; }
        public string personlastname { get; set; }
        [Required(ErrorMessage = "Please select status!")]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Please Select Gender!")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage = "Please Select Occupation!!")]
        public int? OccupationId { get; set; }

        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }

        public EditPersonWithoutAccount()
        {
            PersonFirstName = this.PersonFirstName;
            personmiddlename = this.personmiddlename;
            personlastname = this.personlastname;
            GenderId = this.GenderId;
            OccupationId = this.OccupationId;
            Status = this.Status;

        }
        public EditPersonWithoutAccount(Persons person)
        {
            PersonFirstName = person.PersonFirstName;
            personmiddlename = person.PersonMiddleName;
            personlastname = person.PersonLastName;
            GenderId = person.GenderId;
            OccupationId = person.OccupationId;
            Status = person.Status;
        }
    }
}