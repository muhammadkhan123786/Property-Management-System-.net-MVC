using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class EditPersonalInfoViewModel
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public bool? Status { get; set; }

        public int? GenderId { get; set; }

        public int? OccupationId { get; set; }

        public virtual Genders gender { get; set; }

        public virtual Occupations occupation { get; set; }
       

        public EditPersonalInfoViewModel(Contacts con)
        {
            FirstName = con.person.PersonFirstName;
            SecondName = con.person.PersonMiddleName;
            LastName = con.person.PersonLastName;
            Status = con.Status;
            GenderId = con.person.GenderId;
            OccupationId = con.person.OccupationId;
        }
    }
}