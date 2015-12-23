using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PublisherController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;

        //
        // GET: /Admin/Publisher/
        public ActionResult Index()
        {
            return View(unit.Publishers.GetAll());
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string Name, string Detail, bool Status)
        {
            try
            {
                unit.Publishers.Add(new ModelsTest.Publisher { Name = Name, Detail = Detail, Status = Status });
                unit.Commit();
                ViewBag.Mess = string.Format("Thêm thành công {0}", Name);
                return View();
            }
            catch (Exception)
            {
                ViewBag.Mess = string.Format("Thêm {0} thất bại", Name);
                return View();
            }            
        }

        public ActionResult Active(int Id)
        {
            try
            {
                unit.Publishers.Active(Id);
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
                unit.Publishers.DeActive(Id);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int Id, string Name, string Detail)
        {
            try
            {
                unit.Publishers.Update(Id, Name, Detail);
                unit.Commit();
                return Json(new { error = "" });
            }
            catch (Exception)
            {
                return Json(new { error = "Đã xảy ra lỗi" });
            }
            

        }

	}
}