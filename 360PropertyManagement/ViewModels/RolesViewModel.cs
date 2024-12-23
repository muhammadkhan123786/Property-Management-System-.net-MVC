using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using _360PropertyManagement.Models;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class RolesViewModel
    {
        [Required(ErrorMessage="Please Enter Role Name")]
        public string RoleName { get; set; }
        [Required(ErrorMessage="Please select status")]
        public bool Status { get; set; }

        public int roleids { get; set; }
        public RolesViewModel()
        {
            RoleName = this.RoleName;
            Status = this.Status;
            roleids = this.roleids;

        }

        public RolesViewModel(Roles role)
        {
            RoleName = role.RoleName;
            Status = role.Status;
            roleids = role.RoleId;
        }


    }
}