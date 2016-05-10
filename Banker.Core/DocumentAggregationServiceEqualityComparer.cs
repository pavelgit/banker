using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core {
    public class DocumentAggregationServiceEqualityComparer : IEqualityComparer<Transaction> {
        public bool Equals(Transaction x, Transaction y) {
            return
                x.Amount == y.Amount &&
                x.CreateDateTime == y.CreateDateTime &&
                x.Id == y.Id &&
                x.Receiver == y.Receiver &&
                x.Text == y.Text &&
                x.Type == y.Type;
        }

        public int GetHashCode(Transaction obj) {
            unchecked {
                int hash = 17;
                hash = hash * 23 + obj.Amount.GetHashCode();
                hash = hash * 23 + obj.CreateDateTime.GetHashCode();
                if (obj.Id != null) {
                    hash = hash * 23 + obj.Id.GetHashCode();
                }
                hash = hash * 23 + (obj.Receiver != null ? obj.Receiver.GetHashCode() : 0);
                hash = hash * 23 + (obj.Text != null ? obj.Text.GetHashCode() : 0);
                hash = hash * 23 + (obj.Type != null ? obj.Type.GetHashCode() : 0);
                return hash;
            }
        }

    }

}
