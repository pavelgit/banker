using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentWriters {
    public class DefaultDocumentWriter : IDocumentWriter {
        public string FilePath { get; set; }

        public string GetOutputString(Document document) {
            var sb = new StringBuilder();
            foreach (var transaction in
                document.Transactions.OrderByDescending(transaction => transaction.CreateDateTime)
            ) {
                sb.AppendLine(GetTransactionText(transaction));
            }
            return sb.ToString();
        }

        string GetTransactionText(Transaction transaction) {
            return string.Format(
                "{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}",
                transaction.Id,
                transaction.CreateDateTime,
                transaction.Amount,
                transaction.Type,
                transaction.Tag,
                transaction.Text,
                transaction.Receiver
            );
        }

        public void WriteDocument(Document document) {
            File.WriteAllText(FilePath, GetOutputString(document));
        }
    }
}
