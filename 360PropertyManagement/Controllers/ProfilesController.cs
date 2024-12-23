using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using _360PropertyManagement.ViewModels;
using System.Web;
using PagedList;
using System.Web.Mvc;
using System.Net;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class ProfilesController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /Profiles/
        public ActionResult Index(int Id, string searchString, string currentFilter, int? page, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.Id = Id;
            var acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            ViewBag.aacroleid = acc.RoleId;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var catgories = from c in db.msgreceiver
                            where c.IsDeleted == false && c.AccountId == Id
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.msgdetails.msg.MessageSubject.Contains(searchString) || c.msgdetails.MessageDetails.Contains(searchString)
                     && c.IsDeleted == false);

            }
            catgories = catgories.OrderBy(x => x.IsRead);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return PartialView("AccountMessages", catgories.ToPagedList(pageNumber, pageSize));

        }



        [HttpGet]
        public ActionResult MessageRead(int Id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MessageRead(int Id,MsgReplyViewModle viewmodel)
        {
            return View(viewmodel);
        }
        
        public ActionResult PersonalInfo(int Id)
        {
            var person = db.persons.Where(x => x.AccountId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Personal Info Not Added Yet By User....");
            }
            return PartialView("PersonInfoPartialView", person);
        }

        public PartialViewResult PersonInfoPartialView(Persons person)
        {
            if(person!=null)
            {
                return PartialView(person);
            }
            return PartialView("Error");
        }

        public ActionResult UserAddress(int Id)
        {
            var address = db.addresses.Where(x => x.person.AccountId == Id).SingleOrDefault();
            if(address==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The User didn't add address yet in his profile...");
            }
            return PartialView("AddressInfoPartialView", address);
        }

        public ActionResult ContactInfo(int Id)
        {
            var person = db.persons.Where(x => x.AccountId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Personal Info Not Added Yet By User....");
            }
            var contact = db.contacts.Where(x => x.ContactId == person.PersonId).SingleOrDefault();
            if (contact == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Contact Info Not Added Yet By User....");
            }
            return PartialView("ContactInfoPartialView", contact);
        }

        public PartialViewResult ContactInfoPartialView(Contacts con)
        {
            if (con != null)
            {
                return PartialView(con);
            }
            return PartialView("Error");
        }

        public PartialViewResult AddressInfoPartialView(Addresses address)
        {
            if (address != null)
            {
                return PartialView(address);
            }
            return PartialView("Error");
        }

        public PartialViewResult AccountMessages(IPagedList<MsgReceiver> messages)
        {

            return PartialView(messages);
            

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