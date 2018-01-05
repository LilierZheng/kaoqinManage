using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Sys_Application
    {
        [Key]
        public int ApplicationId { get; set; }
        [MaxLength(20)]
        public string ApplicationCode { get; set; }
        [MaxLength(20)]
        public string ApplicationName { get; set; }
         [MaxLength(50)]
        public string ApplicationDesc { get; set; }
        public bool ShowInMenu { get; set; }
    }
}