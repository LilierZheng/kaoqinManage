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
using TLO_KQGL.ViewModels;
using TLO_KQGL.Utilities;
using TLO_KQGL.BusinessLayer;

namespace TLO_KQGL.Controllers
{
    public class LeaveController : ApiController
    {
        private TLO_KQGLDAL db = new TLO_KQGLDAL();
        private LeaveBll bll = new LeaveBll();

        // GET api/Leave 获取请假条
        [HttpGet]
        public IQueryable<LeaveViewModel> Getleave()
        {
            return bll.GetLeave().AsQueryable();
        }
        //获取请假假别种类
        [HttpGet][SupportFilter]
        public IQueryable<DictionaryViewModel> GetLeaveDic([FromUri] string Token)
        {
            return bll.GetLeaveDic().AsQueryable();
        }
        /// <summary>
        /// 根据条件查询假条记录
        /// </summary>
        /// <param name="beginDate">假条创建时间范围：开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="isCheck">是否审核</param>
        /// <returns></returns>
        [HttpGet]
        [SupportFilter]
        public IQueryable<LeaveViewModel> Getleave(string beginDate, string endDate, bool isCheck, string Token)
        {
            return bll.GetLeaveBy(beginDate, endDate, isCheck).AsQueryable();
        }
        /// <summary>
        /// 根据员工id获取假条
        /// </summary>
        /// <param name="EmpId"></param>
        /// <param name="Token"></param>
        /// <returns></returns>
        [HttpGet]
        [SupportFilter]
        public IQueryable<LeaveViewModel> GetleaveByEmpId(string EmpId, string Token)
        {  
            if (!string.IsNullOrEmpty(EmpId))
                return bll.GetLeaveByEmpId(Guid.Parse(EmpId)).AsQueryable();
            else
                return null;
        }
        // GET api/Leave/5
        [HttpGet][SupportFilter]
         [ResponseType(typeof(LeaveViewModel))]
        public IHttpActionResult GetLeave(string id,string Token)
        {
            LeaveViewModel leave = bll.GetLeaveById(id);
            if (leave == null)
            {
                return NotFound();
            }

            return Ok(leave);
        }
        /// <summary>
        /// 审核请假条
        /// </summary>
        /// <param name="id">假条id</param>
        /// <param name="IsPass">是否同意</param>
        /// <returns></returns>
        public IHttpActionResult AuditLeave([FromBody] LeaveViewModel leave)
        {
            if (string.IsNullOrEmpty(leave.ID))
            {
                return BadRequest();
            }
            Guid _id = Guid.Parse(leave.ID);
            DateTime beginDt =DateTime.Parse( leave.leaveBeginDate.Substring(0,10));
            DateTime endDt =DateTime.Parse(leave.leaveEndDate.Substring(0, 10));
            int days = (endDt - beginDt).Days;
            if (bll.AuditLeave(leave,days) > 0)
            {
                return StatusCode(HttpStatusCode.NoContent);
            }
            return BadRequest();
        }

        // PUT api/Leave/5
        public IHttpActionResult PutLeave(Guid id, Leave leave)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != leave.ID)
            {
                return BadRequest();
            }

            db.Entry(leave).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LeaveExists(id))
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

        /// <summary>
        /// 创建请假条
        /// </summary>
        /// <param name="leave">
        /// </param>
        /// <returns></returns>
        [HttpPost]
        [SupportFilter]
        public HttpResponseMessage PostLeave([FromBody]LeaveViewModel leave, [FromUri] string token)
        {
            HttpResponseMessage msg = null;
            Leave leaModel = new Leave();
            if (string.IsNullOrEmpty(leave.leaveBeginDate) || string.IsNullOrEmpty(leave.leaveEndDate))
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假开始结束时间不能为空！");
                return msg;
            }
            if (string.IsNullOrEmpty(leave.Title))
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假标题不能为空！");
                return msg;
            }
            if (string.IsNullOrEmpty(leave.Content))
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假正文不能为空！");
                return msg;
            }

            leaModel.ID = Guid.NewGuid();
            leaModel.Title = leave.Title;
            leaModel.Content = leave.Content;
            leaModel.LeaveBeginDate = DateTime.Parse(leave.leaveBeginDate);
            leaModel.LeaveEndDate = DateTime.Parse(leave.leaveEndDate);
            leaModel.IsPass = false;
            leaModel.IsCheck = false;
            leaModel.LeaveType = leave.LeaveType;
            string startDate = leave.leaveBeginDate.Substring(0, 10);

            DateTime dtStart = DateTime.Parse(leaModel.LeaveBeginDate.ToString());
            DateTime dtEnd = DateTime.Parse(leaModel.LeaveEndDate.ToString());
            DateTime dtSleepStart = DateTime.Parse(startDate + " " + leave.BeginSleepTime.Substring(0, 2) + ":" + leave.leaveBeginDate.Substring(2, 2));//午休开始时间
            DateTime dtSleepEnd = DateTime.Parse(startDate + " " + leave.leaveEndDate.Substring(0, 2) + ":" + leave.leaveEndDate.Substring(2, 2));//午休结束时间
            TimeSpan tsSleepStart = new TimeSpan(dtSleepStart.Ticks);
            TimeSpan tsSleepEnd = new TimeSpan(dtSleepEnd.Ticks);
            TimeSpan tsSleep = tsSleepEnd.Subtract(tsSleepStart).Duration();
            decimal sleepHours = decimal.Parse(tsSleep.TotalHours.ToString("0.0"));//午休时间
            //判断开始结束时间在正常的范围之内，不在正常范围内的强制在正常范围时间
            string startTime = leave.leaveBeginDate.Substring(11, 5).Remove(2, 1);
            string endTime = leave.leaveEndDate.Substring(11, 5).Remove(2, 1);
            string endDate = leave.leaveEndDate.Substring(0, 10);
            int intStartTime = int.Parse(startTime);
            int intEndTime = int.Parse(endTime);
            int onWorkTime = int.Parse(leave.OnWorkTime);
            int offWorkTime = int.Parse(leave.OffWorkTime);
            int endSleepTime = int.Parse(leave.EndSleepTime);
            int startSleepTime = int.Parse(leave.BeginSleepTime);
            if (intStartTime < onWorkTime || intEndTime > offWorkTime)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假开始结束时间不规范！");
                return msg;
            }
            if (intStartTime > startSleepTime && intStartTime < endSleepTime)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假开始时间不规范，不应在午休时间范围内！");
                return msg;
            }
            if (intEndTime > startSleepTime && intEndTime < endSleepTime)
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假结束时间不规范，不应在午休时间范围内！");
                return msg;
            }
            if (!IsWorkDay(dtStart) || !IsWorkDay(dtEnd))
            {
                msg = Request.CreateErrorResponse(HttpStatusCode.OK, "请假开始时间或者结束时间在双休日或者节假日范围内");
                return msg;

            }
            leaModel.CreateUser = leave.CreateUser;
            leaModel.CreateDate = DateTime.Now;
            leaModel.Hours = GetLeaveDay(dtStart, dtEnd, int.Parse(leave.OnWorkTime.Substring(0, 2)),
                int.Parse(leave.OffWorkTime.Substring(0, 2)), int.Parse(leave.OnWorkTime.Substring(2, 2)),
                int.Parse(leave.OffWorkTime.Substring(2, 2)), leave.BeginSleepTime, leave.EndSleepTime);
            leaModel.IsCheck = false;//未审核
            Guid guid = Guid.Parse(leave.empId);
            var emps = db.Employees.Include("leave").Single(p => p.ID == guid);
            emps.leave.Add(leaModel);
            db.leave.Add(leaModel);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                throw;
            }
            msg = Request.CreateErrorResponse(HttpStatusCode.OK, "新增假条成功！");
            return msg;
        }

        // DELETE api/Leave/5
        [ResponseType(typeof(Leave))]
        public IHttpActionResult DeleteLeave(Guid id)
        {
            Leave leave = db.leave.Find(id);
            if (leave == null)
            {
                return NotFound();
            }

            db.leave.Remove(leave);
            db.SaveChanges();

            return Ok(leave);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LeaveExists(Guid id)
        {
            return db.leave.Count(e => e.ID == id) > 0;
        }
        /// <summary>
        /// 计算请假时间
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="startWork"></param>
        /// <param name="endWork"></param>
        /// <param name="startMin"></param>
        /// <param name="endMin"></param>
        /// <param name="startSleep"></param>
        /// <param name="endSleep"></param>
        /// <returns></returns>
        private int GetLeaveDay(DateTime dtStart, DateTime dtEnd, int startWork, int endWork, int startMin, int endMin, string startSleep, string endSleep)
        {
            int startSleeHour = int.Parse(startSleep.Substring(0, 2));
            int startSleeMin = int.Parse(startSleep.Substring(2, 2));
            int endSleepHour = int.Parse(endSleep.Substring(0, 2));
            int endSleepMin = int.Parse(endSleep.Substring(2, 2));
            DateTime dtFirstDayGoToWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, startWork, startMin, 0);//请假第一天的上班时间
            DateTime dtFirstDayGoOffWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, endWork, endMin, 0);//请假第一天的下班时间

            DateTime dtLastDayGoToWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, startWork, startMin, 0);//请假最后一天的上班时间
            DateTime dtLastDayGoOffWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, endWork, endMin, 0);//请假最后一天的下班时间

            DateTime dtFirstDayRestStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, startSleeHour, startSleeMin, 0);//请假第一天的午休开始时间
            DateTime dtFirstDayRestEnd = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, endSleepHour, endSleepMin, 0);//请假第一天的午休结束时间

            DateTime dtLastDayRestStart = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, startSleeHour, startSleeMin, 0);//请假最后一天的午休开始时间
            DateTime dtLastDayRestEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, endSleepHour, endSleepMin, 0);//请假最后一天的午休结束时间

            //如果开始请假时间早于上班时间或者结束请假时间晚于下班时间，者需要重置时间
            if (!IsWorkDay(dtStart) && !IsWorkDay(dtEnd))
                return 0;
            if (dtStart >= dtFirstDayGoOffWork && dtEnd <= dtLastDayGoToWork && (dtEnd - dtStart).TotalDays < 1)
                return 0;
            if (dtStart >= dtFirstDayGoOffWork && !IsWorkDay(dtEnd) && (dtEnd - dtStart).TotalDays < 1)
                return 0;

            if (dtStart < dtFirstDayGoToWork)//早于上班时间
                dtStart = dtFirstDayGoToWork;
            if (dtStart >= dtFirstDayGoOffWork)//晚于下班时间
            {
                while (dtStart < dtEnd)
                {
                    dtStart = new DateTime(dtStart.AddDays(1).Year, dtStart.AddDays(1).Month, dtStart.AddDays(1).Day, 9, 0, 0);
                    if (IsWorkDay(dtStart))
                    {
                        dtFirstDayGoToWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 8, 30, 0);//请假第一天的上班时间
                        dtFirstDayGoOffWork = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 17, 30, 0);//请假第一天的下班时间
                        dtFirstDayRestStart = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 11, 30, 0);//请假第一天的午休开始时间
                        dtFirstDayRestEnd = new DateTime(dtStart.Year, dtStart.Month, dtStart.Day, 12, 30, 0);//请假第一天的午休结束时间

                        break;
                    }
                }
            }

            if (dtEnd > dtLastDayGoOffWork)//晚于下班时间
                dtEnd = dtLastDayGoOffWork;
            if (dtEnd <= dtLastDayGoToWork)//早于上班时间
            {
                while (dtEnd > dtStart)
                {
                    dtEnd = new DateTime(dtEnd.AddDays(-1).Year, dtEnd.AddDays(-1).Month, dtEnd.AddDays(-1).Day, 18, 0, 0);
                    if (IsWorkDay(dtEnd))
                    {
                        dtLastDayGoToWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 8, 30, 0);//请假最后一天的上班时间
                        dtLastDayGoOffWork = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 17, 30, 0);//请假最后一天的下班时间
                        dtLastDayRestStart = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 11, 30, 0);//请假最后一天的午休开始时间
                        dtLastDayRestEnd = new DateTime(dtEnd.Year, dtEnd.Month, dtEnd.Day, 12, 30, 0);//请假最后一天的午休结束时间
                        break;
                    }
                }
            }

            //计算请假第一天和最后一天的小时合计数并换算成分钟数           
            double iSumMinute = dtFirstDayGoOffWork.Subtract(dtStart).TotalMinutes + dtEnd.Subtract(dtLastDayGoToWork).TotalMinutes;//计算获得剩余的分钟数

            if (dtStart > dtFirstDayRestStart && dtStart < dtFirstDayRestEnd)
            {//开始休假时间正好是在午休时间内的，需要扣除掉
                iSumMinute = iSumMinute - dtFirstDayRestEnd.Subtract(dtStart).Minutes;
            }
            if (dtStart < dtFirstDayRestStart)
            {//如果是在午休前开始休假的就自动减去午休的60分钟
                iSumMinute = iSumMinute - 60;
            }
            if (dtEnd > dtLastDayRestStart && dtEnd < dtLastDayRestEnd)
            {//如果结束休假是在午休时间内的，例如“请假截止日是1月31日 12:00分”的话那休假时间计算只到 11:30分为止。
                iSumMinute = iSumMinute - dtEnd.Subtract(dtLastDayRestStart).Minutes;
            }
            if (dtEnd > dtLastDayRestEnd)
            {//如果是在午休后结束请假的就自动减去午休的60分钟
                iSumMinute = iSumMinute - 60;
            }


            int leaveday = 0;//实际请假的天数
            double countday = 0;//获取两个日期间的总天数

            DateTime tempDate = dtStart;//临时参数
            while (tempDate < dtEnd)
            {
                countday++;
                tempDate = new DateTime(tempDate.AddDays(1).Year, tempDate.AddDays(1).Month, tempDate.AddDays(1).Day, 0, 0, 0);
            }
            //循环用来扣除双休日、法定假日 和 添加调休上班
            for (int i = 0; i < countday; i++)
            {
                DateTime tempdt = dtStart.Date.AddDays(i);
                if (IsWorkDay(tempdt))
                    leaveday++;
            }

            //去掉请假第一天和请假的最后一天，其余时间全部已8小时计算。
            //SumMinute/60： 独立计算 请假第一天和请假最后一天总归请了多少小时的假
            double doubleSumHours = 0;
            if (leaveday < 2)
            {
                doubleSumHours = iSumMinute / 60;
            }
            else
            {
                doubleSumHours = ((leaveday - 2) * 8) + iSumMinute / 60;
            }

            int intSumHours = Convert.ToInt32(doubleSumHours);

            if (doubleSumHours > intSumHours)//如果请假时间不足1小时话自动算作1小时
                intSumHours++;

            return intSumHours;
        }
        /// 判断是否是工作日| true：工作 | flase：休息
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns>true：工作 | flase:休息</returns>
        private bool IsWorkDay(DateTime date)
        {
            try
            {
                string year = date.Year.ToString();
                string DateKey = date.ToString("yyyy-MM-dd");
                var cal = db.calendar.Where(p => p.Date == DateKey).ToList().FirstOrDefault();
                return cal.IsWork;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}