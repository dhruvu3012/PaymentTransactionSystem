using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentTransactionService.Business.IServices;
using PaymentTransactionService.Business.Models;
using PaymentTransactionService.Data.Enums;

namespace PaymentTransactionService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhookController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public WebhookController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> PaymentCallback([FromBody] WebhookDto input)
        {
            bool isVerify = await _transactionService.VerifyPayment(input.ProviderReference);
            if (!isVerify)
                return NotFound("Transaction not found!");
            bool isUpdated = await _transactionService.Payment(input);
            if(isUpdated)
            {
                return Ok($"Transaction has been updated to {((PaymentStatus)input.Status).ToString()}!");
            }
            return BadRequest("Something went wrong!");
        }
    }
}
