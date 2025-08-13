using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySlip.Application.DTOs;
using PaySlip.Domain.Constants;
using PaySlip.Domain.Models;

namespace PaySlip.Application.Contracts.Application
{
    public interface IPaymentService
    {
        Task<Transaction> CreateDeposit(decimal amount, PaymentMethod paymentMethod);
        Task<Transaction> ProcessPayment(Guid transactionId, string paymentCredentials);
        Task<Transaction> CancelTransaction(Guid transactionId);
        Task<decimal> AddWalletBalance(decimal amount);
        Task<bool> UpdateTransactionStatus(Guid transactionId, string statusUpdate);

        Task<IEnumerable<Transaction>> GetTransactionHistory();
        decimal GetWalletBalance();
    }
}
