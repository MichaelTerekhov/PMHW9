using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebServiceTestingApp.Dtos
{
    public class ResponseResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("error")]
        public object Error { get; set; }

        [JsonPropertyName("duration")]
        public string Duration { get; set; }

        [JsonPropertyName("primes")]
        public List<int> Primes { get; set; }
        public ResponseResult()
        {
            Primes = new List<int>();
        }
    }
}
