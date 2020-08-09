using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.Enums
{
    public enum OrderStatus
    {
        [Display(Name ="در انتظار پذیرش راننده")]
        Waiting =1,
        [Display(Name ="تایید شده توسط راننده")]
        Confirmed=2,
        [Display(Name ="راننده در مسیر جمع اوری")]
        InWay=3,
        [Display(Name ="در حال جمع اوری")]
        Taking=4,
        [Display(Name = "اتمام جمع اوری")]
        Done=5
    }
}
