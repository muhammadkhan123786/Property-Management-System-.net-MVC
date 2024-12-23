using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class AdsFilterViewModel
    {
        public int? CategoryId { get; set; }
        public int? SubCategory { get; set; }
        public int? NumberOfRooms { get; set; }
        public bool IsFurnished { get; set; }
        public decimal minimumSqr { get; set; }
        public decimal maximumSqr { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string Location { get; set; }
        public string ZipCode { get; set; }
        public string AdTitle { get; set; }
    }
}