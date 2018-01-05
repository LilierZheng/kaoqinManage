using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace TLO_KQGL.Models
{
    public class Employee
    {
        [Key]
        public Guid ID { get; set; }
        [MaxLength(5)]
        [Required]
        public string Emp_No { get; set; }
        [MaxLength(20)]
        [Required]
        public string Emp_Name { get; set; }

        public bool Emp_Sex { get; set; }
         [MaxLength(50)]
        public string Address { get; set; }
           [MaxLength(20)]
        public string NativePlace { get; set; }
        public int Age { get; set; }
           [MaxLength(20)]
        public string BirthDay { get; set; }
           [MaxLength(20)]
        public string Job { get; set; }
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
        [Required]
        [MaxLength(11)]
        public string Tel { get; set; }
        /// <summary>
        /// 员工与部门的关系一对一
        /// </summary>
        public Department Dep { get; set; }
        /// <summary>
        /// 员工与考勤表的关系 一对多
        /// </summary>
        public ICollection<Attendance> Atten { get; set; }
    }
}