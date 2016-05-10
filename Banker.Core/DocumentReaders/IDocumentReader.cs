using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentReaders {
    public interface IDocumentReader {
        IEnumerable<Document> Read();
    }
}
