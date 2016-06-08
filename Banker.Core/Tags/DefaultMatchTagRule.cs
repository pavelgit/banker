using Banker.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Banker.Core.Tags {
    public class DefaultMatchTagRule : TagService {
        public string Tag;
        public string[] Matches;

        public DefaultMatchTagRule(ILogger logger):base(logger) {
        }

        public DefaultMatchTagRule(string tag, string[] matches, ILogger logger):this(logger) {
            this.Tag = tag;
            this.Matches = matches;
        }

        IEnumerable<string> GetTransactionDescriptionTexts(Transaction transaction) {
            yield return transaction.Text;
            yield return transaction.Receiver;
            yield return transaction.Type;
            yield return transaction.AccountNumber;
            yield return transaction.BankCode;
        }

        string GetRegexPatternForMatch(string match) {
            return string.Format(@"\b{0}\b", Regex.Escape(match));
        }

        string[] SplitToWords(string str) {
            return Regex.Split(str, @"[\s\t]+");
        }

        bool IsMatch(string match, string text) {
            return SplitToWords(match).All(matchWord => Regex.IsMatch(
                text, GetRegexPatternForMatch(matchWord), RegexOptions.IgnoreCase));
        }

        bool IsMatchAny(IEnumerable<string> matches, string text) {
            return matches.Any(match => IsMatch(match, text));
        }

        public override string GetTag(Transaction transaction) {
            var text = string.Join(" ", GetTransactionDescriptionTexts(transaction));
            return IsMatchAny(Matches, text) ? Tag : null;
        }
    }
}
