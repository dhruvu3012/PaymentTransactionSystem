using Azure;
using Newtonsoft.Json;
using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Middleware;
using System.Text;

namespace PaymentTransactionService.Helper
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _headerName;
        private readonly string _secretKey;
        public HttpClientHelper(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _headerName = _configuration.GetSection("SignatureSetting:HeaderName").Value;
            _secretKey = _configuration.GetSection("SignatureSetting:SecretKey").Value;
        }

        public async Task<ResponseObject> CallWebHook(WebhookDto input, string baseUrl)
        {
            var inputString = JsonConvert.SerializeObject(input);
            var generateSignature = Constant.ComputeHmacSha256(inputString, _secretKey);
            string url = baseUrl + "/api/Webhook/UpdateStatus";

            using var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            requestMessage.Headers.Add(_headerName, generateSignature);

            requestMessage.Content = new StringContent(inputString, Encoding.UTF8, "application/json");

            var httpResponse =  await _httpClient.SendAsync(requestMessage);
            string response = await httpResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ResponseObject>(response); 
        }
    }
}
