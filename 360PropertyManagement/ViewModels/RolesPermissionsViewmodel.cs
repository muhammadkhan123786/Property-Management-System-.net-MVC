using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class RolesPermissionsViewmodel
    {
        public int RolePermissionId { get; set; }
        public bool CanSeeContactNumberOfAdUser { get; set; }
        public bool CanSeeAddressOfAds { get; set; }
        public bool CanSeeMinimumPrice { get; set; }
        public string remarks { get; set; }
        public bool CanShowItsnumberinads { get; set; }
        public bool CanshowItsaddressonads { get; set; }
        public bool CanShowItsNameOnAds { get; set; }

        public int? RoleId { get; set; }
        public virtual Roles role { get; set; }
        
        public RolesPermissionsViewmodel()
        {
            RolePermissionId = this.RolePermissionId;
            CanSeeContactNumberOfAdUser = this.CanSeeContactNumberOfAdUser;
            CanSeeAddressOfAds = this.CanSeeAddressOfAds;
            CanSeeMinimumPrice = this.CanSeeMinimumPrice;
            remarks = this.remarks;
            CanShowItsNameOnAds = this.CanShowItsNameOnAds;
            CanshowItsaddressonads = this.CanshowItsaddressonads;
            CanShowItsNameOnAds = this.CanShowItsNameOnAds;
            RoleId = this.RoleId;
        }

        public RolesPermissionsViewmodel(RolesPermissions rolepermission)
        {
            RolePermissionId = rolepermission.RolePermissionId;
            CanSeeContactNumberOfAdUser = rolepermission.CanSeeContactNumberOfAdUser;
            CanSeeAddressOfAds = rolepermission.CanSeeAddressOfAds;
            CanSeeMinimumPrice = rolepermission.CanSeeMinimumPrice;
            remarks = rolepermission.remarks;
            CanShowItsNameOnAds = rolepermission.CanShowItsNameOnAds;
            CanshowItsaddressonads = rolepermission.CanshowItsaddressonads;
            CanShowItsNameOnAds = rolepermission.CanShowItsNameOnAds;
            RoleId = rolepermission.RoleId;
            
        }
    }
}