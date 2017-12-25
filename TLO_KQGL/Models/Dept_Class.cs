using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Dept_Class
    {
       [Key] 
        public int ID { get; set; }

        /// <summary>
        /// 【部门-班别】与班别的关系 一对一
        /// </summary>
        /// 
        public ClassType classType { get; set; }
        /// <summary>
        /// 【部门-班别】与部门的关系 一对一
        /// </summary>
        public Department dep { get; set; }
    }
}