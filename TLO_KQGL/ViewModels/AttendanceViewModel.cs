using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLO_KQGL.ViewModels
{
    public class AttendanceViewModel
    {
        public string ID { get; set; } 
        public string SignOn { get; set; }
        public string SignOff { get; set; }
        public bool Late { get; set; }
        public bool LeaveEary { get; set; }
        public bool IsCheck { get; set; }
        /// <summary>
        /// 是否请假
        /// </summary>
        public bool IsLeave { get; set; }
        /// <summary>
        /// 是否工作
        /// </summary>
        public bool IsWork { get; set; }
        public bool IsRest { get; set; }
        public string  WorkOverTime { get; set; }
        public string EmpId { get; set; }//员工id
        public string EmpNo { get; set; }//员工编号
        public string EmpName { get; set; }//员工姓名
        public string DeptId { get;set;}//部门id
        public string LeaveId { get; set; }
        public string LeaveTypeName { get; set; }
        public string LeaveHours { get; set; }
        public string DeptNO { get; set; }//部门编号
        public string DeptName { get; set; }//部门名称


    }
}