using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class CityViewModel
    {
        [Required(ErrorMessage="Please Enter City Name")]
        public string CityName { get; set; }
        [Required(ErrorMessage="Please Select Status")]
        public bool Status { get; set; }

        [Required(ErrorMessage="Please Select Country")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage="Please Select State")]
        public int? StateId { get; set; }

        public virtual Countries country { get; set; }
        public virtual States state { get; set; }

        public CityViewModel()
        {
            CityName = this.CityName;
            Status = this.Status;
            CountryId = this.CountryId;
            StateId = this.StateId;
        }
        public CityViewModel(Cities city)
        {
            CityName = city.CityName;
            Status = city.Status;
            CountryId = city.CountryId;
            StateId = city.StateId;
        }
    }
}