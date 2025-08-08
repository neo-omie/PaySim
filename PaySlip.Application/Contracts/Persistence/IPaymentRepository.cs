using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySlip.Domain.Constants;
using PaySlip.Domain.Models;

namespace PaySlip.Application.Contracts.Persistence
{
    public interface IPaymentRepository
    {
        Task<Transaction> CreateDepositAsync(decimal amount, PaymentMethod paymentMethod);
        Task<Transaction> ProcessPaymentAsync(Guid transactionId, string paymentCredentials);
        Task<Transaction> CancelTransactionAsync(Guid transactionId);
        Task<decimal> AddWalletBalanceAsync(decimal amount);

        Task<IEnumerable<Transaction>> GetTransactionHistoryAsync();
        decimal GetWalletBalance();
    }
}
