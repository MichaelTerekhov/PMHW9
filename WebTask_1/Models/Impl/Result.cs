using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebTask_1.Models.Impl
{
     public  class Result
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public object Error { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("primes")]
        public List<int> Primes { get; set; }
        public Result()
        {
            Primes = new List<int>();
        }
    }
}
