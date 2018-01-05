using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [MaxLength(20)]
        public string RoleName { get; set; }
          [MaxLength(50)]
        public string RoleDesc { get; set; }
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
        public ICollection<User> user { get; set; }
    }
}