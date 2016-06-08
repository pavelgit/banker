using Banker.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Tags {
    public class ListTagService : TagService {

        public ListTagService(ILogger logger) : base(logger) {

        }

        public ITagService[] TagServices { get; set; }

        public override string GetTag(Transaction transaction) {
            return TagServices
                .Select(tagService => tagService.GetTag(transaction))
                .FirstOrDefault(tag => tag != null);
        }
    }
}
