using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _360PropertyManagement.Models;

namespace _360PropertyManagement.ViewModels
{
    public class AboutUsViewModel
    {
        public string AboutUs { get; set; }
        public string MissionStatement { get; set; }
        public string VisionStatement { get; set; }
        public int LicenseId { get; set; }

        public AboutUsViewModel()
        {
            AboutUs = this.AboutUs;
            MissionStatement = this.MissionStatement;
            VisionStatement = this.VisionStatement;
            LicenseId = this.LicenseId;
        }
        public AboutUsViewModel(LicenseProduct license)
        {
            AboutUs = license.AboutUs;
            MissionStatement = license.MissionStatement;
            VisionStatement = license.VisionStatement;
            LicenseId = license.LicenseId;
        }
    }
}