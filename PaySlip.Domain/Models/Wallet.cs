using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlip.Domain.Models
{
    public class Wallet
    {
        [Key]
        public int Id { get; set; }
        public decimal Balance { get; set; }
    }
}
