using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _360PropertyManagement.Models;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LicensePersonViewModel
    {
        [Required(ErrorMessage = "Please Enter Firt name")]
        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public string lastname { get; set; }

        public string PersonGenderName { get; set; }

        public string PersonOccupationName { get; set; }

        [Required(ErrorMessage = "Please Select Gender!")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage = "Please Select Occupation!!")]
        public int? OccupationId { get; set; }

        public int PersonId { get; set; }

        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }

        public LicensePersonViewModel()
        {
            Firstname = this.Firstname;
            Secondname = this.Secondname;
            lastname = this.lastname;
            GenderId = this.GenderId;
            OccupationId = this.OccupationId;
            PersonId = this.PersonId;
        }

        public LicensePersonViewModel(Persons person)
        {
            Firstname = person.PersonFirstName;
            Secondname = person.PersonMiddleName;
            lastname = person.PersonLastName;
            PersonId = person.PersonId;
            PersonGenderName = person.gender.GenderName;
            PersonOccupationName = person.occupation.OccupationName;
            GenderId = person.GenderId;
            OccupationId = person.OccupationId;
        }
    }
}