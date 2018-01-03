using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using System.Web.Security;
using TLO_KQGL.Utilities;
using System.Web.Providers.Entities;
using Newtonsoft.Json;
using System.Text;

namespace TLO_KQGL.Controllers
{
    public class EmplyeeController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="empNo"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult LoginEmp([FromUri] string empNo, [FromUri] string pwd)
        {
            StringBuilder sb = new StringBuilder();
            var Emps = (from p in db.Employees
                        where p.Emp_No == empNo && p.PassWord == pwd
                        select p).Include("Dep").FirstOrDefault();
            var classes=(from p in db.ClassType where p.DeptID==Emps.Dep.ID select p).FirstOrDefault();
            if (Emps == null)
            {
                sb.Append("{\"status\":\"").Append("falied").Append("\",\"Message\":\"用户名或密码错误!\"}");
                return Json(sb.ToString());
            }
            FormsAuthenticationTicket token = new FormsAuthenticationTicket(0, empNo, DateTime.Now,
                            DateTime.Now.AddHours(1), true, string.Format("{0}&{1}", empNo, pwd),
                            FormsAuthentication.FormsCookiePath);
            var Token = FormsAuthentication.Encrypt(token).ToString();
            //将身份信息保存在session中，验证当前请求是否是有效请求
            HttpContext.Current.Session[empNo] = Token;
            sb.Clear();
            sb.Append("{\"status\":\"").Append("success").Append("\",\"Message\":\"登录成功\"");
            sb.Append(",\"Token\":\"").Append(Token).Append("\",\"Lontitude\":\"")
                .Append(Emps.Dep.lontitude).Append("\",\"Latitude\":\"")
                .Append(Emps.Dep.latitude).Append("\",\"DeptName\":\"")
                .Append(Emps.Dep.DeptName).Append("\",\"DeptId\":").Append("\"").Append(Emps.Dep.ID).Append("\"")
                .Append(",\"EmpId\":").Append("\"").Append(Emps.ID).Append("\"");

            if(classes!=null)
            {
                sb.Append(",\"SignOn\":\"").Append(classes.OnWorkTime)
                    .Append("\",\"SignOff\":\"").Append(classes.OffWorkTime).Append("\"")
                  .Append(",\"ClassId\":\"").Append(classes.ID).Append("\"");
            }
             sb.Append("}");
            return Json(sb.ToString());
        }

        /// <summary>
        /// 获取所有的员工
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SupportFilter]
        public HttpResponseMessage GetEmployee()
        {
            HttpResponseMessage msg = null;
            var Emps = (from p in db.Employees select p).ToList();
            if (Emps.Count > 0)
            {
                msg = Request.CreateResponse(HttpStatusCode.OK, JsonConvert.SerializeObject(Emps));
                return msg;
            }
            msg = Request.CreateErrorResponse(HttpStatusCode.NotFound, "无法找到数据");
            return msg;
        }
          [HttpGet]
        public IHttpActionResult GetEmployeeTemp()
        {
            var Emps = (from p in db.Employees select p).ToList();
            if (Emps.Count > 0)
            {
                return Json<List<Employee>>(Emps);
            }
            return null;
        }
        // GET api/Emplyee/5
        public Employee GetEmployeeById(Guid id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }
            var ss = HttpContext.Current.Session["10001"];
            return employee;
        }

        // PUT api/Emplyee/5
        public HttpResponseMessage PutEmployee(Guid id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != employee.ID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Entry(employee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Emplyee
        public HttpResponseMessage PostEmployee([FromBody] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.ID = Guid.NewGuid();
                employee.Emp_No = "10001";
                employee.Emp_Name = "张三";
                employee.Job = "net工程师";
                employee.NativePlace = "中国山东";
                employee.Address = "山东济南";
                employee.Age = 21;
                employee.BirthDay = "1991/09/07";
                employee.CreateDate = DateTime.Now;
                employee.CreateUser = "admin";
                db.Employees.Add(employee);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, employee);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = employee.ID }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Emplyee/5
        public HttpResponseMessage DeleteEmployee(Guid id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Employees.Remove(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, employee);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}