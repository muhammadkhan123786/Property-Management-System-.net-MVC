using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using PagedList;
using System.Net;
using System.Drawing;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class PersonController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /Person/
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

            var catgories = from c in db.persons
                            where c.IsDeleted == false
                            select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                catgories = catgories.Where(c => c.PersonFirstName.Contains(searchString) || c.PersonMiddleName.Contains(searchString)
                    || c.PersonLastName.Contains(searchString)&& c.IsDeleted == false);

            }
            catgories = catgories.OrderByDescending(x => x.PersonId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new PersonViewModel(person);
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", person.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", person.GenderId);

            return View(viewmodel);


        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new PersonViewModel();
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PersonViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                if(viewmodel.EmailId==null)
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
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId=TosaveImage.ImageId,
                            IsDeleted = false

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Person");
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
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId = TosaveImage.ImageId,
                            IsDeleted = false

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Person");
                    }
                }
                else
                {
                    if(IsemailExists(viewmodel.EmailId))
                    {
                        var role = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                        if(viewmodel.Photo==null)
                        {
                            var acc = new Accounts()
                            {
                                AccountEmailId = viewmodel.EmailId,
                                AccountPassword = Extensions.Encrypt(viewmodel.Password),
                                IsActive = true,
                                IsVarified = true,
                                DateofCreation = DateTime.Now,
                                RoleId = role.RoleId,
                                IsDeleted = false


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
                                PersonFirstName = viewmodel.PersonFirstName,
                                PersonMiddleName = viewmodel.personmiddlename,
                                PersonLastName = viewmodel.personlastname,
                                GenderId = viewmodel.GenderId,
                                OccupationId = viewmodel.OccupationId,
                                ImageId = TosaveImage.ImageId,
                                AccountId = acc.AccountId,
                                IsDeleted = false

                            };
                            db.persons.Add(person);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Person");
                          


                        }
                        else
                        {
                            var acc = new Accounts()
                            {
                                AccountEmailId = viewmodel.EmailId,
                                AccountPassword = Extensions.Encrypt(viewmodel.Password),
                                IsActive = true,
                                IsVarified = true,
                                DateofCreation = DateTime.Now,
                                RoleId = role.RoleId,
                                IsDeleted = false


                            };
                            db.accounts.Add(acc);
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
                                GenderId = viewmodel.GenderId,
                                OccupationId = viewmodel.OccupationId,
                                ImageId = TosaveImage.ImageId,
                                AccountId = acc.AccountId,
                                IsDeleted = false


                            };
                            db.persons.Add(person);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Person");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Email Id Already Associated another account please check...");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid.....");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? Id,PersonViewModel Viewmodel)
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
                    person.PersonFirstName = Viewmodel.PersonFirstName;
                    person.PersonMiddleName = Viewmodel.personmiddlename;
                    person.PersonLastName = Viewmodel.personlastname;
                    person.GenderId = Viewmodel.GenderId;
                    person.OccupationId = Viewmodel.OccupationId;
                    person.account.AccountEmailId = Viewmodel.EmailId;
                    person.Status = Viewmodel.Status;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Person");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There is some exception this" + ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid...");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", person.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", person.GenderId);
            return View(Viewmodel);

        }

        public ActionResult Details(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        
            return View(person);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.EmailId = person.PersonFullName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int? Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            person.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Person");
        }

        [HttpGet]
        public ActionResult ChangeProfileImage(int Id)
        {
            var con = db.images.Where(x => x.ImageId == Id).SingleOrDefault();
            if (con == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var imageuploders = new ImageUploader();
            return View(imageuploders);

        }

        [HttpPost, ActionName("ChangeProfileImage")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeProfileImage(int? Id, ImageUploader picture)
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
                            return RedirectToAction("Index", "Person");

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
        public ActionResult AddAccount(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Person is null please check...");
            }
            var viewmodel = new AccountViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccount(int Id,AccountViewModel viewmodel)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Person is null please check...");
            }
            if(ModelState.IsValid)
            {
                if (IsemailExists(viewmodel.AccountEmailId))
                {
                    var roleid = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                    var pwd = Extensions.Encrypt(viewmodel.AccountPassword);
                    var accoutn = new Accounts()
                    {
                        AccountEmailId = viewmodel.AccountEmailId,
                        AccountPassword = pwd,
                        DateofCreation = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                        IsVarified = true,
                        RoleId = roleid.RoleId

                    };
                    person.AccountId = accoutn.AccountId;
                    db.accounts.Add(accoutn);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Person");
                }
                else
                {
                    ModelState.AddModelError("", "The email id already associated another account");
                }

            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid..please check ");
            }
            return View(viewmodel);

        }

        [HttpGet]
        public ActionResult EditWithOutAccount(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Person is null please check...");
            }
            var viewmodel = new EditPersonWithoutAccount(person);
            ViewBag.occupationId = new SelectList(db.occupations.Where(x=>x.Status==true).ToList(), "OccupationId", "OccupationName",viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders.Where(x=>x.Status==true).ToList(), "GenderId", "GenderName",viewmodel.GenderId);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditWithOutAccount(int Id,EditPersonWithoutAccount viewmodel)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if(person==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Person is null please check...");
            }
            if(ModelState.IsValid)
            {
                try
                {
                    person.PersonFirstName = viewmodel.PersonFirstName;
                    person.PersonMiddleName = viewmodel.personmiddlename;
                    person.PersonLastName = viewmodel.personlastname;
                    person.GenderId = viewmodel.GenderId;
                    person.OccupationId = viewmodel.OccupationId;
                    person.Status = viewmodel.Status;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Person");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", "There is some exception please check...."+ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check....");
            }
            ViewBag.occupationId = new SelectList(db.occupations.Where(x => x.Status == true).ToList(), "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders.Where(x => x.Status == true).ToList(), "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }

        public bool IsemailExists(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email&&x.IsDeleted==false).Count() > 0)
            {
                return false;
            }
            else
            { return true ; }
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