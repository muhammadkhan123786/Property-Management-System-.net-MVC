using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace _360PropertyManagement.ViewModels
{
    public class AdsViewModel
    {
        public int PropertyId { get; set; }
        [Required(ErrorMessage = "Please Enter Length of 70 Character Property Title"), StringLength(70)]
        public string PropertyTitle { get; set; }
        [Required(ErrorMessage = "Please Enter Maximum Price")]
        public decimal MaximumPrice { get; set; }
        [Required(ErrorMessage = "Please Enter Minimum Price")]
        public decimal MinimumPrice { get; set; }
        [Required(ErrorMessage = "Please Select Is Property Furnished??")]
        public bool IsFurnished { get; set; }

        public decimal? SqrFoot { get; set; }
        [Required(ErrorMessage = "Please Enter Some Property Description")]
        public string PropertyDescription { get; set; }

        public bool IsActive { get; set; }

        public int? numberofviews { get; set; }

        [Required(ErrorMessage = "Please Enter Property Address")]
        public string Address { get; set; }
        
        public string MobileNumber { get; set; }
        
        public string PersonName { get; set; }

        public int addid { get; set; }

        public DateTime datentime { get; set; }

        public bool ContactInfo { get; set; }
        public bool PersonalInfo { get; set; }

        public string ZipCode { get; set; }
        public string SeoKeyWords { get; set; }
        public string Location { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PropertyPhotoOne { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PropertyPhotoTwo { get; set; }

        [DataType(DataType.Upload)]
        public HttpPostedFileBase PropertyPhotoThree { get; set; }


        
        public int? TopCategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? RoomsId { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? AccountId { get; set; }
    

        
        public virtual TopCategory topcategory { get; set; }
        public virtual PropertySubCategories subcategory { get; set; }
        public virtual PropertyRooms rooms { get; set; }
        public virtual Countries country { get; set; }
        public virtual PropertyAds propertyad { get; set; }
        public virtual States state { get; set; }
        public virtual Cities city { get; set; }
        public virtual Accounts acc { get; set; }
       
        public AdsViewModel()
        {
            PropertyTitle = this.PropertyTitle;
            PropertyDescription = this.PropertyDescription;
            PropertyId = this.PropertyId;
            RoomsId = this.RoomsId;
            TopCategoryId = this.TopCategoryId;
            SubCategoryId = this.SubCategoryId;
            CountryId = this.CountryId;
            StateId = this.StateId;
            CityId = this.CityId;
            AccountId = this.AccountId;
            MaximumPrice = this.MaximumPrice;
            MinimumPrice = this.MinimumPrice;
            IsFurnished = this.IsFurnished;
            SqrFoot = this.SqrFoot;
            numberofviews = this.numberofviews;
            Address = this.Address;
            addid = this.addid;
            MobileNumber = this.MobileNumber;
            PersonName = this.PersonName;
            ZipCode = this.ZipCode;
            SeoKeyWords = this.SeoKeyWords;
            Location = this.Location;
            datentime = this.datentime;
        }

        public AdsViewModel(SubmitedAds propertyad)
        {
            PropertyTitle = propertyad.propertyad.PropertyTitle;
            PropertyDescription = propertyad.propertyad.PropertyDescription;
            PropertyId = propertyad.propertyad.PropertyId;
            RoomsId = propertyad.propertyad.RoomsId;
            datentime = propertyad.DateNTime;
            addid = propertyad.AdId;
            AccountId = propertyad.account.AccountId;
            numberofviews = propertyad.NumberOfViews;
            TopCategoryId = propertyad.propertyad.TopCategoryId;
            SubCategoryId = propertyad.propertyad.SubCategoryId;
            CountryId = propertyad.areaads.CountryId;
            StateId = propertyad.areaads.StateId;
            CityId = propertyad.areaads.CityId;
            MaximumPrice =Convert.ToDecimal(propertyad.propertyad.MaximumPrice);
            MinimumPrice = Convert.ToDecimal(propertyad.propertyad.MinimumPrice);
            IsFurnished = propertyad.propertyad.IsFurnished;
            SqrFoot = propertyad.propertyad.SqrFoot;
            Address = propertyad.propertyad.Address;
            MobileNumber = propertyad.propertyad.ContactNumber;
            PersonName = propertyad.propertyad.PersonName;
            ZipCode = propertyad.areaads.ZipCode;
            SeoKeyWords = propertyad.areaads.SeoKeyWords;
            Location = propertyad.areaads.Location;
        }
    }
}