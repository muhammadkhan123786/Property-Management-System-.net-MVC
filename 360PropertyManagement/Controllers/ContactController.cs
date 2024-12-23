using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using _360PropertyManagement.ViewModels;
using System.Drawing;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class ContactController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
       
        //
        // GET: /Contact/
        public ViewResult Index(string searchString, string currentFilter, int? page, string sortOrder)
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

            var catgories = from c in db.contacts
                            where c.IsDeleted==false
                            select c;
            

            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.MobileOne.Contains(searchString)||c.person.account.AccountEmailId.ToString().Contains(searchString)
                    ||c.person.PersonFirstName.Contains(searchString)||c.person.PersonMiddleName.Contains(searchString)||c.person.PersonLastName.Contains(searchString)&&c.IsDeleted==false);

            }
            catgories = catgories.OrderByDescending(x => x.ContactId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Add()
        {
           ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName");
           ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(ContactViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (contactexists(viewmodel.MobileOne))
                {
                    if (emailidexists(viewmodel.Email_Id))
                    {
                        if (viewmodel.Photo == null)
                        {
                            var role = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                            var acc = new Accounts()
                            {
                                AccountEmailId = viewmodel.Email_Id,
                                AccountPassword = Extensions.Encrypt(viewmodel.Password),
                                IsActive = true,
                                IsVarified = true,
                                DateofCreation = DateTime.Now,
                                RoleId = role.RoleId,
                                IsDeleted=false


                            };
                            db.accounts.Add(acc);
                            var imagenot = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                            var TosaveImage = new Images()
                            {
                                Image = imagenot.ImageNotAvailable
                            };
                            db.images.Add(TosaveImage);
                            var person = new Persons()
                            {
                                PersonFirstName = viewmodel.Firstname,
                                PersonMiddleName = viewmodel.Secondname,
                                PersonLastName = viewmodel.lastname,
                                GenderId = viewmodel.GenderId,
                                OccupationId = viewmodel.OccupationId,
                                ImageId = TosaveImage.ImageId,
                                AccountId = acc.AccountId,
                                IsDeleted=false

                            };
                            db.persons.Add(person);

                            var contact = new Contacts()
                            {
                                MobileOne = viewmodel.MobileOne,
                                MobileTwo = viewmodel.MobileTwo,
                                PersonId = person.PersonId,
                                PhoneOne = viewmodel.PhoneOne,
                                PhoneTwo = viewmodel.PhoneTwo,
                                Status = viewmodel.Status,
                                Website = viewmodel.Website,
                                IsDeleted=false

                            };
                            db.contacts.Add(contact);
                            var profile = new Profiles()
                            {
                                AccountId = acc.AccountId,
                                PersonId = person.PersonId,
                                ContactId = contact.ContactId,
                                IsDeleted=false
                            };
                            db.profiles.Add(profile);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Contact");
                    }
                        else
                        {
                            var role = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                            var acc = new Accounts()
                            {
                                AccountEmailId = viewmodel.Email_Id,
                                AccountPassword = Extensions.Encrypt(viewmodel.Password),
                                IsActive = true,
                                IsVarified = true,
                                DateofCreation = DateTime.Now,
                                RoleId = role.RoleId,
                                IsDeleted=false


                            };
                            db.accounts.Add(acc);

                            
                            var uploadImage = new Bitmap(viewmodel.Photo.InputStream);
                            var newimage = Extensions.ResizeImage(uploadImage);
                            var imagebytes = Extensions.ImageToByte(newimage);
                            var TosaveImage = new Images()
                            {
                                Image = imagebytes,
                                IsDeleted=false
                            };
                            db.images.Add(TosaveImage);
                            var person = new Persons()
                            {
                                PersonFirstName = viewmodel.Firstname,
                                PersonMiddleName = viewmodel.Secondname,
                                PersonLastName = viewmodel.lastname,
                                GenderId = viewmodel.GenderId,
                                OccupationId = viewmodel.OccupationId,
                                ImageId = TosaveImage.ImageId,
                                AccountId = acc.AccountId,
                                IsDeleted=false


                            };
                            db.persons.Add(person);

                            var contact = new Contacts()
                            {
                                MobileOne = viewmodel.MobileOne,
                                MobileTwo = viewmodel.MobileTwo,
                                PersonId = person.PersonId,
                                PhoneOne = viewmodel.PhoneOne,
                                PhoneTwo = viewmodel.PhoneTwo,
                                Status = viewmodel.Status,
                                Website = viewmodel.Website,
                                IsDeleted=false
                            };
                            db.contacts.Add(contact);
                            var profile = new Profiles()
                            {
                                AccountId = acc.AccountId,
                                PersonId = person.PersonId,
                                ContactId = contact.ContactId,
                                IsDeleted=false
                            };
                            db.profiles.Add(profile);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Contact");
                     }
                    }
                    else
                    {
                        ModelState.AddModelError("", "This Email Id already Associated with another Account.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The Mobile Number Already Available");
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid");
            }
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);

            return View(viewmodel);

        }

        [HttpGet]
        public ActionResult AddAccount(int Id)
        {
            var accountviewmodel = new AccountViewModel();
            var con = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if(con==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(accountviewmodel);
        }

        [HttpPost, ActionName("AddAccount")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccountPost(int? Id,AccountViewModel viewmoadl)
        {
            if(ModelState.IsValid)
            {
                if (emailidexists(viewmoadl.AccountEmailId))
                {
                    var role = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                    var acc = new Accounts()
                    {
                        AccountEmailId=viewmoadl.AccountEmailId,
                        DateofCreation=DateTime.Now,
                        IsActive=true,
                        IsVarified=true,
                        RoleId=role.RoleId,
                        IsDeleted=false


                    };
                    db.accounts.Add(acc);
                    var con = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
                    con.person.AccountId = acc.AccountId;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Contact");

                }
                    else
                {
                    ModelState.AddModelError("", "The Email Id Already Associated another account!");
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid please check!");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {

            Contacts cont = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if (cont == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                byte[] byteArray = db.images.Find(cont.person.image.ImageId).Image;
                 if(byteArray != null){
                    FileContentResult imagecontent= new FileContentResult(byteArray, "image/jpeg");
                    ViewBag.image = imagecontent;    
                }
                
            
            return View(cont);
        }

        [HttpGet]
        public ActionResult EditContactInfo(int Id)
        {
            var con = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if(con==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contact = new ContactViewModel(con);
            return View(contact);
        }

        [HttpPost,ActionName("EditContactInfo")]
        [ValidateAntiForgeryToken]
        public ActionResult EditContactInfo(int? Id)
        {
            var con = db.contacts.Where(x => x.ContactId == Id).SingleOrDefault();
            if (con == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ContactToUpdate = con;
                      if (TryUpdateModel(ContactToUpdate, "",
                         new string[] { "MobileOne", "MobileTwo","PhoneOne","PhoneTwo","Website","Status" }))
                          try
                          {
                              if (ModelState.IsValid)
                              {
                                  db.SaveChanges();
                                  return RedirectToAction("Index", "Contact");
                              }
                              else
                              {
                                  ModelState.AddModelError("", "The Model State is not valid!!");
                              }
                          }

                          catch (RetryLimitExceededException)
                          {
                              ModelState.AddModelError("", "Some Model Exception is occured!!");

                          }

                      return View(con);

           
        }

        [HttpGet]
        public ActionResult EditPersonalInfo(int Id)
        {
            
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var contact = new PersonViewModel(person);
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", person.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", person.GenderId);

            return View(contact);
        }

        [HttpPost, ActionName("EditPersonalInfo")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPersonalInfo(int? Id,PersonViewModel Viewmodel)
        {
            
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            try
                {
                    if (ModelState.IsValid)
                    {
                        person.PersonFirstName = Viewmodel.PersonFirstName;
                        person.PersonMiddleName = Viewmodel.personmiddlename;
                        person.PersonLastName = Viewmodel.personlastname;
                        person.OccupationId = Viewmodel.OccupationId;
                        person.GenderId = Viewmodel.GenderId;
                        db.SaveChanges();
                        return RedirectToAction("Index", "Contact");



                    }
                    else
                    {
                        ModelState.AddModelError("", "The Model State is not valid!!");
                    }
                }

                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Some Model Exception is occured!!");

                }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", person.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", person.GenderId);

            return View(person);


        }



        public bool contactexists(string mobilenumber)
        {
            if (db.contacts.Where(x => x.MobileOne == mobilenumber).Count() > 0)
            {
                return false;
            }
            return true;
        }

        public bool emailidexists(string emailid)
        {
            if(db.accounts.Where(x=>x.AccountEmailId==emailid&&x.IsDeleted==false).Count()>0)
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var acc = db.contacts.Where(x => x.ContactId == id).SingleOrDefault();
            ViewBag.MobileNumber = acc.MobileOne;
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, bool checkResp=false)
        {

            Contacts acc = db.contacts.Find(id);
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
                var person = db.persons.Where(x => x.PersonId == acc.PersonId).SingleOrDefault();
                var account = db.accounts.Where(x => x.AccountId == acc.person.AccountId).SingleOrDefault();
                acc.IsDeleted = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            
            
            
        }

        
        [HttpGet]
        public ActionResult ChangeProfileImage(int Id)
        {
            var con = db.images.Where(x => x.ImageId == Id).SingleOrDefault();
            if(con==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageuploders = new ImageUploader();
            return View(imageuploders);

        }

        [HttpPost, ActionName("ChangeProfileImage")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfileImage(int? Id,ImageUploader picture)
        {
            var img = db.images.Where(x => x.ImageId == Id).SingleOrDefault();
            if (img == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (picture.Photo==null )
            {
                ModelState.AddModelError("", "Please upload image");
            }
            else
            {

                var uploadImage = new Bitmap(picture.Photo.InputStream);
                var newimage = Extensions.ResizeImage(uploadImage);
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
                            return RedirectToAction("Index", "Contact");

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
        public ActionResult Uploadimage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadImage(ImageNot image, HttpPostedFileBase picture)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (picture.ContentLength > 0)
                    {
                        var uploadImage = new Bitmap(picture.InputStream);
                        var newimage = Extensions.ResizeImage(uploadImage);
                        var imagebytes = Extensions.ImageToByte(newimage);
                        var TosaveImage = new ImageNot()
                        {
                            ImageNotAvailable = imagebytes,
                            Status=true

                        };
                        db.imagenot.Add(TosaveImage);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Contact");

                    }
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Some thing is went wrong Please check!!" + e.ToString());
            }
            return View(image);
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