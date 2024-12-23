using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace _360PropertyManagement.ViewModels
{
    public class StateViewModel
    {
        [Required(ErrorMessage="Please Enter State Name")]
        public string satename { get; set; }
        [Required(ErrorMessage="Please Select State Status")]
        public bool satus { get; set; }

        [Required(ErrorMessage="Please Select Country")]
        public int? CountryId { get; set; }

        public virtual Countries country { get; set; }

        public StateViewModel()
        {
            satename = this.satename;
            satus = this.satus;
            CountryId = this.CountryId;
        }
        public StateViewModel(States state)
        {
            satename = state.StateName;
            satus = state.Status;
            CountryId = state.CountryId;
        }

    }
}