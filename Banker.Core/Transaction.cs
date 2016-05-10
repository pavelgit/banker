using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core {
    public class Transaction {
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Receiver { get; set; }
        public string Tag { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }
    }
}
