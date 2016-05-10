using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Tags {
    public abstract class TagService : ITagService {
        public void ApplyTag(Transaction transaction) {
            transaction.Tag = GetTag(transaction);
        }

        public void ApplyTags(Document document) {
            foreach(var transaction in document.Transactions) {
                ApplyTag(transaction);
            }
        }

        public abstract string GetTag(Transaction transaction);
    }
}
