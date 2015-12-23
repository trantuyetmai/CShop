using Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.ModelsTest;


namespace WebApplication1.Areas.Admin.Controllers
{
    [Authorize(Roles="Admin")]
    public class AccountController : Controller
    {
        MyUnitOfWork unit = MyUnitOfWork.Instance;

        //GET   : Admin/Account/Index
        //Mo ta : Lay danh sach tai khoan
        public ActionResult Index(int? Page)
        {
            ViewBag.Roles = unit.Roles.GetAll();
            if (Page == null)
                Page = TempData[Global.Constants.AccountPageKey] as int?;
            TempData[Global.Constants.AccountPageKey] = Page;

            return View(unit.Users.Paginate(Page, 10));
        }

        //POST  :   Admin/Account/Edit
        //Mo ta :   Chinh sua thong tin user
        [HttpPost]
        public ActionResult Edit(EditUserViewModel user)
        {
            try
            {
                unit.Users.Update(user);
                unit.Commit();
                return Json(new { error = "", data = user });
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }            
        }

        //POST: Admin/Account/ActiveUser
        //Mo ta :   Xac thuc user truc tiep, khong can thong qua email
        [Authorize(Roles = "Admin")]
        public ActionResult ActiveUser(string UserName)
        {
            try
            {
                unit.Users.Active(UserName);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Mess = "Đã xảy ra lỗi";
                return RedirectToAction("Index");
            }            
        }

        [Authorize(Roles="Admin")]
        public ActionResult Grant(string UserName, string RoleName)
        {
            try
            {
                unit.Users.Grant(UserName, RoleName);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Mess = "Đã xảy ra lỗi";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeGrant(string UserName, string RoleName)
        {
            try
            {
                unit.Users.DeGrant(UserName, RoleName);
                unit.Commit();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Mess = "Đã xảy ra lỗi";
                return RedirectToAction("Index");
            }
        }


    }

}