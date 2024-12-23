using System;
using System.Collections.Generic;
using System.Linq;
using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using PagedList;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Drawing;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class CustomProfileHandlerController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
       
        //
        // GET: /CustomProfileHandler/
        public ActionResult Index()
        {
            return View();
        }
              
        //Step Two Personal Info
        [HttpGet]
        public ActionResult ProfilePersonDetails()
        {
            
                var viewmodel = new PersonViewModel();
                ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
                ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
                return View(viewmodel);
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfilePersonDetails(PersonViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                if(user==null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    if(viewmodel.Photo==null)
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
                            PersonMiddleName = viewmodel.personmiddlename,
                            PersonLastName = viewmodel.personlastname,
                            Status = viewmodel.Status,
                            AccountId = user.AccountId,
                            IsDeleted = false,
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId = TosaveImage.ImageId

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");

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
                            PersonMiddleName = viewmodel.personmiddlename,
                            PersonLastName = viewmodel.personlastname,
                            Status = viewmodel.Status,
                            AccountId = user.AccountId,
                            IsDeleted = false,
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId = TosaveImage.ImageId

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("My Dashboard", "Account");

                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
            

        }

        public ActionResult ProfileImage()
        {
            var user = _authentication.GetUser();
            var person = db.persons.Where(x => x.account.AccountEmailId == user.AccountEmailId).SingleOrDefault();
            if (person == null)
            {
                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
            }
            return PartialView("PersonalPartialView", person);
        }


        public PartialViewResult PersonalPartialView(Persons model)
        {
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult PersonalProfileEdit()
        {
            var user = _authentication.GetUser();
            var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (person == null)
            {
                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");

            }
            var viewmodel = new PersonViewModel(person);
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonalProfileEdit(PersonViewModel viewmodel)
        {
            var user = _authentication.GetUser();
            var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (person == null)
            {
                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
            }
            if (ModelState.IsValid)
            {
                person.PersonFirstName = viewmodel.PersonFirstName;
                person.PersonMiddleName = viewmodel.personmiddlename;
                person.PersonLastName = viewmodel.personlastname;
                person.Status = viewmodel.Status;
                person.GenderId = viewmodel.GenderId;
                person.OccupationId = viewmodel.OccupationId;
                db.SaveChanges();
                return RedirectToAction("ProfileImage", "CustomProfileHandler");
            }
            else
            {
                ModelState.AddModelError("", "Model State Is not valid please check....");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }



        //step Three Contact
        [HttpGet]
        public ActionResult ProfileContactDetails()
        {
            var viewmodel = new ProfileCustomContactViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileContactDetails(ProfileCustomContactViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                    if (person == null)
                    {
                        return RedirectToAction("ProfilePersonDetails", "CustomeProfileHandler");
                    }
                    else
                    {
                        var con = new Contacts()
                        {
                            MobileOne = viewmodel.MobileOne,
                            MobileTwo = viewmodel.MobileTwo,
                            PhoneOne = viewmodel.PhoneOne,
                            PhoneTwo = viewmodel.PhoneTwo,
                            Website = viewmodel.Website,
                            Status = viewmodel.Status,
                            PersonId=person.PersonId,
                            AccountId=user.AccountId
                        };
                        db.contacts.Add(con);
                        db.SaveChanges();
                        var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                        if(address==null)
                        {
                            return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
                        }
                        return RedirectToAction("MyDashboard", "Account");
                    }
                }

            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.....");
            }
            return View(viewmodel);
        }

        public ActionResult DisplayContactInfo()
        {
            var user = _authentication.GetUser();
            var Con = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (Con == null)
            {
                return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");

            }
            var viewmodel = new ProfileCustomContactViewModel(Con);
            return PartialView("ContactPartialView", viewmodel);
        }

        public PartialViewResult ContactPartialView(ProfileCustomContactViewModel model)
        {
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult ProfileContactEdit()
        {
            var user = _authentication.GetUser();
            var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (contact == null)
            {
                return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
            }
            var viewmodel = new ProfileCustomContactViewModel(contact);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileContactEdit(ProfileCustomContactViewModel viewmodel)
        {
            var user = _authentication.GetUser();
            var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (contact == null)
            {
                return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
            }
            if (ModelState.IsValid)
            {
                contact.MobileOne = viewmodel.MobileOne;
                contact.MobileTwo = viewmodel.MobileTwo;
                contact.PhoneOne = viewmodel.PhoneOne;
                contact.PhoneTwo = viewmodel.PhoneTwo;
                contact.Website = viewmodel.Website;
                contact.account.AccountEmailId = viewmodel.emailid;
                contact.Status = viewmodel.Status;
                db.SaveChanges();
                return RedirectToAction("DisplayContactInfo", "CustomProfileHandler");
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check....");
            }
            return View(viewmodel);
        }



        //Step 4 Address
        [HttpGet]
        public ActionResult ProfileAddressDetails()
        {
            var viewmodel = new CustomAddressProfileViewModel();
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileAddressDetails(CustomAddressProfileViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                var person = db.persons.Where(x => x.AccountId == user.AccountId).FirstOrDefault();
                if(person==null)
                {
                    return RedirectToAction("ProfilePersonDetails", "CustomeProfileHandler");
                }
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                        var address = new Addresses() { 
                        AddressOne=viewmodel.AddressOne,
                        AddressTwo=viewmodel.AddressTwo,
                        Status=viewmodel.Status,
                        CountryId=viewmodel.CountryId,
                        StateId=viewmodel.StateId,
                        CityId=viewmodel.CityId,
                        AccountId=user.AccountId,
                        PersonId=person.PersonId
                        
                        };
                        db.addresses.Add(address);
                        var contact = db.contacts.Where(x => x.PersonId == person.PersonId).First();
                        var profile = new Profiles() {
                        AccountId=user.AccountId,
                        PersonId=person.PersonId,
                        AddressId=address.AddressId,
                        ContactId=contact.ContactId,
                        Status=true
                        
                        };
                        db.profiles.Add(profile);
                        db.SaveChanges();
                        return RedirectToAction("MyDashBoard", "Account");
                    
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please check....");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);

        }

        public ActionResult DisplayAdressInfo()
        {
            var user = _authentication.GetUser();
            var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (address == null)
            {
                return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");

            }
            var viewmodel = new CustomAddressProfileViewModel(address);
            return PartialView("AddressPartialView", viewmodel);
        }

        public PartialViewResult AddressPartialView(CustomAddressProfileViewModel model)
        {
            return PartialView(model);
        }

        [HttpGet]
        public ActionResult ProfileAddressEdit()
        {
            var user = _authentication.GetUser();
            var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if(address==null)
            {
                return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
            }
            var viewmodel = new CustomAddressProfileViewModel(address);
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileAddressEdit(CustomAddressProfileViewModel viewmodel)
        {
            var user = _authentication.GetUser();
            var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (address == null)
            {
                return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
            }
            if(ModelState.IsValid)
            {
                address.AddressOne = viewmodel.AddressOne;
                address.AddressTwo = viewmodel.AddressTwo;
                address.Status = viewmodel.Status;
                address.CountryId = viewmodel.CountryId;
                address.StateId = viewmodel.StateId;
                address.CityId = viewmodel.CityId;
                db.SaveChanges();
                return RedirectToAction("DisplayAdressInfo", "CustomProfileHandler");
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check....");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            return View(viewmodel);
        }

        //Change Password
        [HttpGet]
        public ActionResult ChangeProfilePassword()
        {
            var user = _authentication.GetUser();
            if(user==null)
            {
                return RedirectToAction("Login", "Account");
            }
            var viewmodel = new ProfileChangePaasswordViewmodel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfilePassword(ProfileChangePaasswordViewmodel Viewmodel)
        {
            if(ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                var pwd = Extensions.Encrypt(Viewmodel.PreviousPassword);
                if (user.AccountPassword == pwd)
                {
                    var acc = db.accounts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                    var newpwd = Extensions.Encrypt(Viewmodel.NewPassword);
                    acc.AccountPassword = newpwd;
                    db.SaveChanges();
                    TempData["notice"] = "Successfully registered";
                    return RedirectToAction("MyDashBoard", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Previous Password Is INCORRECT!!! Please Check...");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is InValid Please check....");
            }
            return View(Viewmodel);
        }

        //Change Profile Image
        [HttpGet]
        public ActionResult ChangeProfileImage()
        {
            var user = _authentication.GetUser();
            var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if(person==null)
            {
                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
            }
            var viewmodel = new ImageUploader();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfileImage(ImageUploader picture)
        {
            var user = _authentication.GetUser();
            var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
            if (person == null)
            {
                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
            }
            if (picture.Photo == null)
            {
                ModelState.AddModelError("", "Model State Is Not Valid....");                

            }
            else
            {
                var uploadImage = new Bitmap(picture.Photo.InputStream);
                var newimage = Extensions.ResizeImage(uploadImage);
                var imagebytes = Extensions.ImageToByte(newimage);
                person.image.Image = imagebytes;
                db.SaveChanges();
                return RedirectToAction("MyDashBoard", "Account");


            }
            return View(picture);
        }
    

        //Message Handler part
        public ActionResult ProfileMessages(string searchString, string currentFilter, int? page, string sortOrder)
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
            var user = _authentication.GetUser();
            var catgories = from c in db.msgreceiver
                            where c.IsDeleted == false && c.AccountId == user.AccountId
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.msgdetails.msg.MessageSubject.Contains(searchString) ||c.msgsender.acc.AccountEmailId.Contains(searchString)||c.msgdetails.MessageDetails.Contains(searchString)
                     && c.IsDeleted == false);

            }
            catgories = catgories.OrderBy(x => x.IsRead);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return PartialView("ProfileAccountMessages", catgories.ToPagedList(pageNumber, pageSize));

        }

        public PartialViewResult ProfileAccountMessages(IPagedList<MsgReceiver> messages)
        {
            return PartialView(messages);
        }


        public ActionResult ReadMessage(int Id)
        {
            var msg = db.msgreceiver.Where(x => x.MeesageReceiverId == Id).SingleOrDefault();
            if(msg==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Message Not Exists....");
            }
            var allmsges = db.msgreceiver.Where(x => x.msgdetails.MessageId == msg.msgdetails.MessageId && x.IsRead == false).ToList();
            foreach(var items in allmsges)
            {
                items.IsRead = true;
            }
            db.SaveChanges();
            var viewmodel = new MessageReadViewmodel(msg);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadMessage(int Id,MessageReadViewmodel viewmodel)
        {
            var msgreciver = db.msgreceiver.Where(x => x.MeesageReceiverId == Id).SingleOrDefault();
            if(msgreciver==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Message Not Exists....");
            }
            if(ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                var msgdetails = new MsgDetails() {
                MessageDetails=viewmodel.replytext,
                DateNTime=DateTime.Now,
                MessageId=msgreciver.msgdetails.MessageId,
                IsDeleted=false,
                
                };
                db.msgdetails.Add(msgdetails);
                var newmsgsender = new MsgSender() { 
                AccountId=user.AccountId,
                IsDeleted=false,
                MessagesDetailsId=msgdetails.MessagesDetailsId,
               
                };
                db.msgsender.Add(newmsgsender);
                var newreciver = new MsgReceiver()
                {
                    MessagesDetailsId=msgdetails.MessagesDetailsId,
                    AccountId=msgreciver.msgsender.AccountId,
                    IsDeleted=false,
                    IsRead=false,
                    MessageSenderId=newmsgsender.MessageSenderId
                };
                db.msgreceiver.Add(newreciver);
                var conversation = new MsgConversation() { 
                MessageSenderId=newmsgsender.MessageSenderId,
                IsDeleted=false,
                MessagesDetailsId=msgdetails.MessagesDetailsId,
                MessageId = msgreciver.msgdetails.MessageId
                
                };
                db.msgconversation.Add(conversation);
                db.SaveChanges();
                return RedirectToAction("ProfileMessages");
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please check....");
            }
            return View(viewmodel);
        }




        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var msgrecive = db.msgreceiver.Where(x => x.MeesageReceiverId == Id).SingleOrDefault();
            if(msgrecive==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Message Does Not Exists Please check.....");
            }
            return View();
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int Id)
        {
            var msgrecive = db.msgreceiver.Where(x => x.MeesageReceiverId == Id).SingleOrDefault();
            if (msgrecive == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Message Does Not Exists...Please check...");
            }
            try
            {
                msgrecive.IsDeleted =true;
                db.SaveChanges();
                return RedirectToAction("ProfileMessages");

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "There is some exception to delete the message please check "+ex.ToString());
            }
            return View();
        }

      //Json Calls

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


        //Ads management
        public ActionResult Myads(string searchString, string currentFilter, int? page, string sortOrder)
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
            var user = _authentication.GetUser();

            ViewBag.CurrentFilter = searchString;

            var catgories = from c in db.submitedads
                            where c.IsDeleted == false && c.AccountId==user.AccountId
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.propertyad.PropertyTitle.Contains(searchString)&& c.IsDeleted == false);

            }
            catgories = catgories.OrderByDescending(x => x.AdId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));

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