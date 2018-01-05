using System.Data;
using System;
using System.Collections.Generic;
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

namespace TLO_KQGL.Controllers
{
    public class AttendanceController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        private AttendenceBll bll = new AttendenceBll();
        /// <summary>
        /// 签到
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public HttpResponseMessage SignOn([FromBody] SignModel sign, [FromUri] string Token)
        {
            if (string.IsNullOrEmpty(sign.EmpId)) {
                return Request.CreateErrorResponse(HttpStatusCode.OK, "员工id不能为空！");
            }
            if (bll.GetOne(sign.EmpId) != null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, "已经签到");
            }
            Attendance att = new Attendance();
            att.ID = Guid.NewGuid();
            att.SignOn = DateTime.Now;//签到时间
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string str = date + " " + sign.SignOn.Substring(0, 2) + ":" + sign.SignOn.Substring(2, 2);
            DateTime dt = DateTime.Parse(str);
            if (att.SignOn >dt )
            {
                att.Late = true;
            }
            else
            {
                att.Late = false;
            }
            att.LeaveEary = false;
            att.SignOff = att.SignOn;//签退时间等于签到时间 
            try
            {
                db.Attendance.Add(att);
                Guid empid = Guid.Parse(sign.EmpId);
                var emp = (from p in db.Employees where p.ID == empid select p).Include("Atten").Single();
                emp.Atten.Add(att);
                Guid classid = Guid.Parse(sign.ClassId);
                var classes = (from p in db.ClassType where p.ID == classid select p).Include("Atten").Single();
                classes.Atten.Add(att);
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.OK, "考勤签到成功！");
                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }
       /// <summary>
       /// 获取签到列表
       /// </summary>
       /// <param name="EmpId"></param>
       /// <param name="Token"></param>
       /// <returns></returns>
        [HttpGet]
        [SupportFilter]
        public IHttpActionResult GetSignList([FromUri] string EmpId, [FromUri] string Token)
        {
            if (!string.IsNullOrEmpty(EmpId))
            {
                var ret = bll.GetSignList(EmpId).ToList();
                //return Json<List<Attendance>>(ret);
                return Json<List<Attendance>>(ret,GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings,Encoding.UTF8);
            }
            return null;
        }

        /// <summary>
        /// 签退
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpPost][SupportFilter]
        public HttpResponseMessage SignOff([FromBody] SignModel sign, [FromUri] string Token)
        {
           if(string.IsNullOrEmpty(sign.EmpId)) return Request.CreateErrorResponse(HttpStatusCode.OK,"员工id不能为空！"); 
           Attendance att=bll.GetOne(sign.EmpId);
           if (att.SignOff != att.SignOn) {
               return Request.CreateErrorResponse(HttpStatusCode.OK, "已经签退！");
           }
            att.SignOff=DateTime.Now;
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string str = date + " " + sign.SignOff.Substring(0, 2) + ":" + sign.SignOff.Substring(2, 2);
            DateTime dt = DateTime.Parse(str);
            if (dt > att.SignOff)//签退时间小于正常下班时间
            {
                att.LeaveEary = true;
            }
            else
            {
               att.LeaveEary=false;
            }
            db.Entry(att).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
                HttpResponseMessage response = Request.CreateErrorResponse(HttpStatusCode.OK, "考勤签退成功！");
                return response;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
        public class SignModel
        {
            public string SignOn { get; set; }//正常上班时间
            public string SignOff { get; set; }
            public string ClassId { get; set; }//班别id
            public string EmpId { get; set; }
        }
    }
}
