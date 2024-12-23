using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class AddViewModel
    {
        [Required(ErrorMessage="Please Enter Country Name")]
        public string CountryName { get; set; }
        [Required(ErrorMessage="Please Select Status")]
        public bool? Status { get; set; }
    }
}