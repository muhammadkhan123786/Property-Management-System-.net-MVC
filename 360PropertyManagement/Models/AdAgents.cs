using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblAgentAssign")]
    public class AdAgents
    {
        [Key]
        public int AgentAssignId { get; set; }
        public string Remarks { get; set; }
        public bool IsDeleted { get; set; }

        //Virtual 
        public int? ContactId { get; set; }
        public int? PersonId { get; set; }
        public int? AccountId { get; set; }
        public int? AdId { get; set; }

        public virtual Contacts con { get; set; }
        public virtual Persons person { get; set; }
        public virtual Accounts acc { get; set; }
        public virtual SubmitedAds adid { get; set; }
    }
}