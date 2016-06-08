using Banker.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Tags {
    public abstract class TagService : ITagService {

        protected ILogger logger;
        public TagService(ILogger logger) {
            this.logger = logger;
        }

        public void ApplyTag(Transaction transaction) {
            transaction.Tag = GetTag(transaction);
        }

        public void ApplyTags(Document document) {
            logger.Log("Start applying tags");
            foreach (var transaction in document.Transactions) {
                ApplyTag(transaction);
            }
        }

        public abstract string GetTag(Transaction transaction);
    }
}
