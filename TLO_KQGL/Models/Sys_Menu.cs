using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Sys_Menu
    {
        [Key]
        public int MenuId { get; set; }
        [MaxLength(5)]
        public string MenuNo { get; set; }
         [MaxLength(20)]
        public string ApplicationCode { get; set; }
        [MaxLength(5)]
        public string MenuParentNo { get;set;}
        [MaxLength(5)]
        public string MenuOrder { get; set; }
           [MaxLength(20)]
        public string MenuName { get; set; }
        [MaxLength(50)]
        public string MenuUrl { get; set; }
           [MaxLength(10)]
        public string MenuIcon { get; set; }
        public bool IsVisible { get; set; }
        public bool IsLeaf { get; set; }
    }
}