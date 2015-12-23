using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;
        //
        // GET: /Admin/Order/
        public ActionResult Index(int? Page)
        {
            
            ViewBag.OrderProducts = unit.OrderProducts.GetAll();
            return View(unit.Orders.Paginate(Page, 10));
        }
	}
}