using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblImageNotExists")]
    public class ImageNot
    {
        [Key]
        public int ImageNotExistsId { get; set; }
        public byte[] ImageNotAvailable { get; set; }
        public bool? Status { get; set; }

    }
}