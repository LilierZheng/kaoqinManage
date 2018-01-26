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
using TLO_KQGL.BusinessLayer;
using System.Web.Security;
using TLO_KQGL.Utilities;
using System.Web.Providers.Entities;
using Newtonsoft.Json;
using System.Text;
using TLO_KQGL.ViewModels;

namespace TLO_KQGL.Controllers
{
    public class EmplyeeController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        private EmployeeBll bll = new EmployeeBll();

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
        /// <summary>
        /// 获取对应权限下得所有未审核的员工
        /// </summary>
        /// <param name="DepId">部门id</param>
        /// <param name="empId">员工id</param>
        /// <returns></returns>
        [HttpGet]
          public IHttpActionResult GetAuditEmp(string DepId,string empId)
          {
              if (string.IsNullOrEmpty(DepId) || string.IsNullOrEmpty(empId))
              {
                  return null;
              }
             List<Employee> list= bll.GetAuditEmp(DepId, empId).ToList();
             return Json < List < Employee >>( list,Configuration.Formatters.JsonFormatter.SerializerSettings,Encoding.UTF8);
          }
        [HttpGet]
        public IQueryable<AttendanceViewModel> GetAttendanceForExcel(string empId)
        {
            if (string.IsNullOrEmpty(empId)) return null;
            Guid id = Guid.Parse(empId);
            var listEMP=bll.GetAttendanceForExcel(id).ToList();
            var list = bll.GetAttendanceForExcel(id).ToDictionary(p => p.EmpId);
            var results = listEMP.GroupBy(P => P.EmpId).ToDictionary(group => group.Key, group => group.ToList());
            return bll.GetAttendanceForExcel(id).AsQueryable();
        }
        /// <summary>
        /// 审核员工
        /// </summary>
        /// <param name="attenIds">员工考勤id字符串集合&#13;</param>
        /// <returns></returns>
        [HttpPost][SupportFilter]
        public HttpResponseMessage AuditEmp(string attenIds,string Token)
        {
            HttpResponseMessage msg = null;
            if (string.IsNullOrEmpty(attenIds))
            {
                msg=Request.CreateErrorResponse(HttpStatusCode.OK,"考勤id传入不能为空！");
                return msg;
            }
            if (bll.UpdateAuditEmp(attenIds) > 0)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "审核成功！");
                return msg;
            }
            msg=Request.CreateErrorResponse(HttpStatusCode.OK, "审核失败，请联系管理员！");
            return msg;
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