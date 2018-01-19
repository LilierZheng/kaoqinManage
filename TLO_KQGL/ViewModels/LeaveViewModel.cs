using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLO_KQGL.ViewModels
{
    public class LeaveViewModel
    {
        public  string ID { get; set; }
        public string Title{get;set;}
        public string Content { get; set; }
        public string leaveBeginDate { get; set; }
        public string leaveEndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveTypeName { get; set; }
        public string CreateUser { get; set; }
        public DateTime createDate { get; set; }
        public string empId { get; set; }
        public string empNo { get; set; }
        public string empName { get; set; }
        public string DeptName { get; set; }
        public bool IsPass { get; set; }
        public bool IsCheck { get; set; }//是否审核
        public string OnWorkTime { get; set; }//上班时间
        public string OffWorkTime { get; set; }//下班时间
        public string BeginSleepTime { get; set; }//午休开始时间
        public string EndSleepTime { get; set; }//午休结束时间

        public string ClassId { get; set; }
    }
}