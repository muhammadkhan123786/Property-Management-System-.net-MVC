using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using PagedList;
using System.Net;
using _360PropertyManagement.ViewModels;
using System.Drawing;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class LicenseSettingsController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /LicenseSettings/
        public ActionResult Index()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(license);
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
                            return RedirectToAction("Index", "LicenseSettings");

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
        public ActionResult LicensePayments(int? Id, string searchString, string currentFilter, int? page, string sortOrder)
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
                                c.LicenseId ==Id
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

        [HttpGet]
        public ActionResult LicenseDetails()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var licenseviewmodel = new LicenseDetailsViewmodel(license);
            return View(licenseviewmodel);
        }

        [HttpGet]
        public ActionResult EditDetails()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new LicenseDetailsViewmodel(license);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetails(LicenseDetailsViewmodel viewmodel)
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if (license == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                license.account.AccountEmailId = viewmodel.AccountEmailId;
                license.SalogonStatement = viewmodel.SalogonStatement;
                license.CompanyName = viewmodel.CompanyName;
                license.contact.Website = viewmodel.Website;
                db.SaveChanges();
                return RedirectToAction("LicenseDetails", "LicenseSettings");

            }
            else
            {
                ModelState.AddModelError("", "The Model State Is not valid...");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult LicenseAddress()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(license);

        }

        [HttpGet]
        public ActionResult LicensePerson()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var person = db.persons.Where(x => x.PersonId == license.PersonId).SingleOrDefault();
            var viewmodel = new LicensePersonViewModel(person);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult EditLicensePersonInfo(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new LicensePersonViewModel(person);
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName",viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName",viewmodel.GenderId);
            return View(viewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLicensePersonInfo(int? Id,LicensePersonViewModel viewmodel)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                try
                {
                    person.PersonFirstName = viewmodel.Firstname;
                    person.PersonMiddleName = viewmodel.Secondname;
                    person.PersonLastName = viewmodel.lastname;
                    person.GenderId = viewmodel.GenderId;
                    person.OccupationId = viewmodel.OccupationId;
                    db.SaveChanges();
                    return RedirectToAction("LicensePerson", "LicenseSettings");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There is some error"+ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State Is Not Valid..");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);

        }



        [HttpGet]
        public ActionResult EditAddress(int Id)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new EditAddressViewModel(address);
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
           
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAddress(int? Id,EditAddressViewModel viewmodel)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                try
                {
                    address.AddressOne = viewmodel.AddressOne;
                    address.AddressTwo = viewmodel.AddressTwo;
                    address.CountryId = viewmodel.CountryId;
                    address.Status = viewmodel.Status;
                    address.StateId = viewmodel.StateId;
                    address.CityId = viewmodel.CityId;
                    db.SaveChanges();
                    return RedirectToAction("Index","LicenseSettings");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There is something error please "+ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid.");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);
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

              

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var payment = db.LicenseFee.Where(x => x.LicenseFeeId == Id).SingleOrDefault();
            if(payment==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(payment);
        }


        [HttpGet]
        public ActionResult LicenseContactInfo()
        {
            var licnese = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(licnese==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contact = db.contacts.Where(x => x.ContactId == licnese.ContactId).SingleOrDefault();
            var viewmodel = new LicenseContactInfo(contact);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult EditLicenseContactInfo(int Id)
        {
            var contact = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if(contact==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new LicenseContactInfo(contact);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLicenseContactInfo(int? Id,LicenseContactInfo viewmodel)
        {
            var contact = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if(contact==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                try
                {
                    contact.MobileOne = viewmodel.MobileOne;
                    contact.MobileTwo = viewmodel.MobileTwo;
                    contact.Website = viewmodel.Website;
                    contact.PhoneOne = viewmodel.PhoneOne;
                    contact.PhoneTwo = viewmodel.PhoneTwo;
                    db.SaveChanges();
                    return RedirectToAction("LicenseContactInfo", "LicenseSettings");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There Is Some Exception Please check this" + ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid...");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult LicenseAboutUs()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new AboutUsViewModel(license);
            return View(viewmodel);

        }

        [HttpGet]
        public ActionResult EditAboutUs(int Id)
        {
            var license = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new AboutUsViewModel(license);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAboutUs(int? Id,AboutUsViewModel viewmodel)
        {
            var license = db.LicenseProduct.Where(x => x.LicenseId == Id).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if(ModelState.IsValid)
            {
                try
                {
                    license.AboutUs = viewmodel.AboutUs;
                    license.MissionStatement = viewmodel.MissionStatement;
                    license.VisionStatement = viewmodel.VisionStatement;
                    db.SaveChanges();
                    return RedirectToAction("LicenseAboutUs", "LicenseSettings");

                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There Is Some Exception Please Check..this"+ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid please check..");
            }
            return View(viewmodel);
        }


        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            // if (!Request.IsAuthenticated)
            if (_authentication.IsUserAdmin() || _authentication.IsUserSuperAdmin())
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