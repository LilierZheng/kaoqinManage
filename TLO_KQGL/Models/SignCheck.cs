using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TLO_KQGL.Models
{
    public class SignCheck
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Guid CheckID { get; set; }//审核人ID
        public string CheckedIDs { get; set; }//被审核人
    }
}