using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class TrashReport
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long OrderId { get; set; }
        public CustomeDateTime SubmitDate { get;set; }
        public CustomeDateTime? TakenDate { get; set; }
        public string TrashTypeName { get; set; }
        public long Weight { get; set; }    
        public int? Score { get; set; }     
    }
}
