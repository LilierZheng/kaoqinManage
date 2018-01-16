﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class calendar
    {
        [Key]
        public int ID { get; set; }
        [MaxLength(4)]
        public string Yeat { get; set; }
        [MaxLength(2)]
        public string Month { get; set; }
        [MaxLength(10)]
        public string Date { get; set; }
        public bool IsWork { get; set; }//是否出勤
        public bool IsVacation { get; set; }//是否节假日

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
 
    }
}