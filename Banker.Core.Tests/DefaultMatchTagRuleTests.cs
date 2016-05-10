using Banker.Core.Tags;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Tests
{
    [TestFixture]
    public class DefaultMatchTagRuleTests {

        static object[] Cases =
        {
            new object[] {
                new DefaultMatchTagRule {
                    Tag = "mytag",
                    Matches = new string[] {
                        "OLIVER Twist pizza",
                    }
                },
                new Transaction {
                    Text = "Bought pizza",
                    Receiver = "Oliver Twist",
                    Type = "Credit card",
                },
                "mytag"
            },
            new object[] {
                new DefaultMatchTagRule {
                    Tag = "mytag",
                    Matches = new string[] {
                        "OLIVER TWIST burger",
                        "OLIVER TWIST pizza",
                    }
                },
                new Transaction {
                    Text = "Twist pizza",
                    Receiver = "Oliver",
                    Type = null,
                },
                "mytag"
            },
            new object[] {
                new DefaultMatchTagRule {
                    Tag = "mytag",
                    Matches = new string[] {
                        "Oliver Twist burger",
                        "Oliver Twist pineaple",
                    }
                },
                new Transaction {
                    Text = "Bought pizza",
                    Receiver = "Oliver Twist",
                    Type = "Credit card",
                },
                null
            },
        };

        [Test, TestCaseSource(nameof(Cases))]
        public void When_the_rule_has_particular_data_and_transaction_has_particular_data_should_return_particular_tag(
            DefaultMatchTagRule rule, Transaction transaction, string expectedTag
        ) {
            var tag = rule.GetTag(transaction);
            Assert.AreEqual(expectedTag, tag);
        }


    }
}