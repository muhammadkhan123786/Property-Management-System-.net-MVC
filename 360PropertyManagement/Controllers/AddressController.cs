using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using _360PropertyManagement.ViewModels;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using System.Drawing;
using System.Net;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class AddressController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
       
        //
        // GET: /Address/
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if(searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var catgories = from c in db.addresses
                            where c.IsDeleted == false
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.AddressOne.Contains(searchString) || c.AddressTwo.Contains(searchString)
                    || c.person.PersonFirstName.Contains(searchString) || c.person.PersonMiddleName.Contains(searchString) || c.person.PersonLastName.Contains(searchString) && c.IsDeleted == false);

            }
            catgories = catgories.OrderByDescending(x => x.AddressId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new AddressViewModel();
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AddressViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                if (Addressexists(viewmodel.AddressOne, viewmodel.PersonFirstName,viewmodel.PersonMiddleName,viewmodel.PersonLastName))
                {
                    if (viewmodel.Photo == null)
                    {
                        var imagenot = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                        var TosaveImage = new Images()
                        {
                            Image = imagenot.ImageNotAvailable
                        };
                        db.images.Add(TosaveImage);
                            
                        var person = new Persons()
                        {
                            PersonFirstName = viewmodel.PersonFirstName,
                            PersonMiddleName = viewmodel.PersonMiddleName,
                            PersonLastName = viewmodel.PersonLastName,
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId=TosaveImage.ImageId,
                            IsDeleted=false,
                            Status=true

                        };
                        db.persons.Add(person);
                        var address = new Addresses()
                        {
                            AddressOne=viewmodel.AddressOne,
                            AddressTwo=viewmodel.AddressTwo,
                            CountryId=viewmodel.CountryId,
                            StateId=viewmodel.StateId,
                            CityId=viewmodel.CityId,
                            IsDeleted=false,
                            Status=viewmodel.Status,
                            PersonId=person.PersonId
                        };
                        db.addresses.Add(address);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Address");

                    }
                    else
                    {
                        var uploadImage = new Bitmap(viewmodel.Photo.InputStream);
                        var newimage = Extensions.ResizeImage(uploadImage);
                        var imagebytes = Extensions.ImageToByte(newimage);
                        var TosaveImage = new Images()
                        {
                            Image = imagebytes,
                            IsDeleted = false
                        };
                        db.images.Add(TosaveImage);

                        var person = new Persons()
                        {
                            PersonFirstName = viewmodel.PersonFirstName,
                            PersonMiddleName = viewmodel.PersonMiddleName,
                            PersonLastName = viewmodel.PersonLastName,
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId = TosaveImage.ImageId,
                            IsDeleted = false,
                            Status = true

                        };
                        db.persons.Add(person);
                        var address = new Addresses()
                        {
                            AddressOne = viewmodel.AddressOne,
                            AddressTwo = viewmodel.AddressTwo,
                            CountryId = viewmodel.CountryId,
                            StateId = viewmodel.StateId,
                            CityId = viewmodel.CityId,
                            IsDeleted = false,
                            Status = viewmodel.Status,
                            PersonId = person.PersonId
                        };
                        db.addresses.Add(address);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Address");

                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "The Address Already exists,Please check...");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please Check...");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);
            return View(viewmodel);

        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)            
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Address is null Please Check...");
            }
            var viewmodel = new AddressViewModel(address);
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);

            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id,AddressViewModel viewmodel)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Address is null Please Check...");
            }
            if(ModelState.IsValid)
            {
                try
                {
                    address.AddressOne = viewmodel.AddressOne;
                    address.AddressTwo = viewmodel.AddressTwo;
                    address.CountryId = viewmodel.CountryId;
                    address.StateId = viewmodel.StateId;
                    address.CityId = viewmodel.CityId;
                    address.person.PersonFirstName = viewmodel.PersonFirstName;
                    address.person.PersonMiddleName = viewmodel.PersonMiddleName;
                    address.person.PersonLastName = viewmodel.PersonLastName;
                    address.Status = viewmodel.Status;
                    address.person.GenderId = viewmodel.GenderId;
                    address.person.OccupationId = viewmodel.OccupationId;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Address");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There is some exception please check.."+ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check...");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.GenderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.OccupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);

            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Address is null Please Check...");
            }
            var viewmodel = new AddressViewModel(address);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Address is null Please Check...");
            }
            ViewBag.Object = address.person.PersonFullName;
            return View();
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int Id)
        {
            var address = db.addresses.Where(x => x.AddressId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Address is null Please Check...");
            }
            address.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Address");
        }

        public bool Addressexists(string address,string personname,string personmiddlename,string personlastname)
        {
            if (db.addresses.Where(x => x.AddressOne == address && x.person.PersonFirstName == personname&&x.person.PersonMiddleName==personmiddlename&&x.person.PersonLastName==personlastname).Count() > 0)
            {
                return false;
            }
            return true;
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