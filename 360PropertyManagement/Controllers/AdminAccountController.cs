using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using PagedList;
using System.Web.Routing;
using _360PropertyManagement.ViewModels;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Net.Mail;
using System.Data.Entity;
using System.IO;
using System.Drawing;

namespace _360PropertyManagement.Controllers
{
    public class AdminAccountController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /AdminAccount/
        //General Part
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
            
                var catgories = from c in db.accounts
                                where
                                    c.role.RoleName != "SuperAdmin" && c.IsDeleted == false
                                select c;
                if (!String.IsNullOrEmpty(searchString))
                {
                    catgories = catgories.Where(c => c.AccountEmailId.Contains(searchString) && c.role.RoleName != "SuperAdmin" && c.IsDeleted == false);

                }
                catgories = catgories.OrderByDescending(x => x.AccountId);

                int pageSize = 6;
                int pageNumber = (page ?? 1);
                return View(catgories.ToPagedList(pageNumber, pageSize));
            
}
   
        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new AccountViewModel();
            return View(viewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(AccountViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {

                if (accountexists(viewmodel.AccountEmailId))
                {
                    var user = new Accounts() { AccountEmailId = viewmodel.AccountEmailId };
                    user.AccountEmailId = viewmodel.AccountEmailId;
                    var UserRoleId = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();
                    var result = false;
                    var pwd = Extensions.Encrypt(viewmodel.AccountPassword);
                    var account = new Accounts()
                    {
                        AccountEmailId = viewmodel.AccountEmailId,
                        AccountPassword = pwd,
                        IsVarified = true,
                        IsActive = true,
                        RoleId=UserRoleId.RoleId,
                        DateofCreation = DateTime.Now,
                        IsDeleted=false
                    };
                    db.accounts.Add(account);
                    var profile = new Profiles() {
                      AccountId=account.AccountId,
                      Status=true
                    
                    };
                    db.profiles.Add(profile);
                    db.SaveChanges();
                    result = true;
                    if (result)
                    {
                        return RedirectToAction("Index", "AdminAccount");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The record successfully not saved please check!");
                    }
                }
                ModelState.AddModelError("", "Email id Already Exists!");
            }
            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult Details(int Id)
        {

            Accounts acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(acc);
        }
        
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
            ViewBag.EmailId = acc.AccountEmailId;
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Accounts acc = db.accounts.Find(id);
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            acc.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminAccount");
        }

        [HttpGet]
        public ActionResult MakeAdmin(int id)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
            ViewBag.EmailId = acc.AccountEmailId;
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View();
        }

        [HttpPost, ActionName("MakeAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult MakeAdminConfirmed(int id)
        {
            Accounts acc = db.accounts.Find(id);
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var roleid = db.roles.Where(x => x.RoleName == "Admin").SingleOrDefault();
            acc.RoleId = roleid.RoleId;
            db.SaveChanges();
            return RedirectToAction("Index", "AdminAccount");
        }


        [HttpGet]
        public ActionResult UpdateRole(int Id)
        {
            var account = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            if(account==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.roleid = new SelectList(db.roles.Where(x => x.Status == true&&x.RoleName!="SuperAdmin").ToList(), "RoleId", "RoleName",account.RoleId);
            return View(account);
        }

        [HttpPost,ActionName("UpdateRole")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateRolePost(int? Id)
        {
            var account = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            if(account==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var accountToUpdate = account;
            if (TryUpdateModel(accountToUpdate, "",
           new string[] { "RoleId"}))
                try
                {
                    if (ModelState.IsValid)
                    {
                        var person = db.persons.Where(x => x.AccountId == Id).SingleOrDefault();
                        if (person == null)
                        {
                            return RedirectToAction("PersonAdd", "AdminAccount", new { id = Id });
                        }
         
                        var contact = db.contacts.Where(x => x.AccountId == Id).SingleOrDefault();
                        if(contact==null)
                        {
                            return RedirectToAction("ContactAdd", "AdminAccount", new { id=Id});
                        }
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminAccount");



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

            ViewBag.roleid = new SelectList(db.roles.Where(x => x.Status == true && x.RoleName != "SuperAdmin").ToList(), "RoleId", "RoleName", account.RoleId);
            return View(account);


        }


        [HttpGet]
        public ActionResult ContactAdd(int Id)
        {
            var acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            var person = db.persons.Where(x => x.AccountId == Id).SingleOrDefault();
            if(person==null)
            {
                return RedirectToAction("PersonAdd", "AdminAccount", new { id = Id });
            }
            if(acc==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Please Select Account..");
            }
            var viewmodel = new ProfileCustomContactViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactAdd(int Id, ProfileCustomContactViewModel viewmodel)
        {
            var acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            var accperson = db.persons.Where(x => x.AccountId == Id).SingleOrDefault();
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please Select Account..");
            }
            if(ModelState.IsValid)
            {
                if (db.contacts.Where(x => x.MobileOne == viewmodel.MobileOne).Count() > 0)
                {
                     ModelState.AddModelError("", "Mobile Already Assigns another account...please check..");
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
                        IsDeleted = false,
                        PersonId=accperson.PersonId,
                        Status = true,
                        AccountId = acc.AccountId

                    };
                    db.contacts.Add(con);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please Check.....");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult PersonAdd(int Id)
        {
            var acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            if(acc==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please Select Account..");
            }
            var viewmodel = new PersonViewModel();
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
           
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonAdd(int Id,PersonViewModel viewmodel)
        {
            var acc = db.accounts.Where(x => x.AccountId == Id).SingleOrDefault();
            if(acc==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Please Select Account..");
            }
            if(ModelState.IsValid)
            {
                if(db.persons.Where(x=>x.AccountId==Id).Count()>0)
                {
                    ModelState.AddModelError("", "Person Already added aganst this account");
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
                            GenderId = viewmodel.GenderId,
                            OccupationId = viewmodel.OccupationId,
                            ImageId = TosaveImage.ImageId,
                            AccountId=acc.AccountId,
                            IsDeleted = false

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminAccount");
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
                            AccountId=acc.AccountId,
                            ImageId = TosaveImage.ImageId,
                            IsDeleted = false

                        };
                        db.persons.Add(person);
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminAccount");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please Check....");
            }
            ViewBag.occupationId = new SelectList(db.occupations, "OccupationId", "OccupationName", viewmodel.OccupationId);
            ViewBag.genderId = new SelectList(db.genders, "GenderId", "GenderName", viewmodel.GenderId);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
           if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           ViewBag.isactive = acc.IsActive;
           var accountmodel = new AccountViewModel(acc);
            return View(accountmodel);
        }


        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? Id)
        {
            var cat = db.accounts.Find(Id);
            if (cat == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var orderToUpdate = cat;
            if (TryUpdateModel(orderToUpdate, "",
            new string[] { "AccountEmailId", "IsActive" }))
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index", "AdminAccount");



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


            return View(cat);

        }


        [HttpGet]
        public ActionResult SendEmail(int id)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var emailviewmodel = new EmailSendViewModel();
            emailviewmodel.emailid = acc.AccountEmailId;
            return View(emailviewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail(int id, EmailSendViewModel viewmodel)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
            var result = false;
            if (ModelState.IsValid)
            {
                if (acc.IsVarified)
                {
                    var newemail = new SendEmails()
                    {
                        EmailSubject = viewmodel.EmailSubject,
                        EmailMessage = viewmodel.EmailMessage,
                        SendOnDate = DateTime.Now,
                        AccountId = id,
                        EmailId = acc.AccountEmailId
                    };
                    db.emails.Add(newemail);
                    db.SaveChanges();
                    result = true;
                    if (result)
                    {
                        string msgbody = viewmodel.EmailMessage;
                        StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/Customemail.html"));
                        string readfile = reader.ReadToEnd();
                        string StrContent = "";
                        StrContent = readfile;
                        StrContent = StrContent.Replace("[EmailId]", newemail.account.AccountEmailId);
                        StrContent = StrContent.Replace("[Message]", viewmodel.EmailMessage);                         
                        MailMessage msg = new MailMessage(
                        new MailAddress("onyxtech786@gmail.com", "360Dynamics.net Property Management System"), new MailAddress(viewmodel.emailid));
                        msg.Subject = viewmodel.EmailSubject;
                        msg.Body = StrContent.ToString();
                        msg.IsBodyHtml = true;
                        SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                        sendvia.EnableSsl = true;
                        sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                        sendvia.Send(msg);
                        System.Threading.Thread.Sleep(2000);
                        return RedirectToAction("SendSuccessfully", "AdminAccount", new { Email = viewmodel.emailid });

                    }
                }
                else
                {
                    ModelState.AddModelError("", "Your account is not varified or not active please contact to site admin.Thank you");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");

            }
            return View(viewmodel);
        }


        public ActionResult SendSuccessfully(string email)
        {
            ViewBag.emailid = email;
            return View();
        }
        
        
        
        public JsonResult GetAccounts(string term)
        {
            List<string> accounts;
            accounts = db.accounts.Where(x => x.AccountEmailId.StartsWith(term))
                .Select(y => y.AccountEmailId).ToList();
            return Json(accounts, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public ActionResult ChangePassword(int id)
        {
            var changepwdviewmodel = new ChangePasswordViewmodel();
            changepwdviewmodel.AccountId = id;
            return View(changepwdviewmodel);
        }


        [HttpPost, ActionName("ChangePassword")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(int? id, ChangePasswordViewmodel viewmodel)
        {
            var user = db.accounts.Where(s => s.AccountId == viewmodel.AccountId).SingleOrDefault();
            if(user==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The User is null....please check");
            }
            if (ModelState.IsValid)
            {
              
                var pwd = Extensions.Encrypt(viewmodel.NewPassword);
                user.AccountPassword = pwd;
                db.SaveChanges();
                return RedirectToAction("Index", "AdminAccount");
            }
            else
            {
                ModelState.AddModelError("", "There is something wrong!!");
            }

            return View(viewmodel);
          }

        [HttpGet]
        public ActionResult SendMessage(int id)
        {
            var acc = db.accounts.Where(x => x.AccountId == id).SingleOrDefault();
            if (acc == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var messageviewmodel = new SendMessageViewModel();
            messageviewmodel.AccountId = acc.AccountId;
            return View(messageviewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(int id, SendMessageViewModel viewmodel)
        {

            var result = false;
            if (ModelState.IsValid)
            {
                var user = _authentication.GetUser();
                var newemail = new SendMessages()
                {
                    MessageSubject = viewmodel.MessageSubject
                    
                };
                db.sendmessages.Add(newemail);
                var msgdetails = new MsgDetails()
                {
                    MessageDetails = viewmodel.Message,
                    DateNTime = DateTime.Now,
                    MessageId = newemail.MessageId,
                    IsDeleted = false

                };
                db.msgdetails.Add(msgdetails);
                var msgsender = new MsgSender()
                {
                    IsDeleted = false,
                    MessagesDetailsId = msgdetails.MessagesDetailsId,
                    AccountId = user.AccountId
                };
                db.msgsender.Add(msgsender);
                var msgreciver = new MsgReceiver()
                {
                    IsDeleted = false,
                    IsRead = false,
                    AccountId = id,
                    MessagesDetailsId = msgdetails.MessagesDetailsId,
                    MessageSenderId = msgsender.MessageSenderId
                };
                db.msgreceiver.Add(msgreciver);
                var msgconversation = new MsgConversation()
                {
                    MessageSenderId = msgsender.MessageSenderId,
                    IsDeleted = false,
                    MessageId = newemail.MessageId,
                    MessagesDetailsId = msgdetails.MessagesDetailsId
                };
                db.msgconversation.Add(msgconversation);
                db.SaveChanges(); 
                result = true;
                if (result)
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");

            }
            return View(viewmodel);
        }

          
        
        [HttpGet]
        public ActionResult SendEmailsSuccessfully()
        {
            return View();
        }

        

        [HttpGet]
        public ActionResult BulkMessages()
        {
                var accounts = db.accounts.Where(x =>x.IsActive == true && x.IsVarified == true && x.IsDeleted == false).ToList();
                if(accounts==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Accounts List is null please check....");
                }
                var msgviewmodel = new SendBulkMessages();
                return View(msgviewmodel);
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BulkMessages(SendBulkMessages viewmodel)
        {
            var result = false;
                var accounts = db.accounts.Where(x => x.IsVarified == true && x.IsActive == true && x.IsDeleted==false).ToList();
                if(accounts==null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Accounts List Is Null please check.....");
                }
                if (ModelState.IsValid)
                {
                    var user = _authentication.GetUser();

                    foreach (var items in accounts)
                    {
                        var newemail = new SendMessages()
                        {
                            MessageSubject = viewmodel.MessageSubject,
                        };
                        db.sendmessages.Add(newemail);
                       var msgdetails = new MsgDetails()
                        {
                            MessageDetails = viewmodel.Message,
                            DateNTime = DateTime.Now,
                            MessageId = newemail.MessageId,
                            IsDeleted = false,

                        };
                        db.sendmessages.Add(newemail);
                        var msgsender = new MsgSender()
                        {
                            IsDeleted = false,
                            MessagesDetailsId = msgdetails.MessagesDetailsId,
                            AccountId = user.AccountId
                        };
                        db.msgsender.Add(msgsender);
                        var msgreciver = new MsgReceiver()
                        {
                            IsDeleted = false,
                            IsRead = false,
                            AccountId = items.AccountId,
                            MessagesDetailsId = msgdetails.MessagesDetailsId,
                            MessageSenderId = msgsender.MessageSenderId
                        };
                        db.msgreceiver.Add(msgreciver);
                        var msgconversation = new MsgConversation()
                        {
                            MessageSenderId = msgsender.MessageSenderId,
                            IsDeleted = false,
                            MessageId = newemail.MessageId,
                            MessagesDetailsId = msgdetails.MessagesDetailsId
                        };
                        db.SaveChanges(); 
                        db.msgdetails.Add(msgdetails);
                        result = true;

                    }
                    if (result)
                    {
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Model state is not valid please check");
                }

                return View(viewmodel);
}
            

       
        public bool accountexists(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email&&x.IsDeleted==false).Count() > 0)
            {
                return false;
            }
            return true;
        }

       protected override void OnActionExecuting(ActionExecutingContext ctx)
        {
            // if (!Request.IsAuthenticated)
            if (_authentication.IsUserAdmin()||_authentication.IsUserSuperAdmin())
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