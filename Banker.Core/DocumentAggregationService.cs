using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core {
    public class DocumentAggregationService {

        IEqualityComparer<Transaction> EqualityComparer = 
            new DocumentAggregationServiceEqualityComparer();

        public Document Aggregate(IEnumerable<Document> documents) {
            return new Document() {
                Transactions = documents.SelectMany(v => v.Transactions)
                    .Distinct(EqualityComparer).ToArray()
            };
        }

    }
}
