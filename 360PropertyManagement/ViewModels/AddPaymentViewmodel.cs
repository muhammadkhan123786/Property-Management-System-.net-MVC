using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class AddPaymentViewmodel
    {
        [Required(ErrorMessage = "Please Enter License Fee Per Year")]
        public decimal LicenseFeePerYear { get; set; }
        [Required(ErrorMessage = "Enter License for how many years")]
        public int? LicenseFeeForYears { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal DomainandHostingFee { get; set; }

        public decimal Discount { get; set; }

        public bool PaymentStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReceivingDate { get; set; }

        public AddPaymentViewmodel()
        {
            LicenseFeePerYear = this.LicenseFeePerYear;
            LicenseFeeForYears = this.LicenseFeeForYears;
            TotalPaid = this.TotalPaid;
            DomainandHostingFee = this.DomainandHostingFee;
            Discount = this.Discount;
            PaymentStatus = this.PaymentStatus;
            ReceivingDate = this.ReceivingDate;
        }
        
        public AddPaymentViewmodel(LicenseFees fee)
        {
            LicenseFeePerYear = fee.LicenseFeePerYear;
            LicenseFeeForYears = fee.LicenseFeeForYears;
            DomainandHostingFee = fee.DomainandHostingFee;
            Discount = fee.Discount;
            ReceivingDate = fee.ReceivingDate;
            PaymentStatus = fee.Status;


        }
    }
}
