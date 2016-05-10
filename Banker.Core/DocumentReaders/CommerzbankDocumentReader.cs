using CsvHelper;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentReaders {
    public class CommerzbankDocumentReader : FileDocumentReader {

        string SkipLine(string s) {
            return s.Substring(s.IndexOf(Environment.NewLine) + Environment.NewLine.Length);
        }

        public IEnumerable<string[]> GetRows(string text) {
            using (var stringReader = new StringReader(text)) {
                var csv = new CsvParser(stringReader);
                csv.Configuration.Delimiter = ";";
                for (;;) {
                    var row = csv.Read();
                    if (row == null) {
                        break;
                    }
                    yield return row;
                }
            }
        }

        public Transaction MapRowToTransaction(string[] row) {
            return new Transaction {
                Amount = Convert.ToDecimal(row[4]),
                CreateDateTime = Convert.ToDateTime(row[0]),    
                Text = row[3],
                Type = row[2],
            };
        }

        protected override Document ReadString(string s) {
            return new Document() {
                Transactions = GetRows(SkipLine(s)).Select(MapRowToTransaction).ToArray()
            };
        }

    }


}
