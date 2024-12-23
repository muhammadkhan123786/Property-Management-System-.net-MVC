using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using System.Web.Routing;
using PagedList;
using _360PropertyManagement.ViewModels;
using System.Drawing;
using System.Net;


namespace _360PropertyManagement.Controllers
{
    public class SuperAdminLicenseController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /SuperAdminLicense/
        public ActionResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var catgories = from c in db.LicenseProduct
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.CompanyName.Contains(searchString));

            }
            catgories = catgories.OrderByDescending(x => x.LicenseId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
            
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new LicenseViewModel();
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName",viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName",viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName",viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);
            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(LicenseViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                if(checkLicense(viewmodel.CompanyName,viewmodel.Website))
                {
                    var rolid = db.roles.Where(x => x.RoleName == "Admin").SingleOrDefault();
                    var pwd=Extensions.Encrypt(viewmodel.AccountPassword);
                    var acc = new Accounts() { 
                       AccountEmailId=viewmodel.AccountEmailId,
                       AccountPassword=pwd,
                       IsActive=true,
                       DateofCreation=DateTime.Now,
                       IsVarified=true,
                       RoleId=rolid.RoleId
                    };
                    db.accounts.Add(acc);
                    var uploadImage = new Bitmap(viewmodel.Photo.InputStream);
                    var newimage = Extensions.ResizeImage(uploadImage);
                    var imagebytes = Extensions.ImageToByte(newimage);
                    var TosaveImage = new Images()
                    {
                        Image = imagebytes
                    };
                    db.images.Add(TosaveImage);
                    db.SaveChanges();
                    var person = new Persons() { 
                    PersonFirstName=viewmodel.Firstname,
                    PersonMiddleName=viewmodel.Secondname,
                    PersonLastName=viewmodel.lastname,
                    GenderId=viewmodel.GenderId,
                    OccupationId=viewmodel.OccupationId,
                    AccountId=acc.AccountId,
                    ImageId=TosaveImage.ImageId,
                    Status=true
                  };
                    db.persons.Add(person);
                    var contact = new Contacts() { 
                    MobileOne=viewmodel.MobileOne,
                    MobileTwo=viewmodel.MobileTwo,
                    PhoneOne=viewmodel.PhoneOne,
                    PhoneTwo=viewmodel.PhoneTwo,
                    Website=viewmodel.Website,
                    Status=true,
                    PersonId=person.PersonId
                    };
                    db.contacts.Add(contact);

                    var address = new Addresses() {
                    AddressOne=viewmodel.AddressOne,
                    AddressTwo=viewmodel.AddressTwo,
                    CountryId=viewmodel.CountryId,
                    StateId=viewmodel.StateId,
                    CityId=viewmodel.CityId,
                    Status=true,
                    PersonId=person.PersonId
                    };
                    db.addresses.Add(address);
                    var logoImage = new Bitmap(viewmodel.CompanyLogo.InputStream);
                    var newlogoimage = Extensions.ResizeLogo(logoImage);
                    var logoimagebytes = Extensions.ImageToByte(newlogoimage);
                    
                    var companylogo = new Images() { 
                        Image=logoimagebytes
                    
                    };
                    db.images.Add(companylogo);

                    var licensedetails = new LicenseProduct() {
                    CompanyName=viewmodel.CompanyName,
                    ImageId=companylogo.ImageId,
                    AboutUs=viewmodel.AboutUs,
                    MissionStatement=viewmodel.MissionStatement,
                    VisionStatement=viewmodel.VisionStatement,
                    SalogonStatement=viewmodel.SalogonStatement,
                    Status=viewmodel.Status,
                    PersonId=person.PersonId,
                    ContactId=contact.ContactId,
                    AddressId=address.AddressId,
                    AccountId=acc.AccountId,
                    Createdon=DateTime.Now,
                    ExpireOn=DateTime.Now.AddYears(Convert.ToInt16(viewmodel.LicenseFeeForYears)),
                    AnnualFee = viewmodel.AnnualFee
                   
                    };
                    db.LicenseProduct.Add(licensedetails);
                    var totalpayment = ((((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee)) - viewmodel.Discount);
                    var feetotal = (((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee));
                
                    var licensefee = new LicenseFees() {
                    LicenseFeePerYear = Convert.ToDecimal(feetotal),
                    LicenseFeeForYears=viewmodel.LicenseFeeForYears,
                    DomainandHostingFee=viewmodel.DomainandHostingFee,
                    Discount=viewmodel.Discount,
                    EnteringDate=DateTime.Now,
                    ReceivingDate=viewmodel.ReceivingDate,
                    TotalPaid = Convert.ToDecimal(totalpayment),
                    Status=viewmodel.PaymentStatus,
                    LicenseId=licensedetails.LicenseId
                    
                    };
                    db.LicenseFee.Add(licensefee);
                    db.SaveChanges();
                    return RedirectToAction("Index", "SuperAdminLicense");
                }
                else
                {
                    ModelState.AddModelError("", "The License Alreay Exists!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not Valid Please check");
            }

            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);
            return View();
        }

        [HttpGet]
        public ActionResult ChangeCompanyLogo(int Id)
        {
            var con = db.images.Where(x => x.ImageId == Id).SingleOrDefault();
            if (con == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageuploders = new ImageUploader();
            return View(imageuploders);

        }

        [HttpPost, ActionName("ChangeCompanyLogo")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeCompanyLogo(int? Id, ImageUploader picture)
        {
            var img = db.images.Where(x => x.ImageId == Id).SingleOrDefault();
            if (img == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (picture.Photo == null)
            {
                ModelState.AddModelError("", "Please upload image");
            }
            else
            {

                var uploadImage = new Bitmap(picture.Photo.InputStream);
                var newimage = Extensions.ResizeLogo(uploadImage);
                var imagebytes = Extensions.ImageToByte(newimage);
                img.Image = imagebytes;

                var orderToUpdate = img;
                if (TryUpdateModel(orderToUpdate, "",
                   new string[] { "Image" }))
                    try
                    {
                        if (ModelState.IsValid)
                        {
                            db.SaveChanges();
                            return RedirectToAction("Index", "SuperAdminLicense");

                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Some thing is went wrong Please check!!" + e.ToString());
                    }

            }
            return View();
        }


        [HttpGet]
        public ActionResult AddLicenseFee(int Id)
        {
            var license = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Licensefeeviewmodel = new AddPaymentViewmodel();
            return View(Licensefeeviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddLicenseFee(int? Id, AddPaymentViewmodel viewmodel)
        {
            var license = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                var totalpayment = ((((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee)) - viewmodel.Discount);
                var feetotal = (((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee));
                var LicenseFee = new LicenseFees() {
                LicenseFeePerYear = Convert.ToDecimal(feetotal),
                LicenseFeeForYears=viewmodel.LicenseFeeForYears,
                Discount=viewmodel.Discount,
                TotalPaid =Convert.ToDecimal(totalpayment),
                EnteringDate=DateTime.Now,
                ReceivingDate=viewmodel.ReceivingDate,
                LicenseId=license.LicenseId,
                DomainandHostingFee=viewmodel.DomainandHostingFee,
                Status=viewmodel.PaymentStatus
                
                };
                db.LicenseFee.Add(LicenseFee);
                license.ExpireOn = license.ExpireOn.AddYears(Convert.ToInt16(viewmodel.LicenseFeeForYears));
                db.SaveChanges();
                return RedirectToAction("Index", "SuperAdminLicense");
            }
            else
            {
                ModelState.AddModelError("", "The Model State Is Not Valid Please Check..");
            }
            return View(viewmodel); 
        }
      
        [HttpGet]
        public ActionResult FeeEdit(int Id)
        {
            var LicenseFee = db.LicenseFee.Where(x => x.LicenseFeeId == Id).SingleOrDefault();
            if(LicenseFee==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new AddPaymentViewmodel(LicenseFee);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeeEdit(int? Id,AddPaymentViewmodel viewmodel)
        {
            var LicenseFee = db.LicenseFee.Where(x => x.LicenseFeeId == Id).SingleOrDefault();
            if (LicenseFee == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                var totalpayment = ((((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee)) - viewmodel.Discount);
                var feetotal = (((viewmodel.LicenseFeePerYear) * (viewmodel.LicenseFeeForYears)) + (viewmodel.DomainandHostingFee));

                LicenseFee.LicenseFeePerYear = Convert.ToDecimal(feetotal);
                LicenseFee.LicenseFeeForYears = viewmodel.LicenseFeeForYears;
                LicenseFee.Discount = viewmodel.Discount;
                LicenseFee.TotalPaid = Convert.ToDecimal(totalpayment);
                LicenseFee.ReceivingDate = viewmodel.ReceivingDate;
                LicenseFee.DomainandHostingFee = viewmodel.DomainandHostingFee;
                LicenseFee.Status = viewmodel.PaymentStatus;
                db.SaveChanges();
                return RedirectToAction("SuperLicensePayments", "SuperAdminLicense", new { id = LicenseFee.LicenseId });
                
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid...");
            }
            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult EditLicense(int Id)
        {
            var license = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var licenseeditviewmodel = new LicenseEditViewModel(license);
            return View(licenseeditviewmodel);
        }


        [HttpPost, ActionName("EditLicense")]
        [ValidateAntiForgeryToken]
        public ActionResult EditLicensePost(int? Id,LicenseEditViewModel viewmodel)
        {
            var con = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if (con == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
               
                con.SalogonStatement = viewmodel.SalogonStatement;
                con.ExpireOn = viewmodel.ExpireOn;
                db.SaveChanges();
                return RedirectToAction("Index", "SuperAdminLicense");

            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid Please check..");
            }

            
            return View(viewmodel);


        }

        [HttpGet]
        public ActionResult SuperLicensePayments(int? Id, string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var catgories = from c in db.LicenseFee
                            where
                                c.LicenseId == Id
                            select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.LicenseFeeId.ToString().Contains(searchString));

            }
            catgories = catgories.OrderByDescending(x => x.LicenseFeeId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult StateList(int CountryId)
        {
            var states = (from s in db.states
                          where s.CountryId == CountryId
                          select new
                          {
                              id = s.StateId,
                              name = s.StateName

                          }).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }




        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CityList(int StateId)
        {
            var City = (from s in db.cities
                          where s.StateId == StateId
                          select new
                          {
                              id = s.CityId,
                              name = s.CityName

                          }).ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }


        public bool checkLicense(string businessname,string website)
        {
            if(db.LicenseProduct.Where(x=>x.CompanyName==businessname&&x.contact.Website==website).Count()>0)
            {
                return false;
            }
            return true;
        }


        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            // if (!Request.IsAuthenticated)
            if (_authentication.IsUserSuperAdmin())
            {
                base.OnActionExecuting(ctx);
            }
            else
            {
                ctx.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });
            }
        }

        //Make sure that the resources are cleared.
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

	}
}