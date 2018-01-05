using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Sys_Button
    {
        [Key]
        public int BtnId { get; set; }
        [MaxLength(5)]
        public string BtnNo { get; set; }
  
        [MaxLength(10)]
        public string BtnClass { get; set; }
        [MaxLength(20)]
        public string BtnScript { get; set; }
        [MaxLength(20)]
        public string BtnName { get; set; }
        [MaxLength(5)]
        public string MenuNo { get; set; }
        [MaxLength(10)]
        public string BtnIcon { get; set; }
        [MaxLength(5)]
        public string InitStatus { get; set; }
        [MaxLength(5)]
        public string seqNo { get; set; }
    }
}