﻿using System.Net;
using System.Text.Json.Serialization;

namespace ATechnologiesAssignment.App.Models
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDictionary<string, string[]>? Errors { get; set; }

        public bool Success => (int)StatusCode >= 200 && (int)StatusCode < 300;
    }
}
