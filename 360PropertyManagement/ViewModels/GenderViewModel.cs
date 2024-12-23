using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class GenderViewModel
    {
        [Required(ErrorMessage="Please Enter Gender!!")]
        public string GenderName { get; set; }
        [Required(ErrorMessage="Please select status")]
        public bool Status { get; set; }
        public int id { get; set; }

        public GenderViewModel()
        {
            GenderName = this.GenderName;
            Status = this.Status;
            id = this.id;

        }
        public GenderViewModel(Genders gender)
        {
            GenderName = gender.GenderName;
            Status = gender.Status;
            id = gender.GenderId;
        }


    }
}