using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblCities")]
    public class Cities
    {
        [Key]
        public int CityId { get; set; }
        public string CityName { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }

        public int? StateId { get; set; }
        public int? CountryId { get; set; }


        public virtual States state { get; set; }
        public virtual Countries country { get; set; }
    }
}