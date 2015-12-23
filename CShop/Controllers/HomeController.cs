using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Data;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;

        public ActionResult Index(int? Page)
        {
            Page = Page ?? 1;
            Page = Page <= 0 ? 1 : Page;
            var products = from p in unit.Products.GetAll()
                           where p.Status
                           select p;
            
            products = products.OrderBy(p => p.Cost);
            return View(products.ToPagedList(Page.Value,9));
        }
    }
}