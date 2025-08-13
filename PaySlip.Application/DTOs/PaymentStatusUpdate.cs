using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaySlip.Application.DTOs
{
    public class PaymentStatusUpdate
    {
        public string Status { get; set; }  // e.g. "Success", "Failed"
    }

}
