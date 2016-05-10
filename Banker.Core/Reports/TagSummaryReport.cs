using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Reports {

    public class MonthlyTagSummaryReport {
        
        public MonthlyTagSums[] Sums { get; private set; }
        public string EmptyTag = "Other";

        public void Load(Document document) {
            Sums = document.Transactions
                .GroupBy(transaction => transaction.Tag)
                .Select(group => CreateMonthlySumsFor(group.Key, group))
                .ToArray();
        }

        IEnumerable<DateTime> getMonths() {
            var min = Sums.Min(v => v.Sums.Min(v2 => v2.Key));
            var max = Sums.Max(v => v.Sums.Max(v2 => v2.Key));
            for(var month = min; month<=max; month = GetNextMonth(month)) {
                yield return month;
            }
        }

        DateTime GetNextMonth(DateTime month) {
            return new DateTime(
                month.Month < 12 ? month.Year : month.Year + 1, 
                month.Month < 12 ? month.Month + 1 : 1, 
                1
            );
        }

        string GetMonthRepresentation(DateTime month) {
            return string.Format("{0:00}/{1}", month.Month, month.Year);
        }

        public string GetReportText() {
            var months = getMonths().ToArray();
            var sb = new StringBuilder();
            sb.Append("\t");
            foreach (var month in months) {
                sb.AppendFormat("{0}\t\t", GetMonthRepresentation(month));
            }
            sb.AppendLine();
            foreach (var sum in Sums.OrderBy(sum => sum.Tag)) {
                sb.AppendLine(GetTagRowText(months, sum));
            }
            return sb.ToString();
        }

        decimal GetMonthSum(DateTime month) {
            return Sums.Sum(v => v.Sums.ContainsKey(month) ? v.Sums[month] : 0);
        }

        public void WriteToFile(string fileName) {
            File.WriteAllText(fileName, GetReportText());
        }

        private string GetTagRowText(DateTime[] months, MonthlyTagSums monthlyTagSums) {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}\t", monthlyTagSums.Tag);
            foreach(var month in months) {
                sb.AppendFormat("{0}\t\t", 
                    monthlyTagSums.Sums.ContainsKey(month) ? monthlyTagSums.Sums[month].ToString() : "");
            }
            return sb.ToString();
        }

        decimal GetAmountSum(IEnumerable<Transaction> transactions) {
            return transactions.Sum(transaction => transaction.Amount);
        }

        DateTime GetTimeStamp(DateTime dateTime) {
            if (dateTime.Day < 22) {
                return new DateTime(dateTime.Year, dateTime.Month, 1);
            } else {
                return GetNextMonth(dateTime);
            }
        }

        MonthlyTagSums CreateMonthlySumsFor(string Tag, IEnumerable<Transaction> transactions) {
            return new MonthlyTagSums(
                Tag ?? EmptyTag,
                transactions.GroupBy(t => GetTimeStamp(t.CreateDateTime))
                .ToDictionary(
                    mg => mg.Key,
                    GetAmountSum
                )
            );
        }

    }

    public class MonthlyTagSums {
        public string Tag { get; private set; }
        public ReadOnlyDictionary<DateTime, decimal> Sums { get; private set; }
        public MonthlyTagSums(
            string tag,
            IDictionary<DateTime, decimal> sums
        ) {
            Tag = tag;
            Sums = new ReadOnlyDictionary<DateTime, decimal>(sums);
        }
    }

}
