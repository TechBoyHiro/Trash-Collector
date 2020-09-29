using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class OrderRequest
    {
        [Required]
        public long UserId { get; set; }
        public long? UserLocationId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserLocationName { get; set; }
        public List<TrashRequest> Trashes { get; set; }
        public long TotalScore { get; set; }     // How Many Scores Does The Order Has
        [StringLength(500)]
        public string Description { get; set; }
        public DateTime TakenDate { get; set; }
    }
}
