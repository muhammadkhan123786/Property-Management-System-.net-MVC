using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class RoomsViewModel
    {
        public int RoomsId { get; set; }
        public string NumberOfRooms { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? TopCategoryId { get; set; }

        public RoomsViewModel()
        {
            RoomsId = this.RoomsId;
            NumberOfRooms = this.NumberOfRooms;
            Remarks = this.Remarks;
            IsActive = this.IsActive;
            IsDeleted = this.IsDeleted;
            TopCategoryId = this.TopCategoryId;
        }

        public RoomsViewModel(PropertyRooms room)
        {
            RoomsId = room.RoomsId;
            NumberOfRooms = room.NumberOfRooms;
            Remarks = room.Remarks;
            IsActive = room.IsActive;
           
        }
    }
}