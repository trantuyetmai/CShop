using Data;
using Model.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ModelsTest;

namespace WebApplication1.Areas.Admin.Controllers
{
   [Authorize(Roles = "Admin")]

    public class ProductController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;

        //
        // GET: /Admin/Product/
        public ActionResult Index(int? Page, int? Category, int? Publisher, int? CostLevel)
        {
            if (Category == null)
                Category = TempData[Global.Constants.CategoryFilter] as int?;
            if (Publisher == null)
                Publisher = TempData[Global.Constants.PublisherFilter] as int?;
            if (CostLevel == null)
                CostLevel = TempData[Global.Constants.CostLeverFilter] as int?;
            if (Page == null)
                Page = TempData[Global.Constants.ProductPage] as int?;
            TempData[Global.Constants.CategoryFilter] = Category;
            TempData[Global.Constants.PublisherFilter] = Publisher;
            TempData[Global.Constants.CostLeverFilter] = CostLevel;
            TempData[Global.Constants.ProductPage] = Page;

            var tmp = unit.Products.Search(Category, Publisher, CostLevel);
            var p = tmp.FirstOrDefault();
            var result = unit.Products.Paginate(Page, 10, tmp);
            return View(result);
        }

        public ActionResult Add()
        {
            ViewBag.Categories = unit.Categories.GetAll();
            ViewBag.Publishers = unit.Publishers.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddProductViewModel product, HttpPostedFileBase File)
        {
            try 
	        {
                unit.Products.Add(product, File);
                unit.Commit();
                ViewBag.Mess = string.Format("Thêm {0} thành công", product.Name);
	        }
	        catch (Exception ex)
	        {
                ViewBag.Mess = ex.Message;
	        }

            ViewBag.Categories = unit.Categories.GetAll();
            ViewBag.Publishers = unit.Publishers.GetAll();
            return View();
            
        }

        [HttpPost]
        public ActionResult Edit(int Id, string Name, double Cost, string Detail)
        {
            try
            {

                unit.Products.Update(Id, Name, Cost, Detail);
                unit.Commit();
                return Json(new { error = "" });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
            
        }

        [HttpPost]
        public ActionResult UploadImage(int Id, HttpPostedFileBase Image)
        {
            try
            {
                unit.Products.Update(Id, Image);
                unit.Commit();
                return Json(new { error = ""});
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }           
        }

        public ActionResult Active(int Id)
        {
            try
            {
                unit.Products.Active(Id);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult DeActive(int Id)
        {
            try
            {
                unit.Products.DeActive(Id);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
	}
}