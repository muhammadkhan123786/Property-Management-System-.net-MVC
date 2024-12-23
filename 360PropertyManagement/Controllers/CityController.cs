using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _360PropertyManagement.ViewModels;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using PagedList;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class CityController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /City/
        public ActionResult Index(string searchString,int? Countryid,int? stateid, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var result = from c in db.cities
                         where c.IsDeleted==false
                         select c;
            
            if (!String.IsNullOrEmpty(searchString)&&Countryid!=null&&stateid!=null)
            {
                result = result.Where(c => c.CityName.Contains(searchString)&&c.CountryId==Countryid&&c.StateId==stateid);
            }
            else
                if(!String.IsNullOrEmpty(searchString))
                {
                    result = result.Where(x => x.CityName.Contains(searchString));
                }
           
            result = result.OrderByDescending(x => x.CityId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new CityViewModel();
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);

            return View(viewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CityViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (CityNameexists(viewmodel.CountryId,viewmodel.StateId,viewmodel.CityName))
                {
                    var city = new Cities()
                    {
                        CityName = viewmodel.CityName,
                        Status = viewmodel.Status,
                        CountryId=viewmodel.CountryId,
                        StateId=viewmodel.StateId,
                        IsDeleted=false

                    };
                    db.cities.Add(city);
                    db.SaveChanges();
                    return RedirectToAction("Index", "City");
                }
                else
                {
                    ModelState.AddModelError("", "City name already exists with these info.");
                }


            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");

            return View(viewmodel);
            
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var city = db.cities.Where(x => x.CityId == Id).SingleOrDefault();
            if (city == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", city.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", city.StateId);

            var cityviewmodel = new CityViewModel(city);
            return View(cityviewmodel);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? Id,CityViewModel viewmodel)
        {
            var city = db.cities.Where(x => x.CityId == Id).SingleOrDefault();
            if (city == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             try
                {
                    if (ModelState.IsValid)
                    {
                        city.CityName = viewmodel.CityName;
                        city.Status = viewmodel.Status;
                        city.CountryId = viewmodel.CountryId;
                        city.StateId = viewmodel.StateId;
                        db.SaveChanges();
                        return RedirectToAction("Index", "City");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Model State is not valid!!");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Some Model Exception is occured!!");
                }
             ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", city.CountryId);
             ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", city.StateId);
            return View(city);

        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var city = db.cities.Where(x => x.CityId == Id).SingleOrDefault();

            if (city == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(city);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var city = db.cities.Where(x => x.CityId == id).SingleOrDefault();

            if (city == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EmailId = city.CityName;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deletepost(int Id)
        {
            var city = db.cities.Where(x => x.CityId == Id).SingleOrDefault();
            if (city == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            city.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "City");
        }

        public bool CityNameexists(int? Countryid,int? Stateid,string City)
        {
            if (db.cities.Where(x => x.CountryId == Countryid&&x.StateId==Stateid&&x.CityName==City&&x.IsDeleted==false).Count() > 0)
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
                          select new {
                          id = s.StateId,
                          name = s.StateName
                                        
                          }).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
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