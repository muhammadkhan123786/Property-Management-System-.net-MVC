using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _360PropertyManagement.Models;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class OccupationViewModel
    {
        [Required(ErrorMessage="Please Enter Occupation!")]
        public string OccupationName { get; set; }
        [Required(ErrorMessage="Please Select Status")]
        public bool Status { get; set; }
        public int occupid { get; set; }

        public OccupationViewModel()
        {
            OccupationName = this.OccupationName;
            Status = this.Status;
            occupid = this.occupid;
        }
        public OccupationViewModel(Occupations occup)
        {
            OccupationName = occup.OccupationName;
            Status = occup.Status;
            occupid = occup.OccupationId;
        }
    }
}