using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Leave
    {
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 请假标题
        /// </summary>
        [MaxLength(20)]
        public string Title { get; set; }
        /// <summary>
        /// 请假正文
        /// </summary>
       [MaxLength(300)]
        public string Content { get; set; }

       private Nullable<DateTime> leaveBeginDate;

       public Nullable<DateTime> LeaveBeginDate
       {
           get { return leaveBeginDate; }
           set { leaveBeginDate = value; }
       }
       private Nullable<DateTime> leaveEndDate;

       public Nullable<DateTime> LeaveEndDate
       {
           get { return leaveEndDate; }
           set { leaveEndDate = value; }
       }
        /// <summary>
        /// 请假时长
        /// </summary>
       public decimal Hours { get; set; }
        /// <summary>
        /// 是否审核请假条
        /// </summary>
        public bool IsCheck { get; set; }
        /// <summary>
        /// 是否同意
        /// </summary>
        public bool IsPass { get; set; }
        /// <summary>
        /// 假别种类
        /// </summary>
        [MaxLength(6)]
        public string LeaveType { get; set; }
        [MaxLength(10)]
        public string CreateUser { get; set; }
        private Nullable<DateTime> createDate;

        public Nullable<DateTime> CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }
        [MaxLength(10)]
        public string LastUpdateUser { get; set; }
        private Nullable<DateTime> lastUpdateDate;

        public Nullable<DateTime> LastUpdateDate
        {
            get { return lastUpdateDate; }
            set { lastUpdateDate = value; }
        }

        /// <summary>
        /// 假条与员工的关系 一对一
        /// </summary>
        public Employee emp { get; set; }
    }
}