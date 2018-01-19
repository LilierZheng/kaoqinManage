using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TLO_KQGL.Models;
using System.Data;
using System.Data.Entity;
using System.Data.Objects.SqlClient;
using TLO_KQGL.ViewModels;
using System.Text;
using System.Data.SqlClient;
using System.Data.Linq;

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
        /// <summary>
        /// 获取对应权限下所有待审核的员工信息
        /// </summary>
        /// <param name="deptId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Employee> GetAuditEmp(int deptId,Guid id)
        {
            var emps = db.Employees.Where(p=>p.Dep.ID==deptId).Include(p => p.Atten).Select(p => new
            {
                Employee = p,
                Attendance = p.Atten.Where(c => c.IsCheck == false)
            }).ToList().Select(p => p.Employee).ToList();
            
            return emps;
        }
        /// <summary>
        /// 更新审核员工考勤
        /// </summary>
        /// <param name="ids">员工考勤guids</param>
        /// <returns></returns>
        public int UpdateAuditEmp(List<Guid> guids)
        {
           var attens = db.Attendance.Where(p =>guids.Contains(p.ID)).ToList();
            attens.ForEach(p => p.IsCheck = true);
            int flag = db.SaveChanges();
            return flag;
        }
        public IEnumerable<Employee> GetList()
        {
          // DataContext
            var Emps = (from p in db.Employees select p).ToList();
           
            return Emps;
        }

        public int AddEmp(Employee model)
        {
           db.Employees.Add(model);
            int flag = db.SaveChanges();
            return flag;
        }
        /// <summary>
        /// 获取所有的考勤记录--导出excel用
        /// </summary>
        /// <param name="empId"></param>
        /// <returns></returns>
        public IEnumerable<AttendanceViewModel> GetAttendanceForExcel(Guid empId)
        {
            string attNo = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 4) + DateTime.Now.ToString("yyyy-MM-dd").Substring(5, 2);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"SELECT 
                                               cast(A.[ID] as varchar(50)) as ID
                                              ,CONVERT(varchar(100),A.[SignOn], 120) as SignOn
                                              ,CONVERT(varchar(100),A.[SignOff], 120) as SignOff
                                              ,A.[Late]
                                              ,A.[LeaveEary]
                                              ,A.[IsCheck]
                                              ,A.[Emp_ID]
                                              ,A.[claType_ID]
                                              ,A.[IsLeave]
                                              ,A.[isRest]
                                              ,cast( A.[WorkOverTime] as varchar(50)) AS WorkOverTime
                                               ,A.AttenNo
                                              ,cast(A.[LeaveHours] as varchar(50)) AS LeaveHours
                                              ,cast(B.ID as varchar(50))  AS EmpId
                                              ,B.Emp_No AS EmpNo
                                              ,B.Emp_Name AS EmpName
                                              ,cast(C.ID as varchar(50)) AS DeptId
                                              ,C.DeptName AS DeptName
                                             ,A.LeaveId,d.value as LeaveTypeName
                                     FROM [dbo].[Attendances] A
                                     LEFT JOIN [dbo].Employees B ON A.Emp_ID=B.ID
                                     LEFT JOIN dbo.Departments C ON C.ID=B.Dep_ID
                                     LEFT JOIN dbo.Leaves L ON L.ID=A.LeaveId
                                     LEFT JOIN DBO.Dictionaries d ON d.code=l.LeaveType and d.TypeCode='01'");
            sb.Append(" WHERE A.[Emp_ID]=@empId");
            sb.Append(" AND LEFT(A.AttenNo,6)=@attNo order by A.[AttenNo] asc");
            SqlParameter[] para = new SqlParameter[2];
            para[0] = new SqlParameter("@empId", empId);
            para[1] = new SqlParameter("@attNo", attNo);
            var ret = db.Database.SqlQuery<AttendanceViewModel>(sb.ToString(), para).ToList();
            return ret;

        }

        /// <summary>
        /// 获取当月请假条
        /// </summary>
        /// <param name="empId">员工id</param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveByDate(string empId)
        {
            DateTime dt = DateTime.Now;
            string mon = dt.Month < 10 ? ("0" + dt.Month) : dt.Month.ToString();
            string date = dt.Year.ToString() + mon;
            StringBuilder sb=new StringBuilder (@"SELECT A.Title,
                                    cast(A.ID as varchar(50)) as ID,A.Content,
                                    CONVERT(varchar(100), A.LeaveBeginDate, 120) as leaveBeginDate,
                                    CONVERT(varchar(100), A.LeaveEndDate, 120) as leaveEndDate,
                                    A.CreateUser ,A.CreateDate AS createDate
                                    ,A.LeaveType,D.value as LeaveTypeName,
                                    cast(B.ID as varchar(50))  AS empId,
                                    B.Emp_No AS empNo,B.Emp_Name AS empName,
                                    C.DeptName AS DeptName,A.IsCheck,A.IsPass  
                                  FROM Leaves A LEFT JOIN Employees B ON A.emp_ID=B.ID
                                  LEFT JOIN Departments C ON B.Dep_ID=C.ID
                                  LEFT JOIN Dictionaries D ON D.code=A.LeaveType WHERE 1=1");
            sb.Append(" AND B.ID=@empId and convert(nvarchar(6),A.LeaveBeginDate,112)=@dt ");
            SqlParameter para = new SqlParameter("@dt", date);
            var ret = db.Database.SqlQuery<LeaveViewModel>(sb.ToString(), para).ToList();
            return ret;

        }
    }
}