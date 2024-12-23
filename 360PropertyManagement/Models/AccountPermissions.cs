using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblAccountPermissions")]
    public class AccountPermissions
    {
     
     [Key]
     public int UserPermissionId  {get;set;}

     public bool CanSeeContactNumberOfAdUser{get;set;}	
     public bool CanSeeAddressOfAds	{get;set;}
     public bool CanSeeMinimumPrice	{get;set;}
     public string remarks	{get;set;}
     public bool  CanShowItsnumberinads{get;set;}
     public bool  CanshowItsaddressonads{get;set;}


     public int? AccountId { get; set; }
     public virtual Accounts account { get; set; } 
    }
}