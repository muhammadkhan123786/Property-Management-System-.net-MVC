using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class AddressViewModel
    {
        [Required(ErrorMessage="Please Enter Address-One")]
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }

        [Required(ErrorMessage = "Please Enter Person First Name")]
        public string PersonFirstName { get; set; }

        public string PersonMiddleName { get; set; }

        public string PersonLastName { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase Photo { get; set; }
        


        public bool Status { get; set; }

        [Required(ErrorMessage="Please Select Your Country")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage="Please Select Your City")]
        public int? CityId { get; set; }
        [Required(ErrorMessage="Please Select State")]
        public int? StateId { get; set; }

        [Required(ErrorMessage = "Please Select Gender")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage = "Please Select Occupation")]
        public int? OccupationId { get; set; }


        public virtual Countries country { get; set; }
        public virtual Cities city { get; set; }
        public virtual States state { get; set; }
        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }

        public int id { get; set; }
        
        public AddressViewModel()
        {
            AddressOne = this.AddressOne;
            AddressTwo = this.AddressTwo;
            PersonFirstName = this.PersonFirstName;
            PersonMiddleName = this.PersonMiddleName;
            PersonLastName = this.PersonLastName;
            Status = this.Status;
            id = this.id;
            CountryId = this.CountryId;
            CityId = this.CityId;
            StateId = this.StateId;
            GenderId = this.GenderId;
            OccupationId = this.OccupationId;

        }

        public AddressViewModel(Addresses address)
        {
            AddressOne = address.AddressOne;
            AddressTwo = address.AddressTwo;
            PersonFirstName = address.person.PersonFirstName;
            PersonMiddleName = address.person.PersonMiddleName;
            PersonLastName = address.person.PersonLastName;
            Status = address.Status;
            CountryId = address.CountryId;
            StateId = address.StateId;
            CityId = address.CityId;
            GenderId = address.person.GenderId;
            OccupationId = address.person.OccupationId;
            id = address.AddressId;
        }

    }
}