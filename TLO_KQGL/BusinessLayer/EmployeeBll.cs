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
        public IEnumerable<Employee> GetListByEmpNo(string empNo, string pwd)
        {
            return dal.GetListByEmpNo(empNo, pwd);
        }
        public IEnumerable<Employee> GetList()
        {
            return dal.GetList();
        }
        public int AddEmp(Employee model)
        {
            return dal.AddEmp(model);
        }
    }
}