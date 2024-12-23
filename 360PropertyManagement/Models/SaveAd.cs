using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblSaveAd")]
    public class SaveAd
    {
        [Key]
        public int SaveAdId { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }

        //virtual
        public int? AdId { get; set; }
        public int? AccountId { get; set; }

        public virtual SubmitedAds ad { get; set; }
        public virtual Accounts acc { get; set; }

    }
}