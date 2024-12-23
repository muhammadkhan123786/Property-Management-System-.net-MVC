using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class LicenseViewModel
    {
        //Account details
        [Required(ErrorMessage = "Please Enter Email_Id!!")]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email_Id")]
        public string AccountEmailId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Password!")]
        public string AccountPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter Repeat Password!")]
        [Compare("AccountPassword", ErrorMessage = "Password & Repeat Password are not same please check!")]
        public string AccountConfirmPassword { get; set; }

        public bool? Isactive;

        //Contact Details
        [Required(ErrorMessage = "Please Enter Mobile-One")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string MobileOne { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string MobileTwo { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string PhoneOne { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers Are Allowed")]
        public string PhoneTwo { get; set; }

        public string Website { get; set; }

        

        //person info
        [Required(ErrorMessage = "Please Enter Firt name")]
        public string Firstname { get; set; }

        public string Secondname { get; set; }

        public string lastname { get; set; }

        [Required(ErrorMessage="Please Upload Person Image")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Photo { get; set; }
        
        [Required(ErrorMessage = "Please Select Gender!")]
        public int? GenderId { get; set; }
        [Required(ErrorMessage = "Please Select Occupation!!")]
        public int? OccupationId { get; set; }



        public virtual Genders gender { get; set; }
        public virtual Occupations occupation { get; set; }

        //address details
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
    
        //License Details
        [Required(ErrorMessage="Please Enter Your Business Name")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage="Please Enter About Us Details")]
        public string AboutUs { get; set; }
        [Required(ErrorMessage = "Please Enter Mission Statement Details")]
        public string MissionStatement { get; set; }
        [Required(ErrorMessage = "Please Enter Vision Details")]
        public string VisionStatement { get; set; }
        [Required(ErrorMessage="Please Select License Status")]
        public bool Status { get; set; }
        [Required(ErrorMessage="Please Upload Your Logo")]
        
        [DataType(DataType.Upload)]
        public HttpPostedFileBase CompanyLogo { get; set; }

        public string SalogonStatement { get; set; }

        //License Fee
        [Required(ErrorMessage="Please Enter License Fee Per Year")]
        public decimal LicenseFeePerYear { get; set; }
        [Required(ErrorMessage = "Enter License for how many years")]
        public int? LicenseFeeForYears { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal DomainandHostingFee { get; set; }

        public decimal AnnualFee { get; set; }

        public decimal Discount { get; set; }

        public bool PaymentStatus { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReceivingDate { get; set; }

        public int? LicenseId { get; set; }

        public virtual LicenseProduct License { get; set; }
 
    }
}