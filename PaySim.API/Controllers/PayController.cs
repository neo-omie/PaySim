using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaySlip.Application.Contracts.Application;
using PaySlip.Application.DTOs;
using PaySlip.Domain.Constants;
using PaySlip.Domain.Models;

namespace PaySim.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayController : ControllerBase
    {
        readonly IPaymentService _paymentService;
        public PayController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("TransactionHistory")]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactionHistory()
        {
            var history = await _paymentService.GetTransactionHistory();
            return Ok(history);
        }
        [HttpGet("WalletBalance")]
        public ActionResult<decimal> GetWalletBalance()
        {
            var bal =  _paymentService.GetWalletBalance();
            return Ok(bal);
        }

        [HttpPost("CreateDeposit")]
        public async Task<ActionResult<Transaction>> CreateDeposit(decimal amount, PaymentMethod paymentMethod)
        {
            var createDepo = await _paymentService.CreateDeposit(amount, paymentMethod);
            return Ok(createDepo);
        }
        [HttpPost("ProcessPayment/{transactionId}/{paymentCredentials}")]
        public async Task<ActionResult<Transaction>> ProcessPayment(Guid transactionId, string paymentCredentials)
        {
            var processPay = await _paymentService.ProcessPayment(transactionId, paymentCredentials);
            return Ok(processPay);
        }
        [HttpPost("CancelTransaction/{transactionId}")]
        public async Task<ActionResult<Transaction>> CancelTransaction(Guid transactionId)
        {
            var cancelTransact = await _paymentService.CancelTransaction(transactionId);
            return Ok(cancelTransact);
        }

        [HttpPost("AddWalletBalance/{amount}")]
        public async Task<ActionResult<decimal>> AddWalletBalance(decimal amount)
        {
            var addBalance = await _paymentService.AddWalletBalance(amount);
            return Ok(addBalance);
        }

        // This API is for bank payment processing simulation
        [HttpPost("PaymentCallback/{transactionId}")]
        public async Task<ActionResult> PaymentCallback(Guid transactionId, [FromBody] PaymentStatusUpdate statusUpdate)
        {
            var result = await _paymentService.UpdateTransactionStatus(transactionId, statusUpdate.Status);

            if (result)
            {
                return Ok(new { message = "Transaction updated successfully", thumbsUp = "👍" });
            }
            else
            {
                return BadRequest(new { message = "Transaction not found or update failed" });
            }
        }

    }
}
