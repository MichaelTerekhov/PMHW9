using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace WebServiceTestingApp.Models
{
    public class CustomUri
    {
        [JsonPropertyName("uri")]
        public string Uri { get; set; }
    }
}
