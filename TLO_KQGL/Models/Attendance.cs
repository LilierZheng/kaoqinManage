using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Attendance
    {
        [Key]
        public Guid ID { get; set; }
        public Nullable<DateTime> SignOn { get; set; }
        public Nullable<DateTime> SignOff { get; set; }
       // public int ClassType { get; set; }
        public bool Late { get; set; }
        public bool LeaveEary { get;set;}
        public bool IsCheck { get; set; }
        /// <summary>
        /// 是否请假
        /// </summary>
        [Required]
        public bool IsLeave { get; set; }
        /// <summary>
        /// 请假小时数
        /// </summary>
        public decimal LeaveHours { get; set; }
        [MaxLength(8)]
        public string AttenNo { get; set; }//考勤记录编号 以创建日期为值 如：“19910901”
        /// <summary>
        /// 是否调休 false：否 true：true 默认false--针对休日出勤的（不调休就折算加班费）
        /// </summary>
        public bool isRest { get; set; }
        [MaxLength(36)]
        public string LeaveId { get; set; }//请假id  只有请假的时候使用（审批同意请假后，同步更新考勤表的假别）
        /// <summary>
        /// 加班时间
        /// </summary>
        public decimal WorkOverTime { get; set; }
        /// <summary>
        /// 考勤表与员工的关系 一对一
        /// </summary>
        public Employee Emp { get; set; }
        /// <summary>
        /// 考勤表与班别表的关系 一对一
        /// </summary>
        public ClassType claType { get; set; }
    }
}