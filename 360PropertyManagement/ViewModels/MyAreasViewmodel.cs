using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class MyAreasViewmodel
    {
        [Key]
        public int MyAreaId { get; set; }
        public string Location { get; set; }
        public DateTime DateNTime { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public string ZipCode { get; set; }

        //virtual 
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public int? RoleId { get; set; }
        public int? AccountId { get; set; }

        public virtual Countries country { get; set; }
        public virtual States state { get; set; }
        public virtual Cities city { get; set; }
        public virtual Roles role { get; set; }
        public virtual Accounts account { get; set; }

        public MyAreasViewmodel()
        {
            IsActive = this.IsActive;
            Remarks = this.Remarks;
            ZipCode = this.ZipCode;
            CountryId = this.CountryId;
            StateId = this.StateId;
            CityId = this.CityId;
            RoleId = this.RoleId;
            AccountId = this.AccountId;
            DateNTime = this.DateNTime;
            IsDeleted = this.IsDeleted;
            MyAreaId = this.MyAreaId;
            Location = this.Location;

        }
        public MyAreasViewmodel(MyAreas viewmodel)
        {
            IsActive = viewmodel.IsActive;
            Remarks = viewmodel.Remarks;
            ZipCode = viewmodel.ZipCode;
            CountryId = viewmodel.CountryId;
            StateId = viewmodel.StateId;
            CityId = viewmodel.CityId;
            RoleId = viewmodel.RoleId;
            AccountId = viewmodel.AccountId;
            DateNTime = viewmodel.DateNTime;
            IsDeleted = viewmodel.IsDeleted;
            MyAreaId = viewmodel.MyAreaId;
            Location = viewmodel.Location;
        }
    }
}