using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class ClassType
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public int DeptID { get; set; }
        /// <summary>
        /// 上班时间
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string OnWorkTime { get; set; }
        /// <summary>
        /// 下班时间
        /// </summary>
        [Required]
        [MaxLength(4)]
        public   string OffWorkTime { get; set; }
        /// <summary>
        /// 中午午休开始时间
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string BeginSleepTime { get; set; }
        /// <summary>
        /// 中午午休结束时间
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string EndSleepTime { get;set;}
        /// <summary>
        /// 加班开始时间
        /// </summary>
        [Required][MaxLength(4)]
        public string WorkEtraTime { get; set; }
        [MaxLength(10)]
        public string CreateUser { get; set; }
        public Nullable<DateTime> createDate;
        public Nullable<DateTime> CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        [MaxLength(10)]
        public string LastUpdateUser { get; set; }
        public Nullable<DateTime> lastUpdateDate;
        public Nullable<DateTime> LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

 
        /// <summary>
        /// 班别种类与【部门-班别】的关系 一对多
        /// </summary>
        public ICollection<Dept_Class> deptClass { get; set; }

        /// <summary>
        /// 班别表与考勤表的关系 一对多
        /// </summary>
        public ICollection<Attendance> Atten{ get; set; }

    }
}