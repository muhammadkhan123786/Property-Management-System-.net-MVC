using System;
using System.Collections.Generic;
using _360PropertyManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class EditAddressViewModel
    {
        [Required(ErrorMessage = "Please Enter Address-One")]
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }

        [Required(ErrorMessage = "Please Select Your Country")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "Please Select Your City")]
        public int? CityId { get; set; }
        [Required(ErrorMessage = "Please Select State")]
        public int? StateId { get; set; }

        public bool Status { get; set; }

        public virtual Countries country { get; set; }
        public virtual Cities city { get; set; }
        public virtual States state { get; set; }

        public EditAddressViewModel()
        {
            AddressOne = this.AddressOne;
            AddressTwo = this.AddressTwo;
            Status = this.Status;
            CountryId = this.CountryId;
            StateId = this.StateId;
            CityId = this.CityId;

        }
        public EditAddressViewModel(Addresses address)
        {
            AddressOne = address.AddressOne;
            AddressTwo = address.AddressTwo;
            Status = address.Status;
            CountryId = address.CountryId;
            StateId = address.StateId;
            CityId = address.CityId;
        }
    }
}