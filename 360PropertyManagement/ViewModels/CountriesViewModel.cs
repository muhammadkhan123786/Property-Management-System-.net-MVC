using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;

namespace _360PropertyManagement.ViewModels
{
    public class CountriesViewModel
    {
        [Required(ErrorMessage="Please Enter Country Name")]
        public string CountryName { get; set; }
        [Required(ErrorMessage="Please select status!!")]
        public bool Status { get; set; }

        public CountriesViewModel()
        {
            CountryName = this.CountryName;
            Status = this.Status;
        }

        public CountriesViewModel(Countries country)
        {
            CountryName = country.CountryName;
            Status = country.Status;
        }

    }
}