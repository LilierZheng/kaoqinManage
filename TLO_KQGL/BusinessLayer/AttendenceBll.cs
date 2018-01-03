using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;

namespace TLO_KQGL.BusinessLayer
{
    public class AttendenceBll
    {
        private AttendenceDal dal = new AttendenceDal();
        public IEnumerable<Attendance> GetSignList(string empId)
        {
            return dal.GetSignList(empId);
        }

        public Attendance GetOne(string id)
        {
            Guid guid = Guid.Parse(id);
            return dal.GetOne(guid);
        }
    }
}