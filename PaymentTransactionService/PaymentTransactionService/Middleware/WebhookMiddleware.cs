using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Helper;
using System.Security.Cryptography;
using System.Text;

namespace PaymentTransactionService.Middleware
{
    public class WebhookMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        string _headerName;
        string _secretKey;

        public WebhookMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
            _headerName = _configuration.GetSection("SignatureSetting:HeaderName").Value;
            _secretKey = _configuration.GetSection("SignatureSetting:SecretKey").Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/Webhook") && !context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
            {


                context.Request.EnableBuffering();
                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
                var rawRequestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (!context.Request.Headers.TryGetValue(_headerName, out var receivedSignatureHeader) || receivedSignatureHeader.Count == 0)
                {
                    context.Response.StatusCode = 403; // Forbidden if header is missing
                    await context.Response.WriteAsync("Missing signature header.");
                    return;
                }
                var receivedSignature = receivedSignatureHeader[0];

                var computedSignature = Constant.ComputeHmacSha256(rawRequestBody, _secretKey);

                if (!CryptographicOperations.FixedTimeEquals(Encoding.UTF8.GetBytes(computedSignature), Encoding.UTF8.GetBytes(receivedSignature)))
                {
                    context.Response.StatusCode = 403; // Forbidden if signatures don't match
                    await context.Response.WriteAsync("Invalid signature.");
                    return;
                }
            }
            var originalBodyStream = context.Response.Body;
            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;
                string jsonResponse = string.Empty;
                try
                {
                    await _next(context);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();
                    var isValidJson = Constant.IsValidJson(responseBody);
                    context.Response.Body = originalBodyStream;
                    if (context.Response.StatusCode == 200)
                    {
                        context.Response.ContentType = "application/json";
                        var response = ResponseObject.Success(isValidJson ? responseBody : null, context.Response.StatusCode, !isValidJson ? responseBody : "");
                        jsonResponse = JsonConvert.SerializeObject(response);
                    }
                    else
                    {
                        context.Response.ContentType = "application/json";
                        var response = ResponseObject.Error(context.Response.StatusCode, responseBody);
                        jsonResponse = JsonConvert.SerializeObject(response);
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Body = originalBodyStream;
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var errorResponse = ResponseObject.Error(500, ex.Message);
                    jsonResponse = JsonConvert.SerializeObject(errorResponse);
                }
                finally
                {
                    context.Response.Body = originalBodyStream;

                    await context.Response.WriteAsync(jsonResponse);
                }
            }
        }

    }
}
