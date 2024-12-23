using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblImages")]
    public class Images
    {
        [Key]
        public int ImageId { get; set; }
        public byte[] Image { get; set; }
        public bool IsDeleted { get; set; }

        public int? AdId { get; set; }

        public virtual SubmitedAds submitad { get; set; }
    }
}