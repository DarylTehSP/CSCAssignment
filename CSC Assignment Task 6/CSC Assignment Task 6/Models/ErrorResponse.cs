using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSC_Assignment_Task_6.Models
{
    public class ErrorMessage
    {
        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("error")]
        public ErrorMessage ErrorMessage { get; set; }
    }
}


