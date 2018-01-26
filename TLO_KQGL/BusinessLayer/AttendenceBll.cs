using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using TLO_KQGL.ViewModels;

namespace TLO_KQGL.BusinessLayer
{
    public class AttendenceBll
    {
        private AttendenceDal dal = new AttendenceDal();
        public IEnumerable<AttendanceViewModel> GetSignList(string empId)
        {
            return dal.GetSignList(empId);
        }

        public Attendance GetOne(string id)
        {
            Guid guid = Guid.Parse(id);
            return dal.GetOne(guid);
        }

        public IEnumerable<AttendanceViewModel> GetOne(string id, string supplementsignon)
        {
            Guid guid = Guid.Parse(id);
            return dal.GetOne(guid, supplementsignon);
        }

        public IEnumerable<Attendance> GetOneSignoff(string id, string supplementsignoff)
        {
            Guid guid = Guid.Parse(id);
            return dal.GetOneSignoff(guid, supplementsignoff);
        }

        /// <summary>
        /// 设置调休状态（针对周六周日出勤的）
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int SetRest(string id,bool isRest)
        {
            return dal.SetRest(id,isRest);
        }
    }
}