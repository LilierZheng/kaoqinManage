using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;

namespace TLO_KQGL.BusinessLayer
{
    public class EmployeeBll
    {
        private EmployeeDal dal = new EmployeeDal();
        public IEnumerable<Employee> GetListByEmpNo(string empNo)
        {
            return dal.GetListByEmpNo(empNo);
        }
        public IEnumerable<Employee> GetList()
        {
            return dal.GetList();
        }
        public int AddEmp(Employee model)
        {
            return dal.AddEmp(model);
        }
        /// <summary>
        /// 获取审核人下属员工和考勤信息
        /// </summary>
        /// <param name="deptid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetAuditEmp(string deptid,string id)
        {
            int _deptId = int.Parse(deptid);
            Guid _id = Guid.Parse(id);
            return dal.GetAuditEmp(_deptId, _id);
        }
        
    }
}