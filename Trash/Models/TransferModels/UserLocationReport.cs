using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class UserLocationReport
    {
        public long? Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
    }
}
