using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Dictionary
    {
        [Key]
        public int ID { get; set; }
        /// <summary>
        /// 字典类编号
        /// </summary>
        [MaxLength(6)][Required]
        public string code { get; set; }
        /// <summary>
        /// 字典分类编号
        /// </summary>
        [MaxLength(2)]
        [Required]
        public string TypeCode { get; set; }
        /// <summary>
        /// 字典内容
        /// </summary>
        [MaxLength(20)]
        [Required]
        public string value { get; set; }

        /// <summary>
        /// 字典分类标注 是否有效
        /// </summary>
        [Required]
        public bool status { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public string desc { get; set; }

        /// <summary>
        /// 删除标志
        /// </summary>
        public bool IsDel { get; set; }
    }
}