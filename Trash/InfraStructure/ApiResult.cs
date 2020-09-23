using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.InfraStructure
{
    public class ApiResult<T> : ApiResult where T : class
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public T Data { get; set; }
    }

    public class ApiResult
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; } 
        public ApiResultStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
    }
}
