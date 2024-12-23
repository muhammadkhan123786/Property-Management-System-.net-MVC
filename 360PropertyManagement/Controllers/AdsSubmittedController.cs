using _360PropertyManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _360PropertyManagement.ViewModels;
using System.Web.Routing;
using System.Drawing;
using System.Net;

namespace _360PropertyManagement.Controllers
{
    public class AdsSubmittedController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
        //
        // GET: /AdsSubmitted/
        public ActionResult Index()
        {
            return RedirectToAction("AdSubmision");
        }


        [HttpGet]
        public ActionResult AdSubmision()
        {
            var viewmodel = new AdsViewModel();
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
            ViewBag.TopCategoryId = new SelectList(db.toppropertycategory.Where(x => x.IsActive == true).ToList(), "TopCategoryId", "TopCategoryName", viewmodel.TopCategoryId);
            ViewBag.SubCategoryId = new SelectList(db.propertysubcategory.Where(x => x.IsActive == true).ToList(), "SubCategoryId", "SubCategoryName", viewmodel.SubCategoryId);
            ViewBag.RoomsId = new SelectList(db.propertyrooms.Where(x => x.IsActive == true).ToList(), "RoomsId", "NumberOfRooms", viewmodel.RoomsId);
            ViewBag.TagAccounts = new MultiSelectList(db.accounts.Where(x => x.IsActive == true).ToList(), "AccountId", "AccountEmailId");
            return View(viewmodel);
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
        public ActionResult AdSubmision(AdsViewModel viewmodel, int[] Accounts)
        {
            if (ModelState.IsValid)
            {
                if(viewmodel.ContactInfo==true)
                {
                    var user = _authentication.GetUser();
                    var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                    if(contact==null)
                    {
                        ModelState.AddModelError("", "You Didn't Added Your Contact Info Yet.. Please Check..");
                    }
                    else
                    {
                        if(viewmodel.PersonalInfo==true)
                        {
                            var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                            if(person==null)
                            {
                                ModelState.AddModelError("", "You didn't add your personal info in your profile yet...");
                            }
                            else
                            {
                                if (viewmodel.PropertyPhotoOne == null)
                                {
                                    var image = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                                    var newimage = new Images()
                                    {
                                        Image = image.ImageNotAvailable
                                    };
                                    db.images.Add(newimage);                                    
                                    var newproperty = new PropertyAds()
                                    {
                                        PropertyTitle = viewmodel.PropertyTitle,
                                        MaximumPrice = viewmodel.MaximumPrice,
                                        MinimumPrice = viewmodel.MinimumPrice,
                                        Address =Extensions.RemoveSpecialCharacters(viewmodel.Address),
                                        ContactNumber = contact.MobileOne,
                                        PersonName = person.PersonFullName,
                                        IsFurnished = viewmodel.IsFurnished,
                                        PropertyDescription = viewmodel.PropertyDescription,
                                        ImageId = newimage.ImageId,
                                        RoomsId = viewmodel.RoomsId,
                                        SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                        TopCategoryId = viewmodel.TopCategoryId,
                                        SubCategoryId = viewmodel.SubCategoryId,

                                    };
                                    db.propertyads.Add(newproperty);
                                    var areaad = new MyAreasAdss()
                                    {
                                        CountryId = viewmodel.CountryId,
                                        StateId = viewmodel.StateId,
                                        CityId = viewmodel.CityId,
                                        IsDeleted = false,
                                        Location = viewmodel.Location,
                                        SeoKeyWords = viewmodel.SeoKeyWords,
                                        ZipCode = viewmodel.ZipCode

                                    };
                                    db.myareas.Add(areaad);
                                    var newads = new SubmitedAds()
                                    {
                                        PropertyId = newproperty.PropertyId,
                                        AccountId = user.AccountId,
                                        NumberOfViews=0,
                                        IsActive = true,
                                        IsDeleted = false,
                                        DateNTime = DateTime.Now,
                                        MyAreaId = areaad.MyAreaId,

                                    };
                                    db.submitedads.Add(newads);
                                    //agents Assign 
                                    if(user.role.RoleName=="Admin"||user.role.RoleName=="SuperUser"||user.role.RoleName=="Agent")
                                    {
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = user.AccountId,
                                            ContactId = contact.ContactId,
                                            PersonId = contact.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);
                                    }
                                    else
                                    {
                                        var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                        var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                        var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId&&x.account.role.RoleName=="Agent").ToList();
                                        var accs = new List<Accounts>();
                                        if(CityLocations==null)
                                        {
                                            var random = new Random();
                                            var r = random.Next(agents.Count());
                                            var randomStringFromList = agents[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);
                                    
                                        }
                                        else
                                        {
                                            foreach(var items in CityLocations)
                                            {
                                                var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                                if(location!=null&&location.account.role.RoleName=="Agent")
                                                {

                                                    accs.Add(location.account);
                                                }
                                            }
                                            if(accs.Count==0)
                                            {
                                                var random = new Random();
                                                var r = random.Next(CityLocations.Count());
                                                var randomStringFromList = CityLocations[r];
                                                var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var assignagent = new AdAgents()
                                                {
                                                    AdId = newads.AdId,
                                                    AccountId = randomStringFromList.AccountId,
                                                    ContactId = con.ContactId,
                                                    PersonId = personagent.PersonId,
                                                    IsDeleted = false,
                                                    Remarks = newads.AdId.ToString(),

                                                };
                                                db.agentsads.Add(assignagent);

                                            }
                                            else
                                            {
                                                var random = new Random();
                                                var r = random.Next(accs.Count());
                                                var randomStringFromList = accs[r];
                                                var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var assignagent = new AdAgents()
                                                {
                                                    AdId = newads.AdId,
                                                    AccountId = randomStringFromList.AccountId,
                                                    ContactId = con.ContactId,
                                                    PersonId = personagent.PersonId,
                                                    IsDeleted = false,
                                                    Remarks = newads.AdId.ToString(),

                                                };
                                                db.agentsads.Add(assignagent);

                                            }

                                        }
                                    }
                                    

                                    db.SaveChanges();
                                    return RedirectToAction("AddImages", "AdsSubmitted", new {id=newads.AdId });
                                }
                                else
                                {
                                    var uploadImage = new Bitmap(viewmodel.PropertyPhotoOne.InputStream);
                                    var newimage = Extensions.ResizeImage(uploadImage);
                                    var imagebytes = Extensions.ImageToByte(newimage);
                                   
                                    var TosaveImage = new Images()
                                    {
                                        Image = imagebytes,
                                        IsDeleted = false
                                    };
                                    db.images.Add(TosaveImage);
                                    var newproperty = new PropertyAds()
                                    {
                                        PropertyTitle = viewmodel.PropertyTitle,
                                        MaximumPrice = viewmodel.MaximumPrice,
                                        MinimumPrice = viewmodel.MinimumPrice,
                                        ContactNumber = contact.MobileOne,
                                        Address=viewmodel.Address,
                                        PersonName = person.PersonFullName,
                                        IsFurnished = viewmodel.IsFurnished,
                                        PropertyDescription = viewmodel.PropertyDescription,
                                        ImageId = TosaveImage.ImageId,
                                        RoomsId = viewmodel.RoomsId,
                                        SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                        TopCategoryId = viewmodel.TopCategoryId,
                                        SubCategoryId = viewmodel.SubCategoryId,

                                    };
                                    db.propertyads.Add(newproperty);
                                    var areaad = new MyAreasAdss()
                                    {
                                        CountryId = viewmodel.CountryId,
                                        StateId = viewmodel.StateId,
                                        CityId = viewmodel.CityId,
                                        IsDeleted = false,
                                        Location = viewmodel.Location,
                                        SeoKeyWords = viewmodel.SeoKeyWords,
                                        ZipCode = viewmodel.ZipCode

                                    };
                                    db.myareas.Add(areaad);
                                    var newads = new SubmitedAds()
                                    {
                                        PropertyId = newproperty.PropertyId,
                                        AccountId = user.AccountId,
                                        IsActive = true,
                                        NumberOfViews = 0,
                                        IsDeleted = false,
                                        DateNTime = DateTime.Now,
                                        MyAreaId = areaad.MyAreaId,

                                    };
                                    db.submitedads.Add(newads);
                                    //agents Assign 
                                    if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                                    {
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = user.AccountId,
                                            ContactId = contact.ContactId,
                                            PersonId = contact.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);
                                    }
                                    else
                                    {
                                        var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                        var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                        var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                        var accs = new List<Accounts>();
                                        if (CityLocations == null)
                                        {
                                            var random = new Random();
                                            var r = random.Next(agents.Count());
                                            var randomStringFromList = agents[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }
                                        else
                                        {
                                            foreach (var items in CityLocations)
                                            {
                                                var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                                if (location != null && location.account.role.RoleName == "Agent")
                                                {

                                                    accs.Add(location.account);
                                                }
                                            }
                                            if (accs.Count == 0)
                                            {
                                                var random = new Random();
                                                var r = random.Next(CityLocations.Count());
                                                var randomStringFromList = CityLocations[r];
                                                var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var assignagent = new AdAgents()
                                                {
                                                    AdId = newads.AdId,
                                                    AccountId = randomStringFromList.AccountId,
                                                    ContactId = con.ContactId,
                                                    PersonId = personagent.PersonId,
                                                    IsDeleted = false,
                                                    Remarks = newads.AdId.ToString(),

                                                };
                                                db.agentsads.Add(assignagent);

                                            }
                                            else
                                            {
                                                var random = new Random();
                                                var r = random.Next(accs.Count());
                                                var randomStringFromList = accs[r];
                                                var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                                var assignagent = new AdAgents()
                                                {
                                                    AdId = newads.AdId,
                                                    AccountId = randomStringFromList.AccountId,
                                                    ContactId = con.ContactId,
                                                    PersonId = personagent.PersonId,
                                                    IsDeleted = false,
                                                    Remarks = newads.AdId.ToString(),

                                                };
                                                db.agentsads.Add(assignagent);

                                            }

                                        }
                                    }
                                    db.SaveChanges();
                                    return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                                }
                            }
                            
                        }
                        else if(!String.IsNullOrEmpty(viewmodel.PersonName))
                        {
                            
                            if (viewmodel.PropertyPhotoOne == null)
                            {
                                
                                var image = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                                var newimage = new Images()
                                {
                                    Image = image.ImageNotAvailable
                                };
                                db.images.Add(newimage);
                                var newproperty = new PropertyAds()
                                {
                                    PropertyTitle = viewmodel.PropertyTitle,
                                    MaximumPrice = viewmodel.MaximumPrice,
                                    MinimumPrice = viewmodel.MinimumPrice,
                                    Address = viewmodel.Address,
                                    ContactNumber = viewmodel.MobileNumber,
                                    PersonName = viewmodel.PersonName,
                                    IsFurnished = viewmodel.IsFurnished,
                                    PropertyDescription = viewmodel.PropertyDescription,
                                    ImageId = newimage.ImageId,
                                    RoomsId = viewmodel.RoomsId,
                                    SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                    TopCategoryId = viewmodel.TopCategoryId,
                                    SubCategoryId = viewmodel.SubCategoryId,

                                };
                                db.propertyads.Add(newproperty);
                                var areaad = new MyAreasAdss()
                                {
                                    CountryId = viewmodel.CountryId,
                                    StateId = viewmodel.StateId,
                                    CityId = viewmodel.CityId,
                                    IsDeleted = false,
                                    Location = viewmodel.Location,
                                    SeoKeyWords = viewmodel.SeoKeyWords,
                                    ZipCode = viewmodel.ZipCode

                                };
                                db.myareas.Add(areaad);
                                var newads = new SubmitedAds()
                                {
                                    PropertyId = newproperty.PropertyId,
                                    AccountId = user.AccountId,
                                    IsActive = true,
                                    IsDeleted = false,
                                    NumberOfViews = 0,
                                    DateNTime = DateTime.Now,
                                    MyAreaId = areaad.MyAreaId,

                                };
                                db.submitedads.Add(newads);
                                //agents Assign 
                                if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                                {
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = user.AccountId,
                                        ContactId = contact.ContactId,
                                        PersonId = contact.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);
                                }
                                else
                                {
                                    var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                    var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                    var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                    var accs = new List<Accounts>();
                                    if (CityLocations == null)
                                    {
                                        var random = new Random();
                                        var r = random.Next(agents.Count());
                                        var randomStringFromList = agents[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        foreach (var items in CityLocations)
                                        {
                                            var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                            if (location != null && location.account.role.RoleName == "Agent")
                                            {

                                                accs.Add(location.account);
                                            }
                                        }
                                        if (accs.Count == 0)
                                        {
                                            var random = new Random();
                                            var r = random.Next(CityLocations.Count());
                                            var randomStringFromList = CityLocations[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }
                                        else
                                        {
                                            var random = new Random();
                                            var r = random.Next(accs.Count());
                                            var randomStringFromList = accs[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }

                                    }
                                }
                                db.SaveChanges();
                                return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                            }
                            else
                            {
                                var uploadImage = new Bitmap(viewmodel.PropertyPhotoOne.InputStream);
                                var newimage = Extensions.ResizeImage(uploadImage);
                                var imagebytes = Extensions.ImageToByte(newimage);
                                var TosaveImage = new Images()
                                {
                                    Image = imagebytes,
                                    IsDeleted = false
                                };
                                db.images.Add(TosaveImage);
                                var newproperty = new PropertyAds()
                                {
                                    PropertyTitle = viewmodel.PropertyTitle,
                                    MaximumPrice = viewmodel.MaximumPrice,
                                    MinimumPrice = viewmodel.MinimumPrice,
                                    Address = viewmodel.Address,
                                    ContactNumber = viewmodel.MobileNumber,
                                    PersonName = viewmodel.PersonName,
                                    IsFurnished = viewmodel.IsFurnished,
                                    PropertyDescription = viewmodel.PropertyDescription,
                                    ImageId = TosaveImage.ImageId,
                                    RoomsId = viewmodel.RoomsId,
                                    SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                    TopCategoryId = viewmodel.TopCategoryId,
                                    SubCategoryId = viewmodel.SubCategoryId,

                                };
                                db.propertyads.Add(newproperty);
                                var areaad = new MyAreasAdss()
                                {
                                    CountryId = viewmodel.CountryId,
                                    StateId = viewmodel.StateId,
                                    CityId = viewmodel.CityId,
                                    IsDeleted = false,
                                    Location = viewmodel.Location,
                                    SeoKeyWords = viewmodel.SeoKeyWords,
                                    ZipCode = viewmodel.ZipCode

                                };
                                db.myareas.Add(areaad);
                                var newads = new SubmitedAds()
                                {
                                    PropertyId = newproperty.PropertyId,
                                    AccountId = user.AccountId,
                                    IsActive = true,
                                    NumberOfViews = 0,
                                    IsDeleted = false,
                                    DateNTime = DateTime.Now,
                                    MyAreaId = areaad.MyAreaId,

                                };
                                db.submitedads.Add(newads);
                                //agents Assign 
                                if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                                {
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = user.AccountId,
                                        ContactId = contact.ContactId,
                                        PersonId = contact.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);
                                }
                                else
                                {
                                    var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                    var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                    var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                    var accs = new List<Accounts>();
                                    if (CityLocations == null)
                                    {
                                        var random = new Random();
                                        var r = random.Next(agents.Count());
                                        var randomStringFromList = agents[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        foreach (var items in CityLocations)
                                        {
                                            var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                            if (location != null && location.account.role.RoleName == "Agent")
                                            {

                                                accs.Add(location.account);
                                            }
                                        }
                                        if (accs.Count == 0)
                                        {
                                            var random = new Random();
                                            var r = random.Next(CityLocations.Count());
                                            var randomStringFromList = CityLocations[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }
                                        else
                                        {
                                            var random = new Random();
                                            var r = random.Next(accs.Count());
                                            var randomStringFromList = accs[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }

                                    }
                                }
                                db.SaveChanges();
                                return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Person Name IS Required Please Check...");
                        }
                    }
                }
                else if(!String.IsNullOrEmpty(viewmodel.MobileNumber))
                {
                    if (viewmodel.PersonalInfo == true)
                    {
                        var user = _authentication.GetUser();
                        var person = db.persons.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                        if (viewmodel.PropertyPhotoOne == null)
                        {
                            
                            if(person==null)
                            {
                                ModelState.AddModelError("", "Person Didn't added yet in your profile...");

                            }
                            else
                            {
                                var image = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                                var newimage = new Images()
                                {
                                    Image = image.ImageNotAvailable
                                };
                                db.images.Add(newimage);
                                var newproperty = new PropertyAds()
                                {
                                    PropertyTitle = viewmodel.PropertyTitle,
                                    MaximumPrice = viewmodel.MaximumPrice,
                                    MinimumPrice = viewmodel.MinimumPrice,
                                    Address = viewmodel.Address,
                                    ContactNumber = viewmodel.MobileNumber,
                                    PersonName =person.PersonFullName,
                                    IsFurnished = viewmodel.IsFurnished,
                                    PropertyDescription = viewmodel.PropertyDescription,
                                    ImageId = newimage.ImageId,
                                    RoomsId = viewmodel.RoomsId,
                                    SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                    TopCategoryId = viewmodel.TopCategoryId,
                                    SubCategoryId = viewmodel.SubCategoryId,

                                };
                                db.propertyads.Add(newproperty);
                                var areaad = new MyAreasAdss()
                                {
                                    CountryId = viewmodel.CountryId,
                                    StateId = viewmodel.StateId,
                                    CityId = viewmodel.CityId,
                                    IsDeleted = false,
                                    Location = viewmodel.Location,
                                    SeoKeyWords = viewmodel.SeoKeyWords,
                                    ZipCode = viewmodel.ZipCode

                                };
                                db.myareas.Add(areaad);
                                var newads = new SubmitedAds()
                                {
                                    PropertyId = newproperty.PropertyId,
                                    AccountId = user.AccountId,
                                    IsActive = true,
                                    NumberOfViews = 0,
                                    IsDeleted = false,
                                    DateNTime = DateTime.Now,
                                    MyAreaId = areaad.MyAreaId,

                                };
                                db.submitedads.Add(newads);
                                //agents Assign 
                                if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                                {
                                    var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = user.AccountId,
                                        ContactId = contact.ContactId,
                                        PersonId = contact.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);
                                }
                                else
                                {
                                    var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                    var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                    var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                    var accs = new List<Accounts>();
                                    if (CityLocations == null)
                                    {
                                        var random = new Random();
                                        var r = random.Next(agents.Count());
                                        var randomStringFromList = agents[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        foreach (var items in CityLocations)
                                        {
                                            var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                            if (location != null && location.account.role.RoleName == "Agent")
                                            {

                                                accs.Add(location.account);
                                            }
                                        }
                                        if (accs.Count == 0)
                                        {
                                            var random = new Random();
                                            var r = random.Next(CityLocations.Count());
                                            var randomStringFromList = CityLocations[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }
                                        else
                                        {
                                            var random = new Random();
                                            var r = random.Next(accs.Count());
                                            var randomStringFromList = accs[r];
                                            var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                            var assignagent = new AdAgents()
                                            {
                                                AdId = newads.AdId,
                                                AccountId = randomStringFromList.AccountId,
                                                ContactId = con.ContactId,
                                                PersonId = personagent.PersonId,
                                                IsDeleted = false,
                                                Remarks = newads.AdId.ToString(),

                                            };
                                            db.agentsads.Add(assignagent);

                                        }

                                    }
                                }
                                db.SaveChanges();
                                return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                            }
                        }
                        else
                        {
                            var uploadImage = new Bitmap(viewmodel.PropertyPhotoOne.InputStream);
                            var newimage = Extensions.ResizeImage(uploadImage);
                            var imagebytes = Extensions.ImageToByte(newimage);
                            var TosaveImage = new Images()
                            {
                                Image = imagebytes,
                                IsDeleted = false
                            };
                            db.images.Add(TosaveImage);
                            var newproperty = new PropertyAds()
                            {
                                PropertyTitle = viewmodel.PropertyTitle,
                                MaximumPrice = viewmodel.MaximumPrice,
                                MinimumPrice = viewmodel.MinimumPrice,
                                Address = viewmodel.Address,
                                ContactNumber = viewmodel.MobileNumber,
                                PersonName = person.PersonFullName,
                                IsFurnished = viewmodel.IsFurnished,
                                PropertyDescription = viewmodel.PropertyDescription,
                                ImageId = TosaveImage.ImageId,
                                RoomsId = viewmodel.RoomsId,
                                SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                TopCategoryId = viewmodel.TopCategoryId,
                                SubCategoryId = viewmodel.SubCategoryId,

                            };
                            db.propertyads.Add(newproperty);
                            var areaad = new MyAreasAdss()
                            {
                                CountryId = viewmodel.CountryId,
                                StateId = viewmodel.StateId,
                                CityId = viewmodel.CityId,
                                IsDeleted = false,
                                Location = viewmodel.Location,
                                SeoKeyWords = viewmodel.SeoKeyWords,
                                ZipCode = viewmodel.ZipCode

                            };
                            db.myareas.Add(areaad);
                            var newads = new SubmitedAds()
                            {
                                PropertyId = newproperty.PropertyId,
                                AccountId = user.AccountId,
                                IsActive = true,
                                NumberOfViews = 0,
                                IsDeleted = false,
                                DateNTime = DateTime.Now,
                                MyAreaId = areaad.MyAreaId,

                            };
                            db.submitedads.Add(newads);
                            //agents Assign 
                            if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                            {
                                var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                var assignagent = new AdAgents()
                                {

                                    AdId = newads.AdId,
                                    AccountId = user.AccountId,
                                    ContactId = contact.ContactId,
                                    PersonId = contact.PersonId,
                                    IsDeleted = false,
                                    Remarks = newads.AdId.ToString(),

                                };
                                db.agentsads.Add(assignagent);
                            }
                            else
                            {
                                var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                var accs = new List<Accounts>();
                                if (CityLocations == null)
                                {
                                    var random = new Random();
                                    var r = random.Next(agents.Count());
                                    var randomStringFromList = agents[r];
                                    var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = randomStringFromList.AccountId,
                                        ContactId = con.ContactId,
                                        PersonId = personagent.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);

                                }
                                else
                                {
                                    foreach (var items in CityLocations)
                                    {
                                        var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                        if (location != null && location.account.role.RoleName == "Agent")
                                        {

                                            accs.Add(location.account);
                                        }
                                    }
                                    if (accs.Count == 0)
                                    {
                                        var random = new Random();
                                        var r = random.Next(CityLocations.Count());
                                        var randomStringFromList = CityLocations[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var r = random.Next(accs.Count());
                                        var randomStringFromList = accs[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }

                                }
                            }
                            db.SaveChanges();
                            return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                        }
                    }
                    else if (!String.IsNullOrEmpty(viewmodel.PersonName))
                    {
                        var user = _authentication.GetUser();
                        if (viewmodel.PropertyPhotoOne == null)
                        {
                            var image = db.imagenot.Where(x => x.Status == true).SingleOrDefault();
                            var newimage = new Images()
                            {
                                Image = image.ImageNotAvailable
                            };
                            db.images.Add(newimage);
                            var newproperty = new PropertyAds()
                            {
                                PropertyTitle = viewmodel.PropertyTitle,
                                MaximumPrice = viewmodel.MaximumPrice,
                                MinimumPrice = viewmodel.MinimumPrice,
                                Address = viewmodel.Address,
                                ContactNumber = viewmodel.MobileNumber,
                                PersonName = viewmodel.PersonName,
                                IsFurnished = viewmodel.IsFurnished,
                                PropertyDescription = viewmodel.PropertyDescription,
                                ImageId = newimage.ImageId,
                                RoomsId = viewmodel.RoomsId,
                                SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                TopCategoryId = viewmodel.TopCategoryId,
                                SubCategoryId = viewmodel.SubCategoryId,

                            };
                            db.propertyads.Add(newproperty);
                            var areaad = new MyAreasAdss()
                            {
                                CountryId = viewmodel.CountryId,
                                StateId = viewmodel.StateId,
                                CityId = viewmodel.CityId,
                                IsDeleted = false,
                                Location = viewmodel.Location,
                                SeoKeyWords = viewmodel.SeoKeyWords,
                                ZipCode = viewmodel.ZipCode

                            };
                            db.myareas.Add(areaad);
                            var newads = new SubmitedAds()
                            {
                                PropertyId = newproperty.PropertyId,
                                AccountId = user.AccountId,
                                IsActive = true,
                                NumberOfViews = 0,
                                IsDeleted = false,
                                DateNTime = DateTime.Now,
                                MyAreaId = areaad.MyAreaId,

                            };
                            db.submitedads.Add(newads);
                            //agents Assign 
                            if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                            {
                                var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                var assignagent = new AdAgents()
                                {
                                    AdId = newads.AdId,
                                    AccountId = user.AccountId,
                                    ContactId = contact.ContactId,
                                    PersonId = contact.PersonId,
                                    IsDeleted = false,
                                    Remarks = newads.AdId.ToString(),

                                };
                                db.agentsads.Add(assignagent);
                            }
                            else
                            {
                                var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                var accs = new List<Accounts>();
                                if (CityLocations == null)
                                {
                                    var random = new Random();
                                    var r = random.Next(agents.Count());
                                    var randomStringFromList = agents[r];
                                    var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = randomStringFromList.AccountId,
                                        ContactId = con.ContactId,
                                        PersonId = personagent.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);

                                }
                                else
                                {
                                    foreach (var items in CityLocations)
                                    {
                                        var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();
                                        if (location != null && location.account.role.RoleName == "Agent")
                                        {

                                            accs.Add(location.account);
                                        }
                                    }
                                    if (accs.Count == 0)
                                    {
                                        var random = new Random();
                                        var r = random.Next(CityLocations.Count());
                                        var randomStringFromList = CityLocations[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var r = random.Next(accs.Count());
                                        var randomStringFromList = accs[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }

                                }
                            }
                            db.SaveChanges();
                            return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                        }
                        else
                        {
                            var uploadImage = new Bitmap(viewmodel.PropertyPhotoOne.InputStream);
                            var newimage = Extensions.ResizeImage(uploadImage);
                            var imagebytes = Extensions.ImageToByte(newimage);
                            var TosaveImage = new Images()
                            {
                                Image = imagebytes,
                                IsDeleted = false
                            };
                            db.images.Add(TosaveImage);
                            var newproperty = new PropertyAds()
                            {
                                PropertyTitle = viewmodel.PropertyTitle,
                                MaximumPrice = viewmodel.MaximumPrice,
                                MinimumPrice = viewmodel.MinimumPrice,
                                Address = viewmodel.Address,
                                ContactNumber = viewmodel.MobileNumber,
                                PersonName =viewmodel.PersonName,
                                IsFurnished = viewmodel.IsFurnished,
                                PropertyDescription = viewmodel.PropertyDescription,
                                ImageId = TosaveImage.ImageId,
                                RoomsId = viewmodel.RoomsId,
                                SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot),
                                TopCategoryId = viewmodel.TopCategoryId,
                                SubCategoryId = viewmodel.SubCategoryId,

                            };
                            db.propertyads.Add(newproperty);
                            var areaad = new MyAreasAdss()
                            {
                                CountryId = viewmodel.CountryId,
                                StateId = viewmodel.StateId,
                                CityId = viewmodel.CityId,
                                IsDeleted = false,
                                Location = viewmodel.Location,
                                SeoKeyWords = viewmodel.SeoKeyWords,
                                ZipCode = viewmodel.ZipCode

                            };
                            db.myareas.Add(areaad);
                            var newads = new SubmitedAds()
                            {
                                PropertyId = newproperty.PropertyId,
                                AccountId = user.AccountId,
                                IsActive = true,
                                NumberOfViews = 0,
                                IsDeleted = false,
                                DateNTime = DateTime.Now,
                                MyAreaId = areaad.MyAreaId,

                            };
                            db.submitedads.Add(newads);
                            //agents Assign 
                            if (user.role.RoleName == "Admin" || user.role.RoleName == "SuperUser"||user.role.RoleName=="Agent")
                            {
                                var contact = db.contacts.Where(x => x.AccountId == user.AccountId).SingleOrDefault();
                                var assignagent = new AdAgents()
                                {
                                    AdId = newads.AdId,
                                    AccountId = user.AccountId,
                                    ContactId = contact.ContactId,
                                    PersonId = contact.PersonId,
                                    IsDeleted = false,
                                    Remarks = newads.AdId.ToString(),

                                };
                                db.agentsads.Add(assignagent);
                            }
                            else
                            {
                                var agents = db.accounts.Where(x => x.role.RoleName == "Agent").ToList();
                                var city = db.cities.Where(x => x.CountryId == viewmodel.CountryId && x.StateId == x.StateId && x.CityId == viewmodel.CityId).SingleOrDefault();
                                var CityLocations = db.MyAreasAds.Where(x => x.CityId == city.CityId && x.account.role.RoleName == "Agent").ToList();
                                var accs = new List<Accounts>();
                                if (CityLocations == null)
                                {
                                    var random = new Random();
                                    var r = random.Next(agents.Count());
                                    var randomStringFromList = agents[r];
                                    var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                    var assignagent = new AdAgents()
                                    {
                                        AdId = newads.AdId,
                                        AccountId = randomStringFromList.AccountId,
                                        ContactId = con.ContactId,
                                        PersonId = personagent.PersonId,
                                        IsDeleted = false,
                                        Remarks = newads.AdId.ToString(),

                                    };
                                    db.agentsads.Add(assignagent);

                                }
                                else
                                {
                                    foreach (var items in CityLocations)
                                    {
                                        var location = CityLocations.Where(x => x.ZipCode.Contains(viewmodel.ZipCode) || x.Location.Contains(viewmodel.Location) || viewmodel.Address.Contains(x.Location)).FirstOrDefault();

                                        if (location != null && location.account.role.RoleName == "Agent")
                                        {

                                            accs.Add(location.account);
                                        }
                                    }
                                    if (accs.Count == 0)
                                    {
                                        var random = new Random();
                                        var r = random.Next(CityLocations.Count());
                                        var randomStringFromList = CityLocations[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }
                                    else
                                    {
                                        var random = new Random();
                                        var r = random.Next(accs.Count());
                                        var randomStringFromList = accs[r];
                                        var con = db.contacts.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var personagent = db.persons.Where(x => x.AccountId == randomStringFromList.AccountId).SingleOrDefault();
                                        var assignagent = new AdAgents()
                                        {
                                            AdId = newads.AdId,
                                            AccountId = randomStringFromList.AccountId,
                                            ContactId = con.ContactId,
                                            PersonId = personagent.PersonId,
                                            IsDeleted = false,
                                            Remarks = newads.AdId.ToString(),

                                        };
                                        db.agentsads.Add(assignagent);

                                    }

                                }
                            }
                            db.SaveChanges();
                            return RedirectToAction("AddImages", "AdsSubmitted", new { id = newads.AdId });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Person Name Is Required Please Check...");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Contact Info Is ReQuired Please Check...");
                }
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please Check....");
            }
            ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName");
            ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName");
            ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName");
            ViewBag.TopCategoryId = new SelectList(db.toppropertycategory.Where(x => x.IsActive == true).ToList(), "TopCategoryId", "TopCategoryName");
            ViewBag.SubCategoryId = new SelectList(db.propertysubcategory.Where(x => x.IsActive == true).ToList(), "SubCategoryId", "SubCategoryName");
            ViewBag.RoomsId = new SelectList(db.propertyrooms.Where(x => x.IsActive == true).ToList(), "RoomsId", "NumberOfRooms");
            ViewBag.Accounts = new MultiSelectList(db.accounts.Where(x => x.IsActive == true).ToList(), "AccountId", "AccountEmailId");
            return View(viewmodel);
        }


        [HttpGet]
        public ActionResult AddImages(int Id)
        {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if(ad==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"The Ad is null please check.. ");
            }
            return View(ad);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImages(int Id,IEnumerable<HttpPostedFileBase> files)
        {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if(ad==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var uploadImage = new Bitmap(file.InputStream);
                    var newimage = Extensions.ResizeSliderImage(uploadImage);
                    var imagebytes = Extensions.ImageToByte(newimage);
                    var TosaveImage = new Images()
                    {
                        Image = imagebytes,
                        IsDeleted = false,
                        AdId=ad.AdId
                    };
                    db.images.Add(TosaveImage);
                           
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }


        [HttpGet]
        public ActionResult Details(int Id)
        {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if(ad==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            ad.NumberOfViews = ad.NumberOfViews + 1; ;
            db.SaveChanges();
            
            return View(ad);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if(ad==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            var user = _authentication.GetUser();
            if(ad.AccountId==user.AccountId)
            {
                var viewmodel = new AdsViewModel(ad);
                ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
                ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
                ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
                ViewBag.TopCategoryId = new SelectList(db.toppropertycategory.Where(x => x.IsActive == true).ToList(), "TopCategoryId", "TopCategoryName", viewmodel.TopCategoryId);
                ViewBag.SubCategoryId = new SelectList(db.propertysubcategory.Where(x => x.IsActive == true).ToList(), "SubCategoryId", "SubCategoryName", viewmodel.SubCategoryId);
                ViewBag.RoomsId = new SelectList(db.propertyrooms.Where(x => x.IsActive == true).ToList(), "RoomsId", "NumberOfRooms", viewmodel.RoomsId);
                return View(viewmodel);
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is not belongs to your account please check.. ");
            }
            
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public ActionResult Edit(int Id,AdsViewModel viewmodel)
     {
         var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
         if (ad == null)
         {
             return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
         }
         if(ModelState.IsValid)
         {
             var user = _authentication.GetUser();
             if(ad.AccountId==user.AccountId)
             {
                 ad.propertyad.PropertyTitle = viewmodel.PropertyTitle;
                 ad.propertyad.TopCategoryId = viewmodel.TopCategoryId;
                 ad.propertyad.SubCategoryId = viewmodel.SubCategoryId;
                 ad.propertyad.RoomsId = viewmodel.RoomsId;
                 ad.propertyad.MaximumPrice = viewmodel.MaximumPrice;
                 ad.propertyad.MinimumPrice = viewmodel.MinimumPrice;
                 ad.propertyad.SqrFoot =Convert.ToDecimal(viewmodel.SqrFoot);
                 ad.propertyad.Address = viewmodel.Address;
                 ad.propertyad.IsFurnished = viewmodel.IsFurnished;
                 ad.propertyad.PersonName = viewmodel.PersonName;
                 ad.propertyad.ContactNumber = viewmodel.MobileNumber;
                 ad.propertyad.Address = viewmodel.Address;
                 ad.propertyad.PropertyDescription = viewmodel.PropertyDescription;
                 ad.areaads.CountryId = viewmodel.CountryId;
                 ad.areaads.StateId = viewmodel.StateId;
                 ad.areaads.CityId = viewmodel.CityId;
                 ad.IsActive = viewmodel.IsActive;
                 db.SaveChanges();
                 return RedirectToAction("Myads", "CustomProfileHandler");
             }
             else
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is not belongs to your account please check.. ");
             }
         }
         else
         {
             ModelState.AddModelError("", "Model State Is Not Valid Please check...");
         }
           ViewBag.CountryId = new SelectList(db.countries.Where(x => x.Status == true).ToList(), "CountryId", "CountryName", viewmodel.CountryId);
           ViewBag.StateId = new SelectList(db.states.Where(x => x.Status == true).ToList(), "StateId", "StateName", viewmodel.StateId);
           ViewBag.CityId = new SelectList(db.cities.Where(x => x.Status == true).ToList(), "CityId", "CityName", viewmodel.CityId);
           ViewBag.TopCategoryId = new SelectList(db.toppropertycategory.Where(x => x.IsActive == true).ToList(), "TopCategoryId", "TopCategoryName", viewmodel.TopCategoryId);
           ViewBag.SubCategoryId = new SelectList(db.propertysubcategory.Where(x => x.IsActive == true).ToList(), "SubCategoryId", "SubCategoryName", viewmodel.SubCategoryId);
           ViewBag.RoomsId = new SelectList(db.propertyrooms.Where(x => x.IsActive == true).ToList(), "RoomsId", "NumberOfRooms", viewmodel.RoomsId);
           
           return View(viewmodel);

     }
       
        [HttpGet]
        public ActionResult Delete(int Id)
         {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if(ad==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            var user = _authentication.GetUser();
            if(ad.AccountId==user.AccountId)
            {
                ViewBag.AdAddress = ad.propertyad.Address;
                return View();
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is not belongs to your account please check.. ");
            }
            
         }
        
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubmitted(int Id)
        {
            var ad = db.submitedads.Where(x => x.AdId == Id).SingleOrDefault();
            if (ad == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is null please check.. ");
            }
            var user = _authentication.GetUser();
            if (ad.AccountId == user.AccountId)
            {
                ad.IsDeleted = true;
                db.SaveChanges();
                return RedirectToAction("Myads", "CustomProfileHandler");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Ad is not belongs to your account please check.. ");
            }
            
        }
        
        public JsonResult GetTopCategory(int TopCategoryId)
        {
            var topcategory = (from s in db.toppropertycategory
                               where s.TopCategoryId == TopCategoryId
                               select new
                               {
                                   IsShowSubCategory = s.IsShowSubCategory,
                                   IsShowArea = s.IsShowArea,
                                   IsShowFurnished = s.IsShowFurnished,
                                   IsShowRooms = s.IsShowRooms

                               }).SingleOrDefault();
            return Json(topcategory, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSubCategory(int TopCategoryId)
        {
            var topcategory = (from s in db.toppropertycategory
                               where s.TopCategoryId == TopCategoryId
                               select new
                               {
                                   IsShowSubCategory = s.IsShowSubCategory,
                                   IsShowArea = s.IsShowArea,
                                   IsShowFurnished = s.IsShowFurnished,
                                   IsShowRooms = s.IsShowRooms

                               }).SingleOrDefault();
            return Json(topcategory, JsonRequestBehavior.AllowGet);
        }


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

        [HttpPost]
        public JsonResult DeleteAJAX(int? employeeId)
        {
            var user = _authentication.GetUser();
            var acc = db.accounts.Where(x => x.AccountEmailId == user.AccountEmailId).SingleOrDefault();
            if(db.savead.Where(x=>x.AdId==employeeId&&x.AccountId==acc.AccountId).Count()>0)
            {
                return Json("Ad Saved Already in your..dashboard please check...");
            }
            SaveAd data = new SaveAd()
            {
                AdId = employeeId,
                IsDeleted = false,
                AccountId = acc.AccountId,
            };
            db.savead.Add(data);
            db.SaveChanges();
            return Json("Ad Saved.. successfully!");

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