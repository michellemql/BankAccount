using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAccount.Models
{
    public class TransactionViewModel
    {
        public List<Transactions> Transactions { get; set; } = new List<Transactions>();
        public Transactions Transaction { get; set; }
    }
}