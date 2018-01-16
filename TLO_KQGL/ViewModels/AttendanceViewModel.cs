using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLO_KQGL.ViewModels
{
    public class AttendanceViewModel
    {
        public Guid ID { get; set; } 
        public DateTime SignOn { get; set; }
        public DateTime SignOff { get; set; }
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
        public decimal  WorkOverTime { get; set; }

    }
}