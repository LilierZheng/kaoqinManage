using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using System.Text;
using System.Web.Security;
using System.Web;
using NPOI.Util;

namespace TLO_KQGL.Controllers
{
    public class UserController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userNo">用户名</param>
        /// <param name="pwd">用户密码</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Login([FromUri] string userNo, [FromUri] string pwd)
        {
            StringBuilder sb = new StringBuilder();
            var users = (from p in db.User
                        where p.userNo == userNo && p.password == pwd
                         select p).Include("role").FirstOrDefault();
            var emps = (from p in db.Employees where p.Emp_No == userNo select p).Include("Dep").FirstOrDefault();
            var classes = (from p in db.ClassType where p.DeptID == emps.Dep.ID select p).FirstOrDefault();
            if (users == null)
            {
                sb.Append("{\"status\":\"").Append("falied").Append("\",\"Message\":\"用户名或密码错误!\"}");
                return Json(sb.ToString());
            }
            FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, userNo, DateTime.Now,
                            DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", userNo, pwd),
                            FormsAuthentication.FormsCookiePath);
            var Token = FormsAuthentication.Encrypt(token).ToString();
            //将身份信息保存在session中，验证当前请求是否是有效请求
            HttpContext.Current.Session[userNo] = Token;
            sb.Clear();
            sb.Append("{\"status\":\"").Append("success").Append("\",\"Message\":\"登录成功\"");
            sb.Append(",\"Token\":\"").Append(Token).Append("\",\"Lontitude\":\"")
                .Append(emps.Dep.lontitude).Append("\",\"Latitude\":\"")
                .Append(emps.Dep.latitude).Append("\",\"DeptName\":\"")
                .Append(emps.Dep.DeptName).Append("\",\"DeptId\":").Append("\"").Append(emps.Dep.ID).Append("\"")
                .Append(",\"EmpId\":").Append("\"").Append(emps.ID).Append("\"");

            if (classes != null)
            {
                sb.Append(",\"SignOn\":\"").Append(classes.OnWorkTime)
                    .Append("\",\"SignOff\":\"").Append(classes.OffWorkTime).Append("\"")
                  .Append(",\"ClassId\":\"").Append(classes.ID).Append("\",\"BeginSleepTime\":\"").Append(classes.BeginSleepTime)
                  .Append("\",\"EndSleepTime\":\"").Append(classes.EndSleepTime).Append("\",\"WorkEtraTime\":\"").Append(classes.WorkEtraTime).Append("\"");
            }
                foreach (var item in users.role)
                {
                    if (item.RoleName == "pro_Leader")
                    {
                        sb.Append(",\"Role\":\"").Append("pro_Leader").Append("\"");
                      }
                    if (item.RoleName == "manager")
                    {
                        sb.Append(",\"Role\":\"").Append("manager").Append("\"");
                    }
                }
            
            sb.Append("}");
            return Json(sb.ToString());
        }

        public HttpResponseMessage GetExcel(string fileName)
        {
            HttpResponseMessage result = null;
            //导出excel

            return result;
        }
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IQueryable<User> GetUser()
        {
            return db.User;
        }

        // GET api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT api/User/5 修改用户
        public IHttpActionResult PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.ID)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/User 增加用户
        [ResponseType(typeof(User))]
        public IHttpActionResult PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.User.Add(user);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE api/User/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteUser(int id)
        {
            User user = db.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            db.User.Remove(user);
            db.SaveChanges();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.User.Count(e => e.ID == id) > 0;
        }
    }
}