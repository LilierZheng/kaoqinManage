using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        [Key]
        public int ID { get; set; }
        [MaxLength(5)]
        public string userNo { get; set; }
        [MaxLength(5)]
        public string userName { get; set; }
        [MaxLength(20)]
        public string password { get; set; }

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
        public ICollection<Role> role { get; set; }
    }
}