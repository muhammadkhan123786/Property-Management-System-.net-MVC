using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace _360PropertyManagement.Controllers
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        private Context db = new Context();
        private HttpContextBase _contexthttpContext;
        private Accounts _contextcachedUser;
        private readonly TimeSpan _contextexpirationTimeSpan;
        
        public FormsAuthenticationService(HttpContextBase httpContext)
        {

            _contexthttpContext = httpContext;
            _contextexpirationTimeSpan = TimeSpan.FromHours(6);
        }

        public virtual void SignIn(Accounts user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            
            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.AccountEmailId,
                now,
                now.Add(_contextexpirationTimeSpan),
                createPersistentCookie,
                user.AccountEmailId,

                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            _contexthttpContext.Response.Cookies.Add(cookie);
            _contextcachedUser = user;
            
        }

        public virtual Accounts GetAuthenticatedUserFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");

            var username = ticket.UserData;

            if (String.IsNullOrWhiteSpace(username))
                return null;
            var visitor = db.accounts.Where(x => x.AccountEmailId == username).SingleOrDefault();
            // var customer = _userRepository.GetById(x => x.Username == username);
            return visitor;
        }

        public virtual Accounts GetAuthenticatedUser()
        {
            if (_contextcachedUser != null)
                return _contextcachedUser;

            if (_contexthttpContext == null ||
                _contexthttpContext.Request == null ||
                !_contexthttpContext.Request.IsAuthenticated ||
                !(_contexthttpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)_contexthttpContext.User.Identity;
            var user = GetAuthenticatedUserFromTicket(formsIdentity.Ticket);
            if (user != null)
                _contextcachedUser = user;
            return _contextcachedUser;
        }

        public virtual void SignOut()
        {
            _contextcachedUser = null;
            FormsAuthentication.SignOut();
        }


        public virtual Accounts GetUser()
        {

            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    if (_contextcachedUser != null)
                    {
                        return _contextcachedUser;
                    }
                    else
                    {
                        HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                        FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                        string ticketdata = ticket.UserData;
                        _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                        return _contextcachedUser;
                    }
                }
                catch (Exception)
                {

                }
            }
            return _contextcachedUser;
        }

        public virtual bool IsUserAuthenicated()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                return true;
            }
            return false;
        }


        public virtual bool IsUserAdmin()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string ticketdata = ticket.UserData;
                    _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                    if (_contextcachedUser.role.RoleName == "Admin")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                }
            }
            return false;
        }


        public virtual bool IsSuperUser()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string ticketdata = ticket.UserData;
                    _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                    if (_contextcachedUser.role.RoleName == "SuperUser")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        public virtual bool IsAgent()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string ticketdata = ticket.UserData;
                    _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                    if (_contextcachedUser.role.RoleName == "Agent")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                }
            }
            return false;
        }

       public virtual bool IsUserSuperAdmin()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string ticketdata = ticket.UserData;
                    _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                    if (_contextcachedUser.role.RoleName == "SuperAdmin")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                }
            }
            return false;
        }

        public virtual bool IsUserNonAdmin()
        {
            if (System.Web.HttpContext.Current.Request.Cookies != null && System.Web.HttpContext.Current.Request.Cookies.AllKeys.Contains(FormsAuthentication.FormsCookieName))
            {
                try
                {

                    HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string ticketdata = ticket.UserData;
                    _contextcachedUser = db.accounts.Where(x => x.AccountEmailId == ticketdata).SingleOrDefault();
                    if (_contextcachedUser.role.RoleName == "User")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch (Exception)
                {

                }
            }
            return false;
        }


  }

    public interface IFormsAuthenticationService
    {

        void SignIn(Accounts user, bool createPersistentCookie);
        void SignOut();
        Accounts GetAuthenticatedUser();

    }
}