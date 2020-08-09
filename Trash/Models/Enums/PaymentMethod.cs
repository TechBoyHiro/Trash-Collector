using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.Enums
{
    public enum PaymentMethod
    {
        [Display(Name ="هیچکدام")]
        None =1,
        [Display(Name= "استفاده از خدمات غیر حضوری")]
        UseService =2,
        [Display(Name ="استفاده از کالا")]
        UseCommodity =3
    }
}
