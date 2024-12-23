using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;
namespace _360PropertyManagement.ViewModels
{
    public class LicenseEditViewModel
    {
        [Required(ErrorMessage = "Please Select License Status")]
        public bool Status { get; set; }
        [Required(ErrorMessage="Please Enter Expiry Date.")]
        public DateTime ExpireOn { get; set; }
        public string SalogonStatement { get; set; }
        public LicenseEditViewModel(LicenseProduct license)
        {
            Status = license.Status;
            ExpireOn = license.ExpireOn;
            SalogonStatement = license.SalogonStatement;
        }
        public LicenseEditViewModel()
        {
            Status = this.Status;
            ExpireOn = this.ExpireOn;
            SalogonStatement = this.SalogonStatement;
        }
    }
}