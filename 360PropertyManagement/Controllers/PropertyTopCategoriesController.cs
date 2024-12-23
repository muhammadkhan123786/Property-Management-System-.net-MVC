using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using _360PropertyManagement.Models;
using _360PropertyManagement.ViewModels;
using System.Web.Routing;
using System.Net;
using System.Drawing;


namespace _360PropertyManagement.Controllers
{
    public class PropertyTopCategoriesController : Controller
    {
        private Context db = new Context();
        private FormsAuthenticationService _authentication = new FormsAuthenticationService(new HttpContextWrapper(System.Web.HttpContext.Current));
         //
        // GET: /PropertyTopCategories/
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
                catgories = catgories.Where(c => c.TopCategoryName.Contains(searchString)&& c.IsDeleted == false);

            }
            catgories = catgories.OrderByDescending(x => x.TopCategoryId);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(catgories.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewmodel = new TopPropertyCategoryViewModel();
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(TopPropertyCategoryViewModel viewmodel)
        {
            if(ModelState.IsValid)
            {
                if (PropertyCategoryCheck(viewmodel.TopCategoryName))
                {
                    var uploadImage = new Bitmap(viewmodel.Photo.InputStream);
                    var newimage = Extensions.ResizeCategoryImage(uploadImage);
                    var imagebytes = Extensions.ImageToByte(newimage);
                    var TosaveImage = new Images()
                    {
                        Image = imagebytes,
                        IsDeleted = false
                    };
                    db.images.Add(TosaveImage);
                           
                    var category = new TopCategory()
                    {
                        TopCategoryName=viewmodel.TopCategoryName,
                        Remarks=viewmodel.Remarks,
                        ImageId=TosaveImage.ImageId,
                        IsActive=viewmodel.IsActive,
                        IsDeleted=false,
                        IsShowArea=viewmodel.IsShowArea,
                        IsShowFurnished=viewmodel.IsShowFurnished,
                        IsShowRooms=viewmodel.IsShowRooms,
                        IsShowSubCategory=viewmodel.IsShowSubCategory
                     };
                    db.toppropertycategory.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("","The Top Category Already Exists...Please check..");
                }
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
            var category = db.toppropertycategory.Where(x => x.TopCategoryId == Id).SingleOrDefault();
            if(category==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest,"The Category does Not Exists Please check....");
            }
            ViewBag.Category = category.TopCategoryName;
            return View();
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int Id)
        {
            var category = db.toppropertycategory.Where(x => x.TopCategoryId == Id).SingleOrDefault();
            if(category==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Category does Not Exists Please check....");
            }
            try
            {
                category.IsDeleted = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please checkk...."+ex.ToString());
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var topcategory = db.toppropertycategory.Where(x => x.TopCategoryId == Id).SingleOrDefault();
            if(topcategory==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "The Category does not exists... Please check....");
            }
            var viewmodel = new TopPropertyCategoryViewModel(topcategory);
            return View(viewmodel);
        }


        public bool PropertyCategoryCheck(string name)
        {
            if (db.toppropertycategory.Where(x => x.TopCategoryName==name && x.IsDeleted == false).Count() > 0)
            {
                return false;
            }
            return true;
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var category = db.toppropertycategory.Where(x => x.TopCategoryId == Id).SingleOrDefault();
            if(category==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category Does Not Exists To Edit");
            }
            var Viewmodel = new TopPropertyCategoryViewModel(category);
            return View(Viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id,TopPropertyCategoryViewModel viewmodel)
        {
            var category = db.toppropertycategory.Where(x => x.TopCategoryId == Id).SingleOrDefault();
            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category Does Not Exists To Edit");
            }
            if(ModelState.IsValid)
            {
                category.TopCategoryName = viewmodel.TopCategoryName;
                category.Remarks = viewmodel.Remarks;
                category.IsActive = viewmodel.IsActive;
                category.IsShowSubCategory = viewmodel.IsShowSubCategory;
                category.IsShowRooms = viewmodel.IsShowRooms;
                category.IsShowFurnished = viewmodel.IsShowFurnished;
                category.IsShowArea = viewmodel.IsShowArea;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Model State Is Not Valid Please check....");
            }
            return View(viewmodel);
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