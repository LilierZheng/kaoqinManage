using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;




namespace TLO_KQGL.DBAccessLayer
{
    public class EmployeeDal:DalBase
    {
        public IEnumerable<Employee> GetListByEmpNo(string empNo)
        {
            var Emps = (from p in db.Employees
                        where p.Emp_No == empNo
                        select p).ToList();
            return Emps;
        }
        public IEnumerable<Employee> GetAuditEmp(int deptId,Guid id)
        {
            var emps = db.Employees.Where(p=>p.Dep.ID==deptId).Include(p => p.Atten).Select(p => new
            {
                Employee = p,
                Attendance = p.Atten.Where(c => c.IsCheck == false)
            }).ToList().Select(p => p.Employee).ToList();
            
            return emps;
        }
        public IEnumerable<Employee> GetList()
        {
            var Emps = (from p in db.Employees select p).ToList();
           
            return Emps;
        }

        public int AddEmp(Employee model)
        {
           db.Employees.Add(model);
            int flag = db.SaveChanges();
            return flag;
        }
    }
}