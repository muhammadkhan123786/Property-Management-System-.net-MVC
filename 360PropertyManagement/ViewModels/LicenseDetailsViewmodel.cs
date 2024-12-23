using System;
using System.Collections.Generic;
using _360PropertyManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LicenseDetailsViewmodel
    {
        [Required(ErrorMessage = "Please Enter Email_Id!!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email_Id")]
        public string AccountEmailId { get; set; }

        public string Website { get; set; }

        public decimal AnnualFee { get; set; }

        [Required(ErrorMessage = "Please Enter Your Business Name")]
        public string CompanyName { get; set; }

        public string SalogonStatement { get; set; }

        public DateTime NextDueDate { get; set; }

        public DateTime LicenseCreatedOn { get; set; }

        public bool LicenseStatus { get; set; }

        public string LicenseNumber { get; set; }

        public LicenseDetailsViewmodel()
        {
            AccountEmailId = this.AccountEmailId;
            Website = this.Website;
            AnnualFee = this.AnnualFee;
            CompanyName = this.CompanyName;
            SalogonStatement = this.SalogonStatement;
            NextDueDate = this.NextDueDate;
            LicenseCreatedOn = this.LicenseCreatedOn;
            LicenseStatus = this.LicenseStatus;
            LicenseNumber = this.LicenseNumber;

        }

        public LicenseDetailsViewmodel(LicenseProduct licese)
        {
            AccountEmailId = licese.account.AccountEmailId;
            Website = licese.contact.Website;
            AnnualFee = licese.AnnualFee;
            CompanyName = licese.CompanyName;
            SalogonStatement = licese.SalogonStatement;
            NextDueDate = licese.ExpireOn;
            LicenseCreatedOn = licese.Createdon;
            LicenseStatus = licese.Status;
            LicenseNumber = licese.LicenseTitle;

        }

    }
}