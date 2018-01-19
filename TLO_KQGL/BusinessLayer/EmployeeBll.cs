using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.DBAccessLayer;
using TLO_KQGL.ViewModels;

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
        /// <summary>
        /// 更新员工考勤审核状态
        /// </summary>
        /// <param name="ids">员工考勤ids</param>
        /// <returns></returns>
        public int UpdateAuditEmp(string ids)
        {
            List<Guid> guids = new List<Guid>();
            string[] _ids = ids.Split(',');
            for (int i = 0; i <_ids.Length ; i++)
            {
                if (!string.IsNullOrEmpty(_ids[i]))
                {
                    guids.Add(Guid.Parse(_ids[i]));
                }
            }
            return dal.UpdateAuditEmp(guids);
        }
        public IEnumerable<AttendanceViewModel> GetAttendanceForExcel(Guid empId)
        {
            return dal.GetAttendanceForExcel(empId);
        }
        /// <summary>
        /// 获取当月请假条
        /// </summary>
        /// <param name="empId">员工id</param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveByDate(string empId)
        {
            return dal.GetLeaveByDate(empId);
        }
    }
}