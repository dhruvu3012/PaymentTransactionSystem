using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PaymentTransactionService.Business.IServices;
using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Helper;

namespace PaymentTransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly HttpClientHelper _httpClientHelper;
        public TransactionController(ITransactionService transactionService,HttpClientHelper httpClientHelper)
        {
            _transactionService = transactionService;
            _httpClientHelper = httpClientHelper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAll();
            return Ok(result); 
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment(CreatePaymentDto input)
        {
            if (input == null ||  input.Amount <= 0)
               return BadRequest("Please enter valid amount!");
            CreatePaymentResponseDto result = await _transactionService.CreatePayment(input);
            return Ok("Transaction Created Successfully!");
        }

        [HttpPost("Process")]
        public async Task<IActionResult> PaymentProcess(WebhookDto  input)
        {
            var request = HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}";
            var result = await _httpClientHelper.CallWebHook(input, baseUrl);
            return StatusCode(result.statusCode, result.message);
        }
    }
}
