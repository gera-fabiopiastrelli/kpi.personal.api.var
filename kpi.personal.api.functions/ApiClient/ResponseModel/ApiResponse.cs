using System;
using System.Net;
using Newtonsoft.Json;

namespace kpi.personal.api.functions.ApiClient.ResponseModel
{
    public class ApiResponse<T>
    {
        [JsonProperty("message")]
        public string Error { get; set; }

        public T Model { get; set; }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessStatusCode { get; set; }
    }
}
