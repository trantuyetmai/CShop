using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using Data;
using WebApplication1.ModelsTest;


namespace WebApplication1.Controllers
{
    public class CommentController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;
        //
        // GET: /Comment/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Post(int? productid, string name, string content)
        {
            if (productid != null)
            {
                var comment = new Comment()
                {
                    Content = content,
                    DateCreated = DateTime.Now,
                    ProductId = productid.Value,
                    SenderName = name,
                    UserId = User.Identity.GetUserId(),
                    Status = true
                };              
                //db.Comments.Add(comment);
                //db.SaveChanges();
                unit.Comments.Add(comment);
                unit.Commit();
                
                return Json(new { Error = "", PostId = comment.Id });
            }

            return Json(new { Error = "ProductId is null" });
            
        }
        [Authorize]
        [HttpPost]
        public JsonResult Edit(int postid, string content)
        {
            var UserId = User.Identity.GetUserId();
            if (unit.Comments.GetAll().Any(t => t.Id == postid && t.UserId == UserId))
            {
                var c = unit.Comments.GetById(postid);
                if (c == null) return Json(new { error = "Not found" });
                c.Content = content;
                unit.Comments.Update(c);
                unit.Commit();
                return Json(new { Error = "" });
            }
            return Json(new { Error = "Khong duoc phep" });
            
            //var comment = db.Comments.FirstOrDefault(cm => cm.Id == postid);
            
            //if (comment != null)
            //{
            //    comment.Content = content;
            //    db.SaveChanges();
            //    return Json(new {Error=""});
            //}
            //return Json(new {Error="Not found" });
        }
        [Authorize(Roles="Admin")]
        [HttpPost]
        public JsonResult Delete(int postid)
        {
            var c = unit.Comments.GetById(postid);
            if (c == null) return Json(new { Error = "Not found" });
            unit.Comments.Delete(c);
            return Json(new { Error = "" });
        }

        [HttpPost]
        public ActionResult Paging(int? productid, int? page)
        {

            return RedirectToAction("Details", "Product", new {id = productid, page=page});
        }
    }
}
