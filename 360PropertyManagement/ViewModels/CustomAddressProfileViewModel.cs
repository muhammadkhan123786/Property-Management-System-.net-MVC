using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class CustomAddressProfileViewModel
    {
        [Required(ErrorMessage = "Please Enter Address-One")]
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }

        public int addid { get; set; }

        public bool Status { get; set; }

        [Required(ErrorMessage = "Please Select Your Country")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "Please Select Your City")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "Please Select State")]
        public int? StateId { get; set; }

        public virtual Countries country { get; set; }
        public virtual Cities city { get; set; }
        public virtual States state { get; set; }

        public CustomAddressProfileViewModel()
        {
            AddressOne = this.AddressOne;
            AddressTwo = this.AddressTwo;
            Status = this.Status;
            CountryId = this.CountryId;
            StateId = this.StateId;
            CityId = this.CityId;
            addid = this.addid;
            CountryName = this.CountryName;
            StateName = this.StateName;
            CityName = this.CityName;
        }
        public CustomAddressProfileViewModel(Addresses address)
        {
            AddressOne = address.AddressOne;
            AddressTwo = address.AddressTwo;
            Status = address.Status;
            CountryName = address.country.CountryName;
            StateName = address.state.StateName;
            CityName = address.city.CityName;
            CountryId = address.CountryId;
            StateId = address.StateId;
            CityId = address.CityId;
            addid = address.AddressId;
        }
    }
}