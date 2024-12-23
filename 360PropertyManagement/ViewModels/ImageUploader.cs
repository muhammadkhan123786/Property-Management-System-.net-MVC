using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _360PropertyManagement.ViewModels
{
    public class ImageUploader
    {
        [DataType(DataType.Upload)]
        [Required(ErrorMessage="Please Uplaod Image")]
        public HttpPostedFileBase Photo { get; set; }
       
    }
}