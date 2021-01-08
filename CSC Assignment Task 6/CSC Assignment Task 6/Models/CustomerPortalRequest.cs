using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSC_Assignment_Task_6.Models
{
    public class CustomerPortalRequest
    {
        [JsonProperty("sessionId")]
        public string SessionId { get; set; }
    }
}

