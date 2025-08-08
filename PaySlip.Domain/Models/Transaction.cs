using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaySlip.Domain.Constants;

namespace PaySlip.Domain.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; }
        public string? PaymentCredentials { get; set; } // Initially null
        public TransactionStatus Status { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal WalletBalanceAfterTransaction { get; set; }
    }
}
