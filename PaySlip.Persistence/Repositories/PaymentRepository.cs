using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySlip.Application.Contracts.Persistence;
using PaySlip.Domain.Models;
using PaySlip.Domain.Constants;
using PaySlip.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace PaySlip.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        readonly AppDBContext _context;
        static Random _random = new Random();
        public PaymentRepository(AppDBContext context)
        {
            _context = context;
            if(!_context.UserWallet.Any())
            {
                _context.UserWallet.Add(new Wallet { Balance = 1000 });
                _context.SaveChanges();
            }
        }
        public async Task<Transaction> CreateDepositAsync(decimal amount, PaymentMethod paymentMethod)
        {
            var wallet = _context.UserWallet.First();
            if (wallet.Balance < amount)
                throw new Exception($"Insufficient balance to process this much amount. Please enter lesser amount than ₹{wallet.Balance}/-");
            var transaction = new Transaction()
            {
                TransactionId = Guid.NewGuid(),
                Amount = amount,
                PaymentMethod = paymentMethod.ToString(),
                PaymentCredentials = null,
                Status = TransactionStatus.Pending,
                Timestamp = DateTime.Now,
                WalletBalanceAfterTransaction  = wallet.Balance
            };
            await _context.Transactions.AddAsync(transaction);
            if (await _context.SaveChangesAsync() > 0)
                return transaction;
            else
                throw new Exception("For some reason, deposit has not been created.");
        }

        public async Task<Transaction> ProcessPaymentAsync(Guid transactionId, string paymentCredentials)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if (transaction == null)
                throw new Exception($"Transaction with ID {transactionId} not found.");
            bool isSuccess = true;
            transaction.Status = isSuccess ? TransactionStatus.Success : TransactionStatus.Failed;
            if (isSuccess)
            {
                var wallet = await _context.UserWallet.FirstAsync();
                wallet.Balance -= transaction.Amount;
                transaction.WalletBalanceAfterTransaction = wallet.Balance;
                transaction.PaymentCredentials = paymentCredentials;
                if (await _context.SaveChangesAsync() > 0)
                    return transaction;
                else
                    throw new Exception($"For some reason, processing payment is not done.");
            }
            throw new Exception($"Uh-oh! Payment failed :(");
        }

        public async Task<Transaction> CancelTransactionAsync(Guid transactionId)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transactionId);
            if(transaction == null || transaction.Status != TransactionStatus.Success)
                throw new Exception($"Transaction with ID {transactionId} not found.");
            transaction.Status = TransactionStatus.Cancelled;
            var wallet = await _context.UserWallet.FirstAsync();
            wallet.Balance += transaction.Amount;
            transaction.WalletBalanceAfterTransaction = wallet.Balance;
            if (await _context.SaveChangesAsync() > 0)
                return transaction;
            else
                throw new Exception($"For some reason, cancellation of transaction is not done.");
        }

        public async Task<IEnumerable<Transaction>> GetTransactionHistoryAsync()
        {
            var history = await _context.Transactions.OrderByDescending(t => t.Timestamp).ToListAsync();
            return history;
        }

        public decimal GetWalletBalance()
        {
            var bal = _context.UserWallet.First().Balance;
            return bal;
        }

        public async Task<decimal> AddWalletBalanceAsync(decimal amount)
        {
            var wallet = await _context.UserWallet.FirstAsync();
            wallet.Balance += amount;
            if(await _context.SaveChangesAsync() > 0)
                return wallet.Balance;
            throw new Exception($"For some reason, wallet balance has not been added.");
        }
    }
}
