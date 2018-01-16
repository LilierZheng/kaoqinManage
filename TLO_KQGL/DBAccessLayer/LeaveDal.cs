using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using TLO_KQGL.Models;
using TLO_KQGL.ViewModels;
using System.Transactions;

namespace TLO_KQGL.DBAccessLayer
{
    public class LeaveDal : DalBase
    {
        /// <summary>
        /// 获取请假条
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeave()
        {
            string sql = @"SELECT A.Title,cast(A.ID as varchar(50)) as ID,A.Content,CONVERT(varchar(100), A.LeaveBeginDate, 120) as leaveBeginDate,
CONVERT(varchar(100), A.LeaveEndDate, 120) as leaveEndDate,A.CreateUser ,A.CreateDate AS createDate
,cast(B.ID as varchar(50))  AS empId,B.Emp_No AS empNo,B.Emp_Name AS empName,C.DeptName AS DeptName,A.IsCheck,A.IsPass  FROM Leaves A LEFT JOIN Employees B ON A.emp_ID=B.ID
LEFT JOIN Departments C ON B.Dep_ID=C.ID";
            var ret = db.Database.SqlQuery<LeaveViewModel>(sql).ToList();
            return ret;
        }
        /// <summary>
        /// 根据条件获取假条
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="isCheck">是否审核</param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveBy(string beginDate, string endDate, bool isCheck)
        {
            StringBuilder sb = new StringBuilder(@"SELECT A.Title,cast(A.ID as varchar(50)) as ID,A.Content,CONVERT(varchar(100), A.LeaveBeginDate, 120) as leaveBeginDate,
CONVERT(varchar(100), A.LeaveEndDate, 120) as leaveEndDate,A.CreateUser ,A.CreateDate AS createDate
,cast(B.ID as varchar(50))  AS empId,B.Emp_No AS empNo,B.Emp_Name AS empName,C.DeptName AS DeptName,A.IsCheck ,A.IsPass
FROM Leaves A LEFT JOIN Employees B ON A.emp_ID=B.ID
LEFT JOIN Departments C ON B.Dep_ID=C.ID where 1=1");
            if (!string.IsNullOrEmpty(beginDate))
            {
                sb.Append(" A.CreateDate>=@beginDate ");
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                sb.Append(" and A.CreateDate<=@endDate");
            }
            sb.Append(" and A.isCheck=@isCheck");

            SqlParameter[] para = new SqlParameter[3];
            para[0] = new SqlParameter("@beginDate", beginDate);
            para[1] = new SqlParameter("@endDate", endDate);
            para[2] = new SqlParameter("@isCheck", isCheck);
            var ret = db.Database.SqlQuery<LeaveViewModel>(sb.ToString(),para).ToList();
            return ret;
        }
        /// <summary>
        /// 根据员工id获取假条
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<LeaveViewModel> GetLeaveByEmpId(Guid id)
        {
            StringBuilder sb = new StringBuilder(@"SELECT A.Title,cast(A.ID as varchar(50)) as ID,A.Content,CONVERT(varchar(100), A.LeaveBeginDate, 120) as leaveBeginDate,
CONVERT(varchar(100), A.LeaveEndDate, 120) as leaveEndDate,A.CreateUser ,A.CreateDate AS createDate
,cast(B.ID as varchar(50))  AS empId,B.Emp_No AS empNo,B.Emp_Name AS empName,C.DeptName AS DeptName,A.IsCheck,A.IsPass
FROM Leaves A LEFT JOIN Employees B ON A.emp_ID=B.ID
LEFT JOIN Departments C ON B.Dep_ID=C.ID where 1=1");
            if (id != null)
            {
                sb.Append(" AND B.ID=@empId");
            }
            SqlParameter para = new SqlParameter("@empId", id);
            var ret = db.Database.SqlQuery<LeaveViewModel>(sb.ToString(),para).ToList();
            return ret;

        }

        /// <summary>
        /// 审核指定假条
        /// </summary>
        /// <param name="id">假条id</param>
        /// <param name="IsPass">是否同意</param>
        /// <returns></returns>
        public int AuditLeave(LeaveViewModel lea,int days)
        {
            int flag = 0;
            StringBuilder sb = new StringBuilder(@"UPDATE Leaves SET IsCheck=1,IsPass=@IsPass WHERE ID=@id;");
            if (lea.IsPass == true)
            {
                GetUpdateAtten(lea, days,sb);
            }
            SqlParameter[] para = new SqlParameter[6];
            para[0] = new SqlParameter("@IsPass", lea.IsPass);
            para[1] = new SqlParameter("@id", Guid.Parse(lea.ID));
            para[2] = new SqlParameter("@empId", lea.empId);
            para[3] = new SqlParameter("@claTypeId", lea.ClassId);
            para[4] = new SqlParameter("@FirBeginDate", lea.leaveBeginDate);

            para[5] = new SqlParameter("@FirEndDate", lea.leaveEndDate);

            try
            {
                using (var tran=new TransactionScope())
                {
                    flag = db.Database.ExecuteSqlCommand(sb.ToString(), para);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return flag;
        }
        /// <summary>
        /// 获取更新考勤记录sql
        /// </summary>
        /// <param name="lea"></param>
        /// <param name="days"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        public StringBuilder GetUpdateAtten(LeaveViewModel lea,int days,StringBuilder sb)
        {
            DateTime dtFirstOnWork = DateTime.Parse(lea.leaveBeginDate.Substring(0, 10) + " " + lea.OnWorkTime.Substring(0, 2) + ":" + lea.OnWorkTime.Substring(2, 2));
            DateTime dtFirstOffWork = DateTime.Parse(lea.leaveBeginDate.Substring(0, 10) + " " + lea.OffWorkTime.Substring(0, 2) + ":" + lea.OffWorkTime.Substring(2, 2));
            DateTime dtEndOnWork = DateTime.Parse(lea.leaveEndDate.Substring(0, 10) + " " + lea.OnWorkTime.Substring(0, 2) + ":" + lea.OnWorkTime.Substring(2, 2));
            DateTime dtEndOffWork = DateTime.Parse(lea.leaveEndDate.Substring(0, 10) + " " + lea.OffWorkTime.Substring(0, 2) + ":" + lea.OffWorkTime.Substring(2, 2));

            if (days == 0) //请假未跨天
            {
                if (lea.leaveBeginDate == dtFirstOnWork.ToString() && lea.leaveEndDate == dtFirstOffWork.ToString())
                {
                    //创建当天的考勤记录
                    sb.Append("INSERT INTO Attendances VALUES(");
                    sb.Append("NEWID(),null,null,0,0,0, @empId,@claTypeId,1,null,0,null);");
                }
                else
                {
                    //更改当天的考勤记录中的是否请假为是
                    sb.Append("UPDATE Attendances SET IsLeave=1 WHERE SignOn<@FirBeginDate and SignOff>@FirEndDate;");

                }
            }
            else//跨天
            {
                for (int i = 0; i < days; i++)
                {
                    DateTime dtTemp = DateTime.Parse(lea.leaveBeginDate).AddDays(i);
                    DateTime tmpOnWork = DateTime.Parse(dtTemp.ToString("yyyy-MM-dd") + " " + lea.OnWorkTime.Substring(0, 2) + ":" + lea.OnWorkTime.Substring(2, 2));
                    DateTime tmpOffWork = DateTime.Parse(dtTemp.ToString("yyyy-MM-dd") + " " + lea.OffWorkTime.Substring(0, 2) + ":" + lea.OffWorkTime.Substring(2, 2));
                    if (dtTemp == tmpOnWork)
                    {
                        //请了一天
                        //创建考勤记录
                        sb.Append("INSERT INTO Attendances VALUES(");
                        sb.Append("NEWID(),null,null,0,0,0, @empId,@claTypeId,1,null,0,null);");
                    }
                    if (dtTemp > tmpOnWork)
                    {
                        //签到时间之后开始请假
                        sb.Append("UPDATE Attendances SET IsLeave=1 WHERE SignOn<'" + dtTemp + "' and SignOff<'" + tmpOffWork + "';");
                    }
                }
            }
            return sb;
        }
    }
}