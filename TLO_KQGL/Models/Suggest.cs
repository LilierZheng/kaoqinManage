using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class Suggest
    {
        [Key]
        public Guid ID { get; set; }
        [MaxLength(1000)][Required]
        public string Content { get; set; }
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
    }
}