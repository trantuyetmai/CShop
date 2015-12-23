using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Data;
using PagedList;
using Data;
using WebApplication1.ModelsTest;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        IUnitOfWork unit = MyUnitOfWork.Instance;

        //
        // GET: /Product/
        public ActionResult Index()
        {
            var products = unit.Products.GetAll().Where(t => t.Status);
            return View(products.ToList());
        }

        //
        // GET: /Product/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id != null)
            {
                var product = unit.Products.GetById(id.Value);
                if (product == null)
                {
                    return HttpNotFound();
                }
                // tăng số lượt xem 
                product.TimeAccess += 1;
                unit.Products.Update(product);
                unit.Commit();

                // lấy danh sách comment và cho vào ViewBag
                var comments = unit.Comments.GetByProductId(id.Value);
                comments = comments.OrderByDescending(cm => cm.Id);

                // phân trang comment
                int currentPageIndex = page.HasValue ? page.Value : 1;
                ViewBag.Comments = comments.ToPagedList(currentPageIndex, 4);

                //related products
                var relatedProducts = unit.Products.GetRelatedProduct(id.Value);
                ViewBag.RelateProducts = relatedProducts;

                return View(product);
            }
            return HttpNotFound();
        }

        // GET: /Search/
        public ActionResult Search(String Name, int? Price, int? Category, int? Publisher, int? Page)
        {
            var Prices = new[]
             {
                new { Id = 1, Name = "Dưới 2 triệu" },
                new { Id = 2, Name = "Từ 2 triệu đến 4 triệu" },
                new { Id = 3, Name = "Từ 4 triệu đến 6 triệu" },
                 new { Id = 4, Name = "Trên 6 triệu" }
    
             };

            ViewBag.Prices = Prices;
            ViewBag.Categories = unit.Categories.GetAll().Where(t => t.Status).ToList();
            ViewBag.Publishers = unit.Publishers.GetAll().Where(t => t.Status).ToList();

            ViewBag.Name = Name;
            ViewBag.Price = Price;
            ViewBag.Category = Category;
            ViewBag.Publisher = Publisher;

            var products = unit.Products.GetAll().Where(t => t.Status);

            // Tìm kiếm theo tên
            if (!String.IsNullOrEmpty(Name))
            {
                products = from p in products
                           where p.Name.Contains(Name)
                           select p;
            }

            // Tìm kiếm theo mức giá
            if (Price != null)
            {
                switch (Price)
                {
                    case 1:
                        products = from p in products
                                   where p.Cost < 2000000
                                   select p;
                        break;
                    case 2:
                        products = from p in products
                                   where (p.Cost <= 4000000) && (p.Cost >= 2000000)
                                   select p;
                        break;
                    case 3:
                        products = from p in products
                                   where (p.Cost <= 6000000) && (p.Cost >= 4000000)
                                   select p;
                        break;
                    case 4:
                        products = from p in products
                                   where (p.Cost > 6000000)
                                   select p;
                        break;
                }
            }

            // Tìm kiếm theo Loại sản phẩm
            if (Category != null)
            {
                int id = Category.Value;
                products = from p in products
                           where p.CategoryId.Equals(id) || id == -1
                           select p;
            }

            // Tìm kiếm theo Nhà sản xuất
            if (Publisher != null)
            {
                int id = Publisher.Value;
                products = from p in products
                           where p.PublisherId.Equals(id)
                           select p;
            }
            products = products.OrderBy((p) => p.Cost);

            //Phan trang
            return View("Search", unit.Products.Paginate(Page, Global.Constants.NumItemOfPage, products));
        }

        public PartialViewResult HotProduct()
        {
            return PartialView("HotProductPartial", unit.Products.GetAllHot(Global.Constants.MaxHotProduct).Where(t => t.Status));
        }


        //public ActionResult ViewByType(string id, string sortOrder)
        //{
        //    var type = from p in unit.Products.GetAll()
        //               select p;


        //    if (!String.IsNullOrEmpty(id))
        //    {
        //        int idx = int.Parse(id);
        //        type = from p in unit.Products.GetAll()
        //               where p.CategoryId == idx
        //               select p;
        //    }

        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.CostSortParm = sortOrder == "Cost" ? "Cost_desc" : "Cost";
        //    ViewBag.StatusSortParm = sortOrder == "Status" ? "Status_desc" : "Status";

        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            type = type.OrderByDescending(s => s.Name);
        //            break;
        //        case "Cost":
        //            type = type.OrderBy(s => s.Cost);
        //            break;
        //        case "Cost_desc":
        //            type = type.OrderByDescending(s => s.Cost);
        //            break;
        //        case "Status":
        //            type = type.OrderBy(s => s.Status);
        //            break;
        //        case "Status_desc":
        //            type = type.OrderByDescending(s => s.Status);
        //            break;

        //        default:
        //            type = type.OrderBy(s => s.Name);
        //            break;
        //    }

        //    return View("Index",type.ToList());
        //}


        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
