using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.DocumentReaders {
    abstract public class FileDocumentReader: IDocumentReader {
        public string InputFolder { get; set; }
        public Encoding Encoding { get; set; } = Encoding.Default;

        IEnumerable<string> ReadFiles() {
            return Directory.GetFiles(InputFolder).Select(v => File.ReadAllText(v, Encoding));
        }

        protected abstract Document ReadString(string s);

        public IEnumerable<Document> Read() {
            return ReadFiles().Select(ReadString);
        }

    }
}
