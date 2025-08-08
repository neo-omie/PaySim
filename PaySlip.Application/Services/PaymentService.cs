using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return await _paymentRepository.CreateDepositAsync(amount, paymentMethod);
        }

        public async Task<Transaction> ProcessPayment(Guid transactionId, string paymentCredentials)
        {
            return await _paymentRepository.ProcessPaymentAsync(transactionId, paymentCredentials);
        }

        public async Task<Transaction> CancelTransaction(Guid transactionId)
        {
            return await _paymentRepository.CancelTransactionAsync(transactionId);
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
    }
}
