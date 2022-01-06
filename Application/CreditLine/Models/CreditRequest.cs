using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CreditLine.Models
{
    public class CreditRequest
    {
        public String FoundingType { get; set; }
        public double CashBalance { get; set; }
        public double MonthlyRevenue { get; set; }
        public double RequestedCreditLine { get; set; }
        public DateTime RequestedDate { get; set; }

    }
}
