using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;


namespace TLO_KQGL.DBAccessLayer
{
    public class AttendenceDal:DalBase
    {
        /// <summary>
        /// 获取签到列表
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public IEnumerable<Attendance> GetSignList(string empId)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string dateStart = date + " 00:00";
            DateTime dtS = DateTime.Parse(dateStart);
            Guid guid = Guid.Parse(empId);
            var ret = (from p in db.Attendance where p.Emp.ID ==guid &&p.SignOn<dtS select p).ToList();
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
                       where p.SignOff >= dtS && p.SignOff <= dtE && p.Emp.ID==empId 
                       select p).FirstOrDefault();
            return ret;
        }
    }
}