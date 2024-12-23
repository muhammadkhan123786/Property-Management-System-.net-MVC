using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using PagedList;
using System.Web.Mvc;
using System.Web.Routing;

namespace _360PropertyManagement.Controllers
{
    public class GenderController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /Gender/
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

            var result = from c in db.genders
                         where c.IsDeleted == false
                         select c;


            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(c => c.GenderName.Contains(searchString) && c.IsDeleted == false);

            }
            result = result.OrderByDescending(x => x.GenderId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new GenderViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(GenderViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (Gendernameexists(viewmodel.GenderName))
                {
                    try
                    {
                        var gender = new Genders()
                        {
                            GenderName = viewmodel.GenderName,
                            Status = viewmodel.Status,
                            IsDeleted = false

                        };
                        db.genders.Add(gender);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Gender");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "There is some exception please check" + ex.ToString());
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The Gender already added");
                }

            }
            else
            {
                ModelState.AddModelError("", "The Model State is not Valid...");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var gender = db.genders.Where(x => x.GenderId == Id).SingleOrDefault();
            if (gender == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"Gender is null please check....");
            }
            var viewmodel = new GenderViewModel(gender);
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? Id, GenderViewModel viewmodel)
        {
            var gender = db.genders.Where(x => x.GenderId == Id).SingleOrDefault();
            if (gender == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    gender.GenderName = viewmodel.GenderName;
                    gender.Status = viewmodel.Status;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Gender");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There is some exception please check.." + ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid....");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var gender = db.genders.Where(x => x.GenderId == Id).SingleOrDefault();
            if (gender == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var viewmodel = new GenderViewModel(gender);
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var gender = db.genders.Where(x => x.GenderId == Id).SingleOrDefault();
            if (gender == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.GenderName = gender.GenderName;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int Id)
        {
            var gender = db.genders.Where(x => x.GenderId == Id).SingleOrDefault();
            if (gender == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            gender.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Gender");

        }

        [HttpGet]
        public ActionResult PersonsByGenderId(int Id, string searchString, string currentFilter, int? page, string sortOrder)
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

            var result = from c in db.persons
                         where c.IsDeleted == false && c.GenderId == Id
                         select c;
            ViewBag.OccupationId = Id;

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(c => c.PersonFirstName.Contains(searchString) && c.IsDeleted == false && c.PersonMiddleName.Contains(searchString) &&
                    c.PersonLastName.Contains(searchString)
                    );

            }
            result = result.OrderByDescending(x => x.PersonId);

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(result.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult SendMessage(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (person.AccountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Person has not Account to read the message, Please create first account then you can send him/her message. Thank you.");
            }
            var messageviewmodel = new SendMessageViewModel();
            messageviewmodel.AccountId = person.AccountId;
            return View(messageviewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendMessage(int id, SendMessageViewModel viewmodel)
        {
            var person = db.persons.Where(x => x.PersonId == id).SingleOrDefault();
            var result = false;
            if (ModelState.IsValid)
            {
                var user = _authentication.GetUser();
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
                    AccountId = user.AccountId,
                };
                db.msgsender.Add(msgsender);
                var msgreciver = new MsgReceiver()
                {
                    IsDeleted = false,
                    IsRead = false,
                    AccountId = person.AccountId,
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
                    return RedirectToAction("PersonsByGenderId", new { id = person.GenderId });
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State is not valid please check.");

            }
            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult PersonDetails(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(person);
        }


        [HttpGet]
        public ActionResult SendEmail(int Id)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The person is null please check...");
            }
            if (person.AccountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The person account not exists please check...");

            }
            var viewmodel = new EmailSendViewModel();
            viewmodel.emailid = person.account.AccountEmailId;
            return View(viewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendEmail(int Id, EmailSendViewModel viewmodel)
        {
            var person = db.persons.Where(x => x.PersonId == Id).SingleOrDefault();
            if (person == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The person is null please check...");
            }
            if (person.AccountId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The person account not exists please check...");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var newemail = new SendEmails()
                    {
                        EmailSubject = viewmodel.EmailSubject,
                        EmailMessage = viewmodel.EmailMessage,
                        SendOnDate = DateTime.Now,
                        AccountId = person.AccountId,
                        EmailId = person.account.AccountEmailId
                    };
                    db.SaveChanges();
                    string msgbody = viewmodel.EmailMessage;
                    StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/Customemail.html"));
                    string readfile = reader.ReadToEnd();
                    string StrContent = "";
                    StrContent = readfile;
                    StrContent = StrContent.Replace("[EmailId]", viewmodel.emailid);
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
                    return RedirectToAction("PersonsByGenderId", "Gender", new { id = person.GenderId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There is some exception please check...." + ex.ToString());
                }
            }
            else
            {
                ModelState.AddModelError("", "The Model State is not valid....");
            }
            return View(viewmodel);
        }

        [HttpGet]
        public ActionResult SendBulkEmails(int Id)
        {
            var persons = db.persons.Where(x => x.GenderId == Id).ToList();
            if (persons.Count > 0)
            {
                var viewmodel = new BulkEmailsViewModel();
                return View(viewmodel);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "There is no any persons in this Gender");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendBulkEmails(int Id, BulkEmailsViewModel viewmodel)
        {
            var result = false;
            var persons = db.persons.Where(x => x.GenderId == Id && x.AccountId != null && x.account.role.RoleName != "Blacklist" && x.account.IsDeleted == false).ToList();
            if (persons.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in persons)
                    {
                        var curntemail = item.account.AccountEmailId;
                        var newemail = new SendEmails()
                        {
                            EmailSubject = viewmodel.EmailSubject,
                            EmailMessage = viewmodel.EmailMessage,
                            SendOnDate = DateTime.Now,
                            AccountId = item.AccountId,
                            EmailId = item.account.AccountEmailId
                        };
                        db.emails.Add(newemail);
                        result = true;
                        if (result)
                        {
                            StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/Customemail.html"));
                            string readfile = reader.ReadToEnd();
                            string StrContent = "";
                            StrContent = readfile;
                            StrContent = StrContent.Replace("[EmailId]", curntemail);
                            StrContent = StrContent.Replace("[Message]", viewmodel.EmailMessage);
                            MailMessage msg = new MailMessage(
                            new MailAddress("onyxtech786@gmail.com", "360Dynamics.net Property Management System"), new MailAddress(item.account.AccountEmailId));
                            msg.Subject = viewmodel.EmailSubject;
                            msg.Body = StrContent.ToString();
                            msg.IsBodyHtml = true;
                            SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                            sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                            sendvia.Send(msg);
                            result = false;

                        }


                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Gender");
                }
                else
                {
                    ModelState.AddModelError("", "Model State Is Not Valid... Please Check.");
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "There is no any persons in this Gender");
            }

            return View(viewmodel);

        }

        [HttpGet]
        public ActionResult SendBulkMessages(int Id)
        {
            var persons = db.persons.Where(x => x.OccupationId == Id).ToList();
            if (persons.Count > 0)
            {
                var viewmodel = new SendBulkMessages();
                return View(viewmodel);

            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "There is no any persons in this Gender");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SendBulkMessages(int Id, SendBulkMessages viewmodel)
        {
            var persons = db.persons.Where(x => x.GenderId == Id && x.AccountId != null && x.account.role.RoleName != "Blacklist" && x.account.IsDeleted == false).ToList();
            if (persons.Count > 0)
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in persons)
                    {
                        var newemail = new SendMessages()
                        {
                            MessageSubject = viewmodel.MessageSubject,
                            
                        };
                        db.sendmessages.Add(newemail);
                       

                    }
                    db.SaveChanges();
                    return RedirectToAction("Index", "Gender");
                }
                else
                {
                    ModelState.AddModelError("", "Model State Is Not Valid Please Check...");
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "There is no any persons in this Gender");
            }
            return View(viewmodel);
        }

        public bool Gendernameexists(string gendername)
        {
            if (db.genders.Where(x => x.GenderName == gendername && x.IsDeleted == false).Count() > 0)
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