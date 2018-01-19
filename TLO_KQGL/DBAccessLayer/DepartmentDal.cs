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
        /// <summary>
        /// 获取部门字典
        /// </summary>
        /// <returns></returns>
        public IDictionary<int, string> GetDeptDic()
        {
            var ret = db.Department.Select(p => new
            {
                code = p.ID,
                value = p.DeptNo + p.DeptName
            }).AsEnumerable().ToDictionary(p => p.code, p => p.value);
            return ret;
        }
    }
}