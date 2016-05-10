using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentWriters {
    public class GoogleSpreadsheetTransactionModel {
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string Receiver { get; set; }
        public string Tag { get; set; }
        public string AccountNumber { get; set; }
        public string BankCode { get; set; }

        public IEnumerable<KeyValuePair<string, string>> GetPairs() {
            yield return new KeyValuePair<string, string>(nameof(Id), Id ?? string.Empty);
            yield return new KeyValuePair<string, string>(nameof(CreateDateTime), CreateDateTime.ToString());
            yield return new KeyValuePair<string, string>(nameof(Amount), Amount.ToString());
            yield return new KeyValuePair<string, string>(nameof(Type), Type ?? string.Empty);
            yield return new KeyValuePair<string, string>(nameof(Text), Text ?? string.Empty);
            yield return new KeyValuePair<string, string>(nameof(Receiver), Receiver ?? string.Empty);
            yield return new KeyValuePair<string, string>(nameof(Tag), !string.IsNullOrEmpty(Tag) ? Tag : "Other");
            yield return new KeyValuePair<string, string>(nameof(AccountNumber), AccountNumber ?? string.Empty);
            yield return new KeyValuePair<string, string>(nameof(BankCode), BankCode ?? string.Empty);

            yield return new KeyValuePair<string, string>("YearMonthTag", GetYearMonthTag());
            yield return new KeyValuePair<string, string>("AmountSignTag", GetAmountSignTag());

        }

        string GetMonthRepresentation(DateTime month) {
            return string.Format("{0:00}/{1}", month.Month, month.Year);
        }

        DateTime GetNextMonth(DateTime month) {
            return new DateTime(
                month.Month < 12 ? month.Year : month.Year + 1,
                month.Month < 12 ? month.Month + 1 : 1,
                1
            );
        }

        DateTime GetTimeStamp(DateTime dateTime) {
            if (dateTime.Day < 22) {
                return new DateTime(dateTime.Year, dateTime.Month, 1);
            } else {
                return GetNextMonth(dateTime);
            }
        }

        string GetYearMonthTag() {
            return GetMonthRepresentation(GetTimeStamp(CreateDateTime));
        }

        string GetAmountSignTag() {
            return Amount > 0 ? @"=""++""" : "--";
        }

    }
}
