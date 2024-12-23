using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class StatesController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /States/
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

            var result = from c in db.states
                         select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(c => c.StateName.Contains(searchString)||c.country.CountryName.Contains(searchString));

            }
            result = result.OrderByDescending(x => x.StateId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CountryId =  new SelectList(db.countries.Where(x=>x.Status==true).ToList(), "CountryId", "CountryName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(StateViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (Statenameexists(viewmodel.CountryId,viewmodel.satename))
                {
                    var state = new States()
                    {
                        StateName = viewmodel.satename,
                        Status = viewmodel.satus,
                        CountryId=viewmodel.CountryId
                    };
                    db.states.Add(state);
                    db.SaveChanges();
                    return RedirectToAction("Index", "States");
                }
                else
                {
                    ModelState.AddModelError("", "State already exists.");
                }


            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x=>x.Status==true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            return View();

        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var state = db.states.Where(x => x.StateId == Id).SingleOrDefault();
            if (state == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName",state.CountryId);
            ViewBag.Status = state.Status;
            var Stateviewmodel = new StateViewModel(state);

            return View(Stateviewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? Id,StateViewModel viewmodel)
        {
            var State = db.states.Where(x => x.StateId == Id).SingleOrDefault();
            
            if (State == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var StateToUpdate = State;
                try
                {
                    if (ModelState.IsValid)
                    {

                        State.StateName = viewmodel.satename;
                        State.CountryId = viewmodel.CountryId;
                        State.Status = viewmodel.satus;
                        db.SaveChanges();
                        return RedirectToAction("Index", "States");
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
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", State.CountryId);
            return View(State);

        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var state = db.states.Where(x => x.StateId == Id).SingleOrDefault();
            if (state == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(state);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var state = db.states.Where(x => x.StateId == id).SingleOrDefault();
            if (state == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EmailId = state.StateName;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Deletepost(int Id)
        {
            var state = db.states.Where(x => x.StateId == Id).SingleOrDefault();
            if (state == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            db.states.Remove(state);
            db.SaveChanges();
            return RedirectToAction("Index", "States");
        }

        public bool Statenameexists(int? countryid,string statename)
        {
            if (db.states.Where(x => x.CountryId == countryid&&x.StateName==statename).Count() > 0)
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