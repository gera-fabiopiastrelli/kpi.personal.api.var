using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using kpi.personal.api.functions.ApiClient.ResponseModel;

namespace kpi.personal.api.functions.ApiClient
{
    public class TokenApi : BaseApiClient
    {
        private TokenApi()
        {
        }

        public static async Task<TokenResponse> PostTokenAsync()
        {
            TokenResponse tokenResponse = null;
            Dictionary<string, string> tokenRequest = new Dictionary<string, string>
            {
                { "client_id", Environment.GetEnvironmentVariable("AppClientId") },
                { "client_secret", Environment.GetEnvironmentVariable("AppClientSecret") },
                { "username", Environment.GetEnvironmentVariable("ApiUser") },
                { "password", Environment.GetEnvironmentVariable("ApiPassword") },
                { "grant_type", "password" }
            };
            HttpResponseMessage response = await HttpClient.PostAsync("token", new FormUrlEncodedContent(tokenRequest));
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.BadRequest:
                    tokenResponse = await response.Content.ReadAsAsync<TokenResponse>();
                    break;
            }
            return tokenResponse;
        }
    }
}
