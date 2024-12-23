using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;

namespace _360PropertyManagement.ViewModels
{
    public class TopPropertyCategoryViewModel
    {
        public int TopCategoryId { get; set; }
        [Required(ErrorMessage="Please Enter Category Name")]
        public string TopCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        [Required(ErrorMessage = "Please Select Status")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "Please Select Is This Have Rent, Sale And Other Options")]
        public bool IsShowSubCategory { get; set; }
        [Required(ErrorMessage = "Please Select Is This Category Have Area")]
        public bool IsShowArea { get; set; }
        [Required(ErrorMessage = "Please Select Is This Category Have Furnished Option")]
        public bool IsShowFurnished { get; set; }
        [Required(ErrorMessage = "Please Select This Category Either Have Rooms Option")]
        public bool IsShowRooms { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage="Please upload thumbnail image...")]
        public HttpPostedFileBase Photo { get; set; }


        public string Remarks { get; set; }

        public TopPropertyCategoryViewModel()
        {
            TopCategoryId = this.TopCategoryId;
            TopCategoryName = this.TopCategoryName;
            IsDeleted = this.IsDeleted;
            IsActive = this.IsActive;
            Remarks = this.Remarks;
            IsShowSubCategory = this.IsShowSubCategory;
            IsShowArea = this.IsShowArea;
            IsShowFurnished = this.IsShowFurnished;
            IsShowRooms = this.IsShowRooms;
            Photo = this.Photo;
        }
        public TopPropertyCategoryViewModel(TopCategory topcategory)
        {
            TopCategoryId = topcategory.TopCategoryId;
            TopCategoryName = topcategory.TopCategoryName;
            IsActive =Convert.ToBoolean(topcategory.IsActive);
            Remarks = topcategory.Remarks;
            IsShowSubCategory = Convert.ToBoolean(topcategory.IsShowSubCategory);
            IsShowArea = Convert.ToBoolean(topcategory.IsShowArea);
            IsShowFurnished = Convert.ToBoolean(topcategory.IsShowFurnished);
            IsShowRooms =Convert.ToBoolean(topcategory.IsShowRooms);

        }

    }
}