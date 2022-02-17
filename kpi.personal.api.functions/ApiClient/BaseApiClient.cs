using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

using kpi.personal.api.functions.ApiClient.Handlers;
using kpi.personal.api.functions.ApiClient.ResponseModel;

namespace kpi.personal.api.functions.ApiClient
{
    public class BaseApiClient
    {
        protected static HttpClient HttpClient;

        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);

        private static Token Token = null;

        private static string BearerToken
        {
            get { return (IsAuthenticated) ? Token.Value : null; }
        }

        private static bool IsAuthenticated
        {
            get { return ((Token != null) && (Token.ExpiresAtUtc > DateTime.UtcNow)); }
        }

        private static async Task Authenticate()
        {
            var result = await TokenApi.PostTokenAsync();
            if ((result != null) && (result.AccessToken != null))
            {
                Token = new Token
                {
                    Value = result.AccessToken,
                    ExpiresAtUtc = DateTime.UtcNow.AddSeconds(result.ExpiresIn - 60)
                };
            }
        }

        static BaseApiClient()
        {
            // get HTTP client implementation, and adds a decompression handler
            HttpClient = HttpClientFactory.Create(new DecompressionHandler());

            // set base address for HTTP requests
            HttpClient.BaseAddress = new Uri(Environment.GetEnvironmentVariable("ApiBaseAddress"));

            // add request header Accept
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // add request header Accept-Language
            HttpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("pt-BR"));

            // add request header Accept-Encoding
            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            HttpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
        }

        protected static async Task<ApiResponse<T>> GetAsync<T>(string requestUri, bool tolerarErro = false)
        {
            ApiResponse<T> baseResponseModel = new ApiResponse<T>();

            baseResponseModel.Model = default(T);

            if (!IsAuthenticated)
            {
                await Semaphore.WaitAsync();
                try
                {
                    if (!IsAuthenticated)
                    {
                        // autenticar
                        await Authenticate();
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            HttpResponseMessage response = await HttpClient.GetAsync(requestUri);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    baseResponseModel.Model = await response.Content.ReadAsAsync<T>();
                    break;
                case HttpStatusCode.BadRequest:
                    baseResponseModel = await response.Content.ReadAsAsync<ApiResponse<T>>();
                    break;
            }

            if (!tolerarErro && !response.IsSuccessStatusCode)
            {
                throw new ApplicationException(string.Format("GetAsync - Uri: {0}, StatusCode: {1}, Error: {2}", requestUri, response.StatusCode, baseResponseModel.Error));
            }

            baseResponseModel.StatusCode = response.StatusCode;
            baseResponseModel.IsSuccessStatusCode = response.IsSuccessStatusCode;

            return baseResponseModel;
        }

        protected static async Task<ApiResponse<T1>> PostAsync<T1, T2>(string requestUri, T2 value, bool tolerarErro = false)
        {
            ApiResponse<T1> baseResponseModel = new ApiResponse<T1>();

            baseResponseModel.Model = default(T1);

            string json = JsonConvert.SerializeObject(value);

            if (!IsAuthenticated)
            {
                await Semaphore.WaitAsync();
                try
                {
                    if (!IsAuthenticated)
                    {
                        // autenticar
                        await Authenticate();
                    }
                }
                finally
                {
                    Semaphore.Release();
                }
            }

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);

            HttpResponseMessage response = await HttpClient.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.NoContent:
                    baseResponseModel.Model = await response.Content.ReadAsAsync<T1>();
                    break;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                    List<ApiResponse<T1>> baseResponseModelList = await response.Content.ReadAsAsync<List<ApiResponse<T1>>>();
                    baseResponseModel = baseResponseModelList[0];
                    break;
            }

            if (!tolerarErro && !response.IsSuccessStatusCode)
            {
                throw new ApplicationException(string.Format("PostAsync - Uri: {0}, StatusCode: {1}, Error: {2}", requestUri, response.StatusCode, baseResponseModel.Error));
            }

            baseResponseModel.StatusCode = response.StatusCode;
            baseResponseModel.IsSuccessStatusCode = response.IsSuccessStatusCode;

            return baseResponseModel;
        }
    }
}
