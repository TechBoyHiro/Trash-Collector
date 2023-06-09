﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class TrashRequest
    {
        [Required]
        public long TrashTypeId { get; set; }
        [Required]
        public long Weight { get; set; }    // In Gram
        public int Score { get; set; }
    }
}
