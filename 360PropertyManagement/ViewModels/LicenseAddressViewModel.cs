using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LicenseAddressViewModel
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


        public virtual Countries country { get; set; }
        public virtual Cities city { get; set; }
        public virtual States state { get; set; }
    
    }
}