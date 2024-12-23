﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.Models
{
    [Table("tblRooms")]
    public class PropertyRooms
    {
        [Key]
        public int RoomsId { get; set; }
        public string NumberOfRooms { get; set; }
        public string Remarks { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        

    }
}