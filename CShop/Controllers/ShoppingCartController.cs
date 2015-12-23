using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Data;
using WebApplication1.ModelsTest;
using System.Web.ModelBinding;

namespace WebApplication1.Controllers
{
    public class ShoppingCartController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;
        
        //
        // GET: /ShopCart/
        public ActionResult Index(int? status)
        {
            List<ShoppingCartViewModels> models = new List<ShoppingCartViewModels>() ;
            var cart = unit.Carts.GetByHttpContext();

            var cartProducts = from cm in unit.CartProducts.GetAll()
                               where cm.CartId == cart.Id
                                select cm;
            foreach(CartProduct cartProduct in cartProducts)
            {
                ShoppingCartViewModels model = new ShoppingCartViewModels();
               
                model.ImageURL = cartProduct.Product.Image;
                model.Cost = cartProduct.Product.Cost;
                model.ProductName = cartProduct.Product.Name;
                model.TotalCost = cartProduct.ProductQuantity * cartProduct.Product.Cost;
                model.ProductQuantity = cartProduct.ProductQuantity;
                model.ProductId = cartProduct.ProductId;
                model.Category = cartProduct.Product.Category.Name.ToString();
                models.Add(model);
            }

            if (status != null)
            {
                if (status.Value == 1)
                    ViewBag.Mess = "Đã xóa sản phẩm khỏi giỏ hàng!";
                else
                    ViewBag.Mess = "Đã xảy ra lỗi!";
            }
            return View(models);
        }

        [HttpPost]
        public JsonResult AddToCart(int? productid, int? quantity)
        {
            if (productid != null && quantity > 0)
            {
                //var a = HttpContext.Session["123"];
                //var b = System.Web.HttpContext.Current.Session;
                if (unit.Products.AddToCart(
                                        productid.Value, 
                                        unit.Carts.GetByHttpContext().Id, 
                                        quantity.Value) == 0)
                    return Json(new { Error = "", Success = "Thay đổi số lượng thành công" });
                unit.Commit();
                return Json(new { Error = "" , Success = "Thêm vào giỏ hàng thành công"});
            }
            else if(quantity <= 0)
                return Json(new { Error = "Vui lòng nhập số lượng lớn hơn 0" });
            else if(quantity == null)
                return Json(new { Error = "Vui lòng nhập lại số lượng" });

            return Json(new { Error = "Product Id is null" });

        }

        [HttpPost]
        public ActionResult RemoveFromCart(int? productid)
        {
            if (productid != null)
            {
                //var product = db.Products.Find(productid);
                //var cart = ShoppingCartClass.GetCart(this.HttpContext);
                //cart.RemoveProductFromCart(product) ;

                unit.Carts.RemoveProductFromCart(unit.Carts.GetByHttpContext().Id, productid.Value);
                unit.Commit();
                return RedirectToAction("Index", new { status = 1 });
            }

            return RedirectToAction("Index", new { status = 0 });

        }

        [Authorize]
        public ActionResult Order ()
        {
            //var cart = ShoppingCartClass.GetCart(this.HttpContext);
            //ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //cart.MigrateCart(user);

            unit.Carts.MigrateCart(unit.Carts.GetByHttpContext().Id, User.Identity.GetUserId());
            unit.Commit();
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult Order(OrderViewModels model)
        {

            var cart = unit.Carts.GetByHttpContext();
            var currentid = User.Identity.GetUserId();
            ApplicationUser user = unit.Users.GetAll().FirstOrDefault(t=>t.Id==currentid);
            
            //cart.CreateOrder(model,user);
          
            if(ModelState.IsValid)
            { 
            var o = new Order { 
                DateOrder = DateTime.Now,
                IsOrdered = false,
                Notes = model.Notes,
                ReceiverAddress = model.ReceiverAddress,
                ReceiverName = model.ReceiverName,
                ReceiverPhone = model.ReceiverPhone,
                UserId = User.Identity.GetUserId()
            };
            unit.Orders.Add(o);
            unit.Commit();
            unit.Orders.OrderCart(o.Id, cart.Id);
      
            unit.Commit();

            return RedirectToAction("ConfirmOrder");
            }
            else
            {
                return View();
            }
        }

                [Authorize]

        public ActionResult ConfirmOrder ()
        {
            var currentid = User.Identity.GetUserId();

            ApplicationUser user = unit.Users.GetAll().FirstOrDefault(t => t.Id == currentid);
            ViewBag.User = user;

            var order = from or in unit.Orders.GetAll()
                          where or.User.Id == user.Id && or.IsOrdered == false
                          select or;
            order = order.OrderByDescending(or => or.Id);
            ViewBag.Order = order.FirstOrDefault();
                    

            List<Product> products = new List<Product>();
            var productInOrder = from p in unit.OrderProducts.GetAll()
                                 where p.Order.Id == order.FirstOrDefault().Id
                                 select p;
            List<int> productquantitys = new List<int>();
            foreach(OrderProduct p in productInOrder)
            {
                
                products.Add(p.Product);
                productquantitys.Add(p.ProductQuantity);
            }
          
            ViewBag.Products = products;
            ViewBag.ProductsQuantity = productquantitys;
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult ConfirmOrder (int? orderId)
        {
            var cart = unit.Carts.GetByHttpContext();

            Order order = unit.Orders.GetAll().FirstOrDefault(or => or.Id == orderId);
            order.IsOrdered = true;       
            TempData["msg"] = "<script>alert('Đã xác nhận đơn hàng thành công!');</script>";
            //unit.Carts.ClearProducts(cart.Id);
            unit.Carts.ClearProducts(cart.Id);
            unit.Commit();
            return RedirectToAction("ShoppingHistory");
        }

        [Authorize]
        public ActionResult ShoppingHistory ()
        {

            ApplicationUser user = unit.Users.Current;
            ViewBag.User = user;

            ShoppingHistory shoppingHistory = new ModelsTest.ShoppingHistory();
            shoppingHistory.orders = new List<OrderModels>();

            var order = from or in unit.Orders.GetAll()
                        where or.User.Id == user.Id && or.IsOrdered == true
                        select or;

            foreach (Order or in order)
            {

                OrderModels orderModel = new OrderModels();
                orderModel.products = new List<Product>();
                orderModel.productQuantitys = new List<int>();
                orderModel.order = or;

                var productInOrder = from p in unit.OrderProducts.GetAll()
                                     where p.Order.Id == or.Id
                                     select p;
                foreach (OrderProduct p in productInOrder)
                {
                    Product product = unit.Products.GetById(p.ProductId);
                    orderModel.products.Add(product);
                    orderModel.productQuantitys.Add(p.ProductQuantity);
                   
                }
                shoppingHistory.orders.Add(orderModel);
            }
            return View(shoppingHistory);
        }
	}
}