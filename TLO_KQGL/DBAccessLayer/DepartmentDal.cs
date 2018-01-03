using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;

namespace TLO_KQGL.DBAccessLayer
{
    public class DepartmentDal:DalBase
    {
        public IEnumerable<Department> GetList()
        {
            var ret = (from p in db.Department
                       select p).ToList();
            return ret;
        }
    }
}