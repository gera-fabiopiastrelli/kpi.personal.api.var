using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace kpi.personal.api.functions.ApiClient.ResponseModel
{
    public enum TokenType
    {
        Bearer
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenType TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("user_name")]
        public string Username { get; set; }

        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("expired_password")]
        public bool IsPasswordExpired { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string Message { get; set; }
    }
}
