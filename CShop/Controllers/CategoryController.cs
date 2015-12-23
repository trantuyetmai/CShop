using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;


        public PartialViewResult List()
        {
            ViewBag.Categories  = unit.Categories.GetAll().Where(t=>t.Status);
            ViewBag.Publishers = unit.Publishers.GetAll().Where(t => t.Status);
            ViewBag.Products = unit.Products.GetAll().Where(t => t.Status);
            return PartialView("CategoriesListPartial");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }



    }
}