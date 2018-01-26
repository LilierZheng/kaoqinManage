using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TLO_KQGL.DBAccessLayer;
using TLO_KQGL.BusinessLayer;
using System.Web.Security;
using TLO_KQGL.Utilities;
using System.Web.Providers.Entities;
using Newtonsoft.Json;
using System.Text;
using TLO_KQGL.ViewModels;
using System.Data.SqlClient;


namespace TLO_KQGL.DBAccessLayer
{
    public class AttendenceDal:DalBase
    {
        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public IEnumerable<AttendanceViewModel> GetSignList(string empId)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string dateStart = date + " 00:00";
            DateTime dtS = DateTime.Parse(dateStart);
            Guid guid = Guid.Parse(empId);
            SqlParameter[] paras =new SqlParameter[2];  
           paras[0]= new SqlParameter("@empId", empId);
           paras[1] = new SqlParameter("@dt", dtS);
           string sql = @"select cast(a.ID as varchar(36)) as ID 
,CONVERT(varchar(100),A.[SignOn], 120) as SignOn
,CONVERT(varchar(100),A.[SignOff], 120) as SignOff
,A.Late,A.LeaveEary,A.IsCheck,A.IsLeave,B.IsWork,A.isRest
,cast( A.[WorkOverTime] as varchar(50)) AS WorkOverTime
 ,cast( A.[ReSign] as varchar(2)) AS ReSign
from Attendances a 
inner join calendars b on CONVERT(varchar(100), a.SignOn, 23)=CONVERT(varchar(100), b.Date, 23) 
where a.Emp_ID=@empId and a.SignOn<@dt";
            List<AttendanceViewModel> lists = new List<AttendanceViewModel>();
            var ret = db.Database.SqlQuery<AttendanceViewModel>(sql, paras).ToList();
           // var ret = (from p in db.Attendance where p.Emp.ID == guid && p.SignOn < dtS select p).ToList();
            return ret;
        }
        /// <summary>
        /// 根据id获取考勤
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Attendance GetOne(Guid empId)
        {
            string date=DateTime.Now.ToString("yyyy-MM-dd");
            string  dateStart=date+" 00:00";
            string  dateEnd=date+" 23:59";
            DateTime dtS=DateTime.Parse(dateStart);
            DateTime dtE=DateTime.Parse(dateEnd);
            var ret = (from p in db.Attendance
                       where p.SignOff >= dtS && p.SignOff <= dtE && p.Emp.ID == empId
                       select p).FirstOrDefault();
            return ret;
        }

        /// <summary>
        /// 根据id、签到时间检查该员工该天是否已签到
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<AttendanceViewModel> GetOne(Guid empId, string supplementsignon)
        {
            SqlParameter[] paras =new SqlParameter[3];  
            paras[0]= new SqlParameter("@empId", empId);
            paras[1] = new SqlParameter("@signon", supplementsignon.Substring(0,10));
            string AttenNo = supplementsignon.Substring(0, 10).Replace("-", "");
            paras[2] = new SqlParameter("@attenno", AttenNo);
            string sql = @"SELECT cast(ID as varchar(36)) as ID 
,CONVERT(varchar(100),[SignOn], 120) as SignOn
,CONVERT(varchar(100),[SignOff], 120) as SignOff
                                  ,[Late]
                                  ,[LeaveEary]
                                  ,[IsCheck]
                                  ,[Emp_ID]
                                  ,[claType_ID]
                                  ,[IsLeave]
                                  ,[isRest]
                                  ,cast([WorkOverTime] as varchar(50)) AS WorkOverTime
                                  ,cast([LeaveHours] as varchar(50)) AS LeaveHours
                                  ,[AttenNo]
                                  ,[LeaveId]
                                  ,cast([ReSign] as varchar(2)) AS ReSign
                              FROM [KQGL].[dbo].[Attendances]
                              where CONVERT(varchar(100), [SignOn], 23)=@signon and Emp_ID=@empId and AttenNo=@attenno";
            List<AttendanceViewModel> lists = new List<AttendanceViewModel>();
            var ret = db.Database.SqlQuery<AttendanceViewModel>(sql, paras).ToList();
            return ret;
        }

        /// <summary>
        /// 根据id、签退时间检查该员工该天是否已签退
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Attendance> GetOneSignoff(Guid empId, string supplementsignoff)
        {
            SqlParameter[] paras = new SqlParameter[3];
            paras[0] = new SqlParameter("@empId", empId);
            paras[1] = new SqlParameter("@signoff", supplementsignoff.Substring(0, 10));
            string AttenNo = supplementsignoff.Substring(0, 10).Replace("-", "");
            paras[2] = new SqlParameter("@attenno", AttenNo);
            string sql = @"SELECT ID
                                  ,[SignOn]
                                  ,[SignOff]
                                  ,[Late]
                                  ,[LeaveEary]
                                  ,[IsCheck]
                                  ,[Emp_ID]
                                  ,[claType_ID]
                                  ,[IsLeave]
                                  ,[isRest]
                                  ,[WorkOverTime]
                                  ,[LeaveHours]
                                  ,[AttenNo]
                                  ,[LeaveId]
                                  ,[ReSign]
                              FROM [KQGL].[dbo].[Attendances]
                              where Emp_ID=@empId and AttenNo=@attenno and SignOn=SignOff";
            List<Attendance> lists = new List<Attendance>();
            var ret = db.Database.SqlQuery<Attendance>(sql, paras).ToList();
            return ret;
        }

        /// <summary>
        /// 设置调休状态（针对周六周日出勤的）
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int SetRest(string ID,bool isRest)
        {
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@isRest", isRest);
            para[1] = new SqlParameter("@ID", ID);
            string sql = "update Attendances set isRest=@isRest where ID=@ID";
            int flag = db.Database.ExecuteSqlCommand(sql, para);
            return flag;
        }
    }
}