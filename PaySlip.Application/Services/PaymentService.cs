using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PaySlip.Application.Contracts.Application;
using PaySlip.Application.Contracts.Persistence;
using PaySlip.Domain.Constants;
using PaySlip.Domain.Models;

namespace PaySlip.Application.Services
{
    public class PaymentService : IPaymentService
    {
        readonly IPaymentRepository _paymentRepository;
        public PaymentService(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<Transaction> CreateDeposit(decimal amount, PaymentMethod paymentMethod)
        {

            var transaction = await _paymentRepository.CreateDepositAsync(amount, paymentMethod);

            await Task.Run(async () =>
            {
                await Task.Delay(5000);
                var callbackUrl = $"https://localhost:7212/api/pay/PaymentCallback/{transaction.TransactionId}";

                using var client = new HttpClient();
                var payload = new { Status = "Success" };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(callbackUrl, content);
            });

            return transaction;
        }

        public async Task<Transaction> ProcessPayment(Guid transactionId, string paymentCredentials)
        {
            return await _paymentRepository.ProcessPaymentAsync(transactionId, paymentCredentials);
        }

        public async Task<Transaction> CancelTransaction(Guid transactionId)
        {
            var cancellingTransaction = await _paymentRepository.CancelTransactionAsync(transactionId);

            // Simulate bank processing and callback after 5 seconds
            await Task.Run(async () =>
            {
                await Task.Delay(5000);
                var callbackUrl = $"https://localhost:7212/api/pay/PaymentCallback/{cancellingTransaction.TransactionId}";

                using var client = new HttpClient();
                var payload = new { Status = "Success" };
                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(callbackUrl, content);
            });
            return cancellingTransaction;
        }

        public async Task<IEnumerable<Transaction>> GetTransactionHistory()
        {
            return await _paymentRepository.GetTransactionHistoryAsync();
        }

        public decimal GetWalletBalance()
        {
            return _paymentRepository.GetWalletBalance();
        }

        public async Task<decimal> AddWalletBalance(decimal amount)
        {
            return await _paymentRepository.AddWalletBalanceAsync(amount);
        }

        public async Task<bool> UpdateTransactionStatus(Guid transactionId, string statusUpdate)
        {
            return await _paymentRepository.UpdateTransactionStatus(transactionId, statusUpdate);
        }
    }
}
