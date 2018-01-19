using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;

namespace TLO_KQGL.BusinessLayer
{
    public class DepartmentBll
    {
        private DepartmentDal dal = new DepartmentDal();
        public IEnumerable<Department> GetList()
        {
            return dal.GetList();
        }
        public IDictionary<int, string> GetDeptDic()
        {
            return dal.GetDeptDic();
        }
    }
}