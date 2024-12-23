using System;
using System.Collections.Generic;
using System.Linq;
using _360PropertyManagement.ViewModels;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using System.Net;
using PagedList;


namespace _360PropertyManagement.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
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

            var catgories = from c in db.toppropertycategory
                            where
                            c.IsDeleted == false
                            select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.TopCategoryName.Contains(searchString) && c.IsDeleted == false);

            }
            catgories = catgories.OrderByDescending(x => x.TopCategoryId);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("TopCategories",catgories.ToPagedList(pageNumber, pageSize));
        }


        public PartialViewResult TopCategories(IPagedList<TopCategory> messages)
        {
            return PartialView(messages);
        }

        public ActionResult Propertiesbycategory(int? id,int? CountryId,int? StateId,int? CityId, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");

            var catgories = from c in db.submitedads
                            where
                            c.IsDeleted == false && c.propertyad.TopCategoryId==id
                            select c;
            catgories = catgories.OrderByDescending(x => x.AdId);
            if(CountryId.HasValue)
            {
                catgories = catgories.Where(x =>x.areaads.CountryId==CountryId);
            }
            if(StateId.HasValue)
            {
                catgories=catgories.Where(x=>x.areaads.StateId==StateId);
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("Propertiesbycategoryid", catgories.ToPagedList(pageNumber, pageSize));
        }


        public PartialViewResult Propertiesbycategoryid(IPagedList<SubmitedAds> messages)
        {
            return PartialView(messages);
        }

        public ActionResult Agents(string location,int? city,string emailid, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (location != null)
            {
                page = 1;
            }
            else
            {
                location = currentFilter;
            }

            ViewBag.CurrentFilter = location;

            var catgories = from c in db.accounts
                            where
                            c.IsDeleted == false &&c.role.RoleName=="Agent"
                            select c;

            if (!String.IsNullOrEmpty(emailid))
            {
                catgories = catgories.Where(c => c.AccountEmailId.Contains(emailid) && c.IsDeleted == false);
            }
            else if(city!=null)
            {
                //var areas = db.MyAreasAds.Where(x=>x.CityId==city&&x.account.role.RoleName=="Agent").ToList();
                 IQueryable<MyAreas> areass= (from p in db.MyAreasAds
                                                 where p.CityId == city && p.IsDeleted==false
                                                 select p
                                                 ).ToList().AsQueryable();
                 catgories = from x in areass
                             select x.account;
            }
            else if(!String.IsNullOrEmpty(location))
            {
                IQueryable<MyAreas> arealocation = (from p in db.MyAreasAds
                                              where p.Location.Contains(location) && p.IsDeleted == false
                                              select p
                                                    ).ToList().AsQueryable();
                catgories = from x in arealocation
                            select x.account;
            }
            catgories = catgories.OrderByDescending(x => x.AccountId);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("Agents", catgories.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Error()
        {
            return View();
        }

        public ActionResult About()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new AboutUsViewModel(license);
            return View(viewmodel);
        }

        public ActionResult Contact()
        {
            var license = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
            if(license==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cont = db.contacts.Where(x => x.ContactId == license.ContactId).SingleOrDefault();
            var viewmodel = new LicenseContactInfo(cont);
            return View(viewmodel);
        }

        

        [HttpPost]
        public JsonResult autoCompleteCategory(string Prefix)  
        {  
            //Note : you can bind same list from database  
            var objList=db.toppropertycategory.Where(x=>x.IsActive==true && x.IsDeleted==false).ToList();
            //Searching records from list using LINQ query  
            var CityName = (from N in objList  
                            where N.TopCategoryName.StartsWith(Prefix)  
                          select new { N.TopCategoryName });
            return Json(CityName, JsonRequestBehavior.AllowGet);  
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var ad = db.agentsads.Where(x => x.AdId == Id).SingleOrDefault();
            if (ad == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            ad.adid.NumberOfViews = ad.adid.NumberOfViews + 1; ;
            db.SaveChanges();

            return View(ad);
        }


     
    }
}