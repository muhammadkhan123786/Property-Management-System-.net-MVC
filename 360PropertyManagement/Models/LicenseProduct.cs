using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblProductLicense")]
    public class LicenseProduct
    {
        [Key]
        public int LicenseId { get; set; }
        public string CompanyName { get; set; }
        public string AboutUs { get; set; }
        public string MissionStatement { get; set; }
        public decimal AnnualFee { get; set; }
        public string VisionStatement { get; set; }
        public string SalogonStatement { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Createdon { get; set; }
        public DateTime ExpireOn { get; set; }
       
        [NotMapped]
        public virtual String LicenseTitle
        {
            get
            {
                return CompanyName + "" + LicenseId.ToString();
            }
        }
       

        public int? AccountId { get; set; }
        public int? AddressId { get; set; }
        public int? ContactId { get; set; }
        public int? ImageId { get; set; }
        public int? PersonId { get; set; }


        public virtual Accounts account { get; set; }
        public virtual Addresses address { get; set; }
        public virtual Contacts contact { get; set; }
        public virtual Images image { get; set; }
        public virtual Persons person { get; set; }

    }
}