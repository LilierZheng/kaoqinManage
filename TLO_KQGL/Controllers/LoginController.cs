using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TLO_KQGL.BusinessLayer;
using TLO_KQGL.Utilities;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;

namespace TLO_KQGL.Controllers
{
    public class LoginController : Controller
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        //
        // GET: /L/

        public ActionResult Login()
        {
            HttpCookie cookie = Request.Cookies["UserInfoRemember"];
            User Model = new User();
            if (cookie != null)
            {
                Model.userNo = cookie["userno"].ToString();
            }
            return View(Model);
        }
        public string confirmation(string userno, string password, bool doRemember)
        {
            //获取前台传回的数据
            var user = db.User.Where(b => b.userNo == userno && b.password == password).ToList();
            if (user.Count == 0) return "error";
            if (doRemember)
            {
                HttpCookie cookie = new HttpCookie("UserInfoRemember");
                cookie.HttpOnly = true;
                cookie["userno"] = userno;
                cookie.Expires = DateTime.MaxValue;
                Response.Cookies.Add(cookie);
            }
            else
            {
                HttpCookie cookie = Request.Cookies["UserInfoRemember"];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);//立即过期 
                    Response.Cookies.Add(cookie);
                }
            }
            return "ok";
        }
    }
}
