using _360PropertyManagement.Models;
using System;
using _360PropertyManagement.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Routing;
using System.Net;

namespace _360PropertyManagement.Controllers
{
    public class MyAreasController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
       
        //
        // GET: /MyAreas/
        public ActionResult Index(string searchString, int? Countryid, int? stateid,int? CityId, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName");

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var user = _authentication.GetUser();
            var result = from c in db.MyAreasAds
                         where c.IsDeleted == false && c.AccountId==user.AccountId
                         select c;

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(c => c.Location.Contains(searchString) || c.CountryId == Countryid || c.StateId == stateid||c.CityId==CityId);
            }
          
            result = result.OrderByDescending(x => x.MyAreaId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName");
            var viewmodel = new MyAreasViewmodel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(MyAreasViewmodel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                var myarea = new MyAreas() {
                CountryId=viewmodel.CountryId,
                StateId=viewmodel.StateId,
                CityId=viewmodel.CityId,
                Location=viewmodel.Location,
                DateNTime=DateTime.Now,
                IsDeleted=false,
                ZipCode=viewmodel.ZipCode,
                IsActive=viewmodel.IsActive,
                Remarks=viewmodel.Remarks,
                AccountId=user.AccountId,
                RoleId=user.RoleId
              };
              db.MyAreasAds.Add(myarea);
              db.SaveChanges();
              return RedirectToAction("Index", "MyAreas");

            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check...");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName");
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var area = db.MyAreasAds.Where(x => x.MyAreaId == Id).FirstOrDefault();
            if(area==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "area is null Please Check...");
            }
            var viewmodel = new MyAreasViewmodel(area);
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName",viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName",viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName",viewmodel.CityId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id,MyAreasViewmodel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var area = db.MyAreasAds.Where(x => x.MyAreaId == id).FirstOrDefault();
                if (area == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "area is null Please Check...");
                }
                area.CountryId = viewmodel.CountryId;
                area.StateId = viewmodel.StateId;
                area.CityId = viewmodel.CityId;
                area.IsActive = viewmodel.IsActive;
                area.Location = viewmodel.Location;
                area.ZipCode = viewmodel.ZipCode;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check...");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var area = db.MyAreasAds.Where(x => x.MyAreaId == Id).FirstOrDefault();
            if (area == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "area is null Please Check...");
            }
           
            return View(area);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var area = db.MyAreasAds.Where(x => x.MyAreaId == Id).FirstOrDefault();
            if (area == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "area is null Please Check...");
            }
            ViewBag.AreaName = area.Location;
            return View();
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int Id)
        {
            var area = db.MyAreasAds.Where(x => x.MyAreaId == Id).FirstOrDefault();
            if (area == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "area is null Please Check...");
            }
            area.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            // if (!Request.IsAuthenticated)
            if (_authentication.IsUserAuthenicated())
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