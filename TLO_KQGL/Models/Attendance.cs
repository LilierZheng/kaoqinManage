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
        [Required]
        public DateTime SignOn { get; set; }
        [Required]
        public DateTime SignOff { get; set; }
       // public int ClassType { get; set; }
        public bool Late { get; set; }
        public bool LeaveEary { get;set;}

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