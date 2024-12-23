using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblLicenseFees")]
    public class LicenseFees
    {
        [Key]
        public int LicenseFeeId { get; set; }

        public decimal LicenseFeePerYear { get; set; }
        public int? LicenseFeeForYears { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal DomainandHostingFee { get; set; }
        public decimal Discount { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime ReceivingDate { get; set; }

        public DateTime EnteringDate { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? LicenseId { get; set; }

        public virtual LicenseProduct License { get; set; }

    }
}