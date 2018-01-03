using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;


namespace TLO_KQGL.DBAccessLayer
{
    public class EmployeeDal:DalBase
    {
        public IEnumerable<Employee> GetListByEmpNo(string empNo, string pwd)
        {
            var Emps = (from p in db.Employees
                        where p.Emp_No == empNo && p.PassWord == pwd
                        select p).ToList();
            return Emps;
        }
        public IEnumerable<Employee> GetList()
        {
            var Emps = (from p in db.Employees select p).ToList();
           
            return Emps;
        }
        //public IEnumerable<Employee> GetListAndEmp()
        //{
        //    //Employee tmp = db.Employees.First();
        //    //tmp.Dep.ID;

        //    //return Emps;
            
        //}
        public int AddEmp(Employee model)
        {
            db.Employees.Add(model);
            int flag = db.SaveChanges();
            return flag;
        }
    }
}