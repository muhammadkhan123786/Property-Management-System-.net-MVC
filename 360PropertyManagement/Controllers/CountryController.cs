using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using _360PropertyManagement.ViewModels;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class CountryController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /Country/
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

                   var result = from c in db.countries
                                where c.IsDeleted==false
                            select c;
        

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(c => c.CountryName.Contains(searchString)&&c.IsDeleted==false);

            }
            result = result.OrderByDescending(x => x.CountryId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CountriesViewModel viewmodel )
        {
            if(ModelState.IsValid)
            {
                if(countrynameexists(viewmodel.CountryName))
                {
                    var country = new Countries() { 
                     CountryName=viewmodel.CountryName,
                     Status=viewmodel.Status,
                     IsDeleted=false
                    };
                    db.countries.Add(country);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Country");
                }
                else
                {
                    ModelState.AddModelError("", "Country name already exists.");
                }
                

            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");
            }
            return View();

        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var country = db.countries.Where(x => x.CountryId == Id).SingleOrDefault();
            if(country==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var countryviewmodel = new CountriesViewModel(country);
            return View(countryviewmodel);
        }

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? Id)
        {
            var country = db.countries.Where(x => x.CountryId == Id).SingleOrDefault();
            if(country==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var CountryToUpdate = country;
            if (TryUpdateModel(CountryToUpdate, "",
                         new string[] { "CountryName", "Status"}))
                try
                {
                    if(ModelState.IsValid)
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "Country");
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
            return View(country);          

        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var country = db.countries.Where(x => x.CountryId == Id).SingleOrDefault();

            if(country==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(country);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var acc = db.countries.Where(x => x.CountryId == id).SingleOrDefault();
           
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EmailId = acc.CountryName;
            return View();
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deletepost(int Id)
        {
            var country = db.countries.Where(x => x.CountryId == Id).SingleOrDefault();
            if(country==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            country.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Country");
        }
        
        public bool countrynameexists(string countryname)
        {
            if (db.countries.Where(x => x.CountryName ==countryname&&x.IsDeleted==false).Count() > 0)
            {
                return false;
            }
            return true;
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