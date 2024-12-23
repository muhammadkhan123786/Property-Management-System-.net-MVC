using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblAccounts")]
    public class Accounts
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountEmailId { get; set; }
        public string AccountPassword { get; set; }
        public bool IsVarified { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

       
        public DateTime DateofCreation { get; set; }

        public int? RoleId { get; set; }
        public virtual Roles role { get; set; }
    }
}