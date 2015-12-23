using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class CategoryController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;
        //
        // GET: /Admin/Category/
        public ActionResult Index()
        {
            return View(unit.Categories.GetAll());
        }

        [HttpPost]
        public ActionResult Edit(int Id, string Name)
        {
            try
            {
                unit.Categories.Update(Id,Name);
                unit.Commit();
                return Json(new { error = "" });
            }
            catch (Exception)
            {
                return Json(new { error = "Đã xảy ra lỗi" });
            }
            
        }

        public ActionResult Active(int Id)
        { 
            try 
	        {	        
		        unit.Categories.Active(Id);
                unit.Commit();
                return RedirectToAction("Index");
	        }
	        catch (Exception)
	        {
                ViewBag.Mess = "Đã xảy ra lỗi";
                return RedirectToAction("Index");
	        }
            
        }
        public ActionResult DeActive(int Id)
        {
            try
            {
                unit.Categories.DeActive(Id);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Mess = "Đã xảy ra lỗi";
                return RedirectToAction("Index");
            }

        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(string Name, bool Status)
        {
            try
            {
                unit.Categories.Add(new ModelsTest.Category { Name = Name, Status = Status });
                unit.Commit();
                ViewBag.Mess = string.Format("Thêm thành công gian hàng '{0}'", Name);
                return View();
            }
            catch (Exception)
            {
                ViewBag.Mess = string.Format("Thêm gian hàng {0} thất bại!", Name);
                return View();
            }
            
        }
	}
}