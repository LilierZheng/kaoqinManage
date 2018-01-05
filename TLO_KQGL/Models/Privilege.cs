using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Privilege
    {
        [Key]
        public int PrivilegeId { get; set; }
        [MaxLength(10)]
        public string PrivilegeMaster { get; set; } //权限主体 用户或者角色
         [MaxLength(10)]
        public string PrivilegeMasterValue { get; set; }
         [MaxLength(10)]
        public string PrivilegeMasterAccess { get; set; }//权限领域或者模块
          [MaxLength(10)]
        public string PrivilegeMasterAccessValue { get; set; }

        public bool PrivilegeOperation { get; set; }//是否禁止用户的权限
    }
}