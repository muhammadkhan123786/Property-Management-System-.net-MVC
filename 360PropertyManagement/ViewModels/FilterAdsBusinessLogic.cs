using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;

namespace _360PropertyManagement.ViewModels
{
    public class FilterAdsBusinessLogic
    {
        private Context db;
        public FilterAdsBusinessLogic()
        {
            db = new Context();
        }
        public IQueryable<SubmitedAds> Getads(AdsFilterViewModel searchModel)
        {
            var result = db.submitedads.AsQueryable().Where(x=>x.IsDeleted==false&&x.IsActive==true);
            if (searchModel != null)
            {
                if (searchModel.CategoryId.HasValue)
                    result = result.Where(x => x.propertyad.TopCategoryId == searchModel.CategoryId);
                if (searchModel.SubCategory.HasValue)
                    result = result.Where(x => x.propertyad.SubCategoryId == searchModel.SubCategory);
                if (searchModel.NumberOfRooms.HasValue)
                    result = result.Where(x => x.propertyad.RoomsId == searchModel.NumberOfRooms);
                if (searchModel.CountryId.HasValue)
                    result = result.Where(x => x.areaads.CountryId == searchModel.CountryId);
                if (searchModel.StateId.HasValue)
                    result = result.Where(x => x.areaads.StateId == searchModel.StateId);
                if (searchModel.CityId.HasValue)
                    result = result.Where(x => x.areaads.CityId == searchModel.CityId);
                if (searchModel.IsFurnished==true)
                    result = result.Where(x => x.propertyad.IsFurnished==true);
                if (searchModel.IsFurnished == false)
                    result = result.Where(x => x.propertyad.IsFurnished == false);
                if (!string.IsNullOrEmpty(searchModel.Location))
                    result = result.Where(x => x.areaads.Location.Contains(searchModel.Location)||x.propertyad.Address.Contains(searchModel.Location));
                if (!string.IsNullOrEmpty(searchModel.ZipCode))
                    result = result.Where(x => x.areaads.ZipCode.Contains(searchModel.ZipCode));
                if (!string.IsNullOrEmpty(searchModel.AdTitle))
                    result = result.Where(x => x.propertyad.PropertyTitle.Contains(searchModel.AdTitle));
                if (searchModel.PriceFrom.HasValue)
                    result = result.Where(x => x.propertyad.MaximumPrice >= searchModel.PriceFrom);
                if (searchModel.PriceTo.HasValue)
                    result = result.Where(x => x.propertyad.MaximumPrice <= searchModel.PriceTo);
            }
            return result;
        }
    }
}