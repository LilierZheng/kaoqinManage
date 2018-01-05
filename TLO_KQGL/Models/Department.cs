using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Department
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public int ParentId { get; set; }
        [MaxLength(5)]
        [Required]
        public string DeptNo { get; set; }
        [MaxLength(20)]
        [Required]
        public string DeptName { get; set; }
 
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
        public decimal lontitude { get; set; }
        public decimal latitude{get;set;}
        /// <summary>
        /// 部门与员工的关系 一对多
        /// </summary>
        public ICollection<Employee> Emp { get; set; }
        /// <summary>
        /// 部门与【部门-班别】的关系 一对多
        /// </summary>
        public ICollection<Dept_Class> Deptclass { get; set; }

    }
}