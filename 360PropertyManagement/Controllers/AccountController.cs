using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using System.Web.Security;
using System.IO;

namespace _360PropertyManagement.Controllers
{
    public class AccountController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
       
        //
        // GET: /Account/
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            var viewmodel =new AccountViewModel();
            ViewBag.RoleId = new SelectList(db.roles.Where(x => x.Status == true && x.RoleName != "Admin" && x.RoleName != "SuperAdmin").ToList(), "RoleId", "RoleName", viewmodel.RoleId);
            return View(viewmodel);
        }
        
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AccountViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (emailcheckfine(model.AccountEmailId))
                    {
                        if (CheckMailExists(model.AccountEmailId))
                        {
                            var user = new Accounts() { AccountEmailId = model.AccountEmailId };
                            user.AccountEmailId = model.AccountEmailId;
                            var result = false;
                            var pwd = Extensions.Encrypt(model.AccountPassword);
                            var role = db.roles.Where(x => x.RoleName == "User").SingleOrDefault();

                            var account = new Accounts()
                            {
                                AccountEmailId = model.AccountEmailId,
                                AccountPassword = pwd,
                                IsVarified = false,
                                IsActive = true,
                                DateofCreation = DateTime.Now,
                                IsDeleted = false,
                                RoleId = role.RoleId
                            };
                            db.accounts.Add(account);
                            db.SaveChanges();
                            result = true;
                            if (result)
                            {
                                MailMessage msg = new MailMessage(
                                new MailAddress("onyxtech786@gmail.com", "360Dynamics.net Property Management System"), new MailAddress(user.AccountEmailId));
                                msg.Subject = "Emil Confirmation";
                                StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/EmailTemplate.html"));
                                string readfile = reader.ReadToEnd();
                                string StrContent = "";
                                string url = (Url.Action("ConfirmEmail", "Account",
                                           new
                                           {
                                               Token = Extensions.Encrypt(user.AccountId.ToString()),
                                               Email = user.AccountEmailId
                                           }, Request.Url.Scheme));

                                StrContent = readfile;
                                StrContent = StrContent.Replace("[EmailId]", model.AccountEmailId);
                                StrContent = StrContent.Replace("[Link]", url);
                                msg.Body = StrContent.ToString();
                                msg.IsBodyHtml = true;
                                SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                                sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                                sendvia.Send(msg);
                                return RedirectToAction("Confirm", "Account", new { Email = user.AccountEmailId });
                            }
                            else
                            {
                                ModelState.AddModelError("", "Your account details are not successfully save please check!");
                                return View(model);
                            }


                        }
                        else
                        {
                            return RedirectToAction("EmailNotConfirmed", "Account");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email Id Already Associated Another Account");
                    }

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There is something wrong please error" + ex.ToString());
                    ViewBag.RoleId = new SelectList(db.roles.Where(x => x.Status == true && x.RoleName != "Admin" && x.RoleName != "SuperAdmin").ToList(), "RoleId", "RoleName", model.RoleId);
                    return View(model);
                }
           
            }
            ViewBag.RoleId = new SelectList(db.roles.Where(x => x.Status == true && x.RoleName != "Admin" && x.RoleName != "SuperAdmin").ToList(), "RoleId", "RoleName", model.RoleId);
            return View(model);
}
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            var loginview = new LoginViewModel();
            return View(loginview);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewmodel,string returnurl)
        {
            if (ModelState.IsValid)
            {
                var pwd = Extensions.Encrypt(viewmodel.Password);
                if (IsUserValid(viewmodel.EmailId, pwd))
                {
                    if (IsUserBlackList(viewmodel.EmailId))
                    {
                        var user = db.accounts.Where(x => x.AccountEmailId == viewmodel.EmailId && x.AccountPassword == pwd).SingleOrDefault();
                        if(user.IsActive)
                        {
                            if (user.IsDeleted)
                            {
                                ModelState.AddModelError("", "Your account has been deleted");

                            }
                            else
                            {
                                if (user.role.RoleName == "SuperAdmin")
                                {
                                    _authentication.SignIn(user, false);
                                    var profile = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                    if (profile == null)
                                    {
                                        return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
                                    }
                                    else
                                    {
                                        var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                        if (contact == null)
                                        {
                                            return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
                                        }
                                        var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                        if (address == null)
                                        {
                                            return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
                                        }
                                    }
                                    return RedirectToAction("MyDashboard", "Account");
                                }
                                if (user.role.RoleName == "Admin")
                                {
                                    _authentication.SignIn(user, false);
                                    var licnese = db.LicenseProduct.Where(x => x.Status == true).SingleOrDefault();
                                    if ((licnese.ExpireOn - DateTime.Today).Days > 0)
                                    {
                                        if ((licnese.ExpireOn - DateTime.Today).Days == 30)
                                        {
                                            MailMessage msg = new MailMessage(
                                           new MailAddress("onyxtech786@gmail.com", "Online Real Estate System Notice"), new MailAddress(licnese.account.AccountEmailId));
                                            msg.Subject = "Online Real Estate System Notice";
                                            StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/NoticeLicense.html"));
                                            string readfile = reader.ReadToEnd();
                                            string StrContent = "";
                                            StrContent = readfile;
                                            StrContent = StrContent.Replace("[EmailId]", licnese.CompanyName);
                                            msg.Body = StrContent.ToString();
                                            msg.IsBodyHtml = true;
                                            SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                                            sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                                            sendvia.Send(msg);
                                            _authentication.SignIn(user, false);
                                            var profile = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                            if (profile == null)
                                            {
                                                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
                                            }
                                            else
                                            {
                                                var contact = db.contacts.Where(x => x.AccountId == profile.AccountId).SingleOrDefault();
                                                if (contact == null)
                                                {
                                                    return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
                                                }
                                                var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                                if (address == null)
                                                {
                                                    return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
                                                }
                                            }
                                            return RedirectToAction("MyDashboard", "Account");
                                        }
                                        else
                                        {
                                            _authentication.SignIn(user, false);
                                            var profile = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                            if (profile == null)
                                            {
                                                return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
                                            }
                                            else
                                            {
                                                var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                                if (contact == null)
                                                {
                                                    return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
                                                }
                                                var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                                if (address == null)
                                                {
                                                    return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
                                                }
                                            }
                                            return RedirectToAction("MyDashboard", "Account");
                                        }
                                    }
                                }
                                if (user.role.RoleName == "User"||user.role.RoleName=="SuperUser")
                                {
                                    _authentication.SignIn(user, false);
                                    var profile = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                    if (profile == null)
                                    {
                                        return RedirectToAction("ProfilePersonDetails", "CustomProfileHandler");
                                    }
                                    else
                                    {
                                        var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                        if(contact==null)
                                        {
                                            return RedirectToAction("ProfileContactDetails", "CustomProfileHandler");
                                        }
                                        var address = db.addresses.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                        if(address==null)
                                        {
                                            return RedirectToAction("ProfileAddressDetails", "CustomProfileHandler");
                                        }
                                    }
                                    return RedirectToAction("MyDashboard", "Account");
                                }
                                if (user.role.RoleName == "Agent")
                                {
                                    _authentication.SignIn(user, false);
                                    return RedirectToAction("MyDashboard", "Account");
                                }
                            }
                            
                        }
                        else
                        {

                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "Your Account Has Been Blacklisted");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "The login details not valid please check!!");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please Check..");
            }
                
            return View(viewmodel);
        }

        public ActionResult MyDashboard()
        {
            if(_authentication.IsUserSuperAdmin())
            {
                return RedirectToAction("Index", "SuperAdminLicense");
            }
            else if(_authentication.IsUserAdmin())
            {
                return RedirectToAction("Index", "Person");
            }
            else if (_authentication.IsUserNonAdmin()||_authentication.IsSuperUser()) {
                return RedirectToAction("Index", "CustomProfileHandler");
            }
            else if(_authentication.IsAgent())
            {
                return RedirectToAction("Index", "CustomProfileHandler");
            }
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Logout()
        {
            _authentication.SignOut();
            Session.Abandon();
            Session.Clear();
            Session["Authenicated"] = null;
            return RedirectToAction("Login", "Account");
        }


        public bool IsUserValid(string email,string password)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email && x.AccountPassword == password&&x.IsVarified==true).Count() > 0)
            {
                return true;
            }
            else
            { return false; }
        }

        public bool IsUserBlackList(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email && x.role.RoleName != "Blacklist").Count() > 0)
            {
                return true;
            }
            else
            { return false; }
        }


        [AllowAnonymous]
        public ActionResult ConfirmEmail(string Token, string Email)
        {
            var user = db.accounts.Where(x => x.AccountEmailId == Email).FirstOrDefault();
            if (user != null)
            {
                if (user.AccountEmailId == Email)
                {
                    user.IsVarified = true;
                    db.SaveChanges();
                    return RedirectToAction("EmailConfirmed", "Account", new { ConfirmedEmail = user.AccountEmailId });
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { Email = user.AccountEmailId });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { Email = "" });
            }
        }

        [AllowAnonymous]
        public ActionResult EmailConfirmed(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [AllowAnonymous]
        public ActionResult Confirm(string Email)
        {
            ViewBag.Email = Email; 
            return View();
        } 

        public bool emailcheckfine(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email && x.IsVarified == true && x.IsDeleted==false).Count() > 0)
            {
                return false;
            }
            else { return true; }
        }

        [AllowAnonymous]
        public bool CheckMailExists(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email && x.IsVarified == false).Count() > 0)
            {
                return false;
            }
            else { return true; }
        }

    
        [AllowAnonymous]
        public ActionResult EmailNotConfirmed()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmationLink()
        {
            var resend = new ResendConfirmationViewModel();
            return View(resend);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ConfirmationLink(ResendConfirmationViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                if(IsemailExists(viewmodel.emailId))
                {
                    var user=db.accounts.Where(x=>x.AccountEmailId==viewmodel.emailId).SingleOrDefault();
                    if(user.IsVarified==true)
                    {
                        ModelState.AddModelError("","Your email id already confirmed!!");
                    }
                    else
                    {
                         MailMessage msg = new MailMessage(
                         new MailAddress("onyxtech786@gmail.com", "360 Dynamics Web Registration"), new MailAddress(user.AccountEmailId));
                         msg.Subject = "Emil Confirmation";
                         StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/EmailTemplate.html"));
                         string readfile = reader.ReadToEnd();
                         string StrContent = "";
                         string url = (Url.Action("ConfirmEmail", "Account",
                                    new
                                    {
                                        Token = Extensions.Encrypt(user.AccountId.ToString()),
                                        Email = user.AccountEmailId
                                    }, Request.Url.Scheme));

                         StrContent = readfile;
                         StrContent = StrContent.Replace("[EmailId]", viewmodel.emailId);
                         StrContent = StrContent.Replace("[Link]", url);
                         msg.Body = StrContent.ToString();
                         msg.IsBodyHtml = true;
                         SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                         sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                         sendvia.Send(msg);
                         return RedirectToAction("Confirm", "Account", new { Email = user.AccountEmailId });
                    }
                }
                else
                {
                    return RedirectToAction("AccountNotExists", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("", "There is somithing wromg please check details!!");
            }
            return View(viewmodel);
        }

        public bool Isemailnotconfirmed(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email && x.IsVarified==false).Count() > 0)
            {
                return true;
            }
            else
            { return false; }
        }

        public ActionResult AccountNotExists()
        {
            return View();
        }

        public bool IsemailExists(string email)
        {
            if (db.accounts.Where(x => x.AccountEmailId == email).Count() > 0)
            {
                return true;
            }
            else
            { return false; }
        }


        [AllowAnonymous]
        public ActionResult ForgetPassword()
        {
            var viewmodel = new ForGetViewModelPassword();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgetPassword(ForGetViewModelPassword viewmodel)
        {
            if (ModelState.IsValid)
            {
                if (IsemailExists(viewmodel.emailId))
                {
                    var user = db.accounts.Where(x => x.AccountEmailId == viewmodel.emailId).SingleOrDefault();
                        MailMessage msg = new MailMessage(
                        new MailAddress("onyxtech786@gmail.com", "360Dynamics.net Property Management System"), new MailAddress(user.AccountEmailId));
                        msg.Subject = "Reset Password Link";
                        StreamReader reader = new StreamReader(Server.MapPath("~/App_Data/Template/ResetPasswordEmail.html"));
                        string readfile = reader.ReadToEnd();
                        string StrContent = "";
                        string url = (Url.Action("ResetPassword", "Account",
                                   new
                                   {
                                       Token = Extensions.Encrypt(user.AccountId.ToString()),
                                       Email = user.AccountEmailId
                                   }, Request.Url.Scheme));

                        StrContent = readfile;
                        StrContent = StrContent.Replace("[EmailId]", viewmodel.emailId);
                        StrContent = StrContent.Replace("[Link]", url);
                        msg.Body = StrContent.ToString();
                        msg.IsBodyHtml = true;
                        SmtpClient sendvia = new SmtpClient("smtp.gmail.com", 587);
                        sendvia.Credentials = new NetworkCredential("onyxtech786@gmail.com", "cyberghost123");
                        sendvia.EnableSsl = true;
                        sendvia.Send(msg);
                        return RedirectToAction("LinkSent", "Account", new { Email = user.AccountEmailId });
                    
                }
                else
                {
                    return PartialView("AccountNotExists");
                }
            }
            else
            {
                ModelState.AddModelError("", "Please check model state!!");
            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string email)
        {
            var resetviewmodel = new PasswordResetViewModel();
            resetviewmodel.AccountEmailId = email;
            return View(resetviewmodel);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(PasswordResetViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                var user = db.accounts.Where(s => s.AccountEmailId == viewmodel.AccountEmailId).SingleOrDefault();
                var pwd = Extensions.Encrypt(viewmodel.AccountPassword);
                user.AccountPassword = pwd;
                db.SaveChanges();
                return RedirectToAction("PasswordChanged", "Account");
            }
            else
            {
                ModelState.AddModelError("", "There is something wrong!!");
            }
            return View(viewmodel);
        }

        public ActionResult PasswordChanged()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LinkSent(string email)
        {
            ViewBag.Email = email;
            return View();
        }
	}
}