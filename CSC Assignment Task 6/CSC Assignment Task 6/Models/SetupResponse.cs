using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSC_Assignment_Task_6.Models
{
    public class SetupResponse
    {
        [JsonProperty("publishableKey")]
        public string PublishableKey { get; set; }

        [JsonProperty("proPrice")]
        public string ProPrice { get; set; }

        [JsonProperty("basicPrice")]
        public string BasicPrice { get; set; }
    }

}
