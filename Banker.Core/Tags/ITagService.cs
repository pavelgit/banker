using System;
using System.Linq;

namespace Banker.Core.Tags {
    public interface ITagService {
        string GetTag(Transaction transaction);
        void ApplyTag(Transaction transactions);
        void ApplyTags(Document document);
    }
}