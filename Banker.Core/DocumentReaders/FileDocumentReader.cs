using Banker.Core.Loggers;
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

        ILogger logger;
        public FileDocumentReader(ILogger logger) {
            this.logger = logger;
        }

        IEnumerable<string> ReadFiles() {
            return Directory.GetFiles(InputFolder).Select(v => File.ReadAllText(v, Encoding));
        }

        protected abstract Document ReadString(string s);

        public IEnumerable<Document> Read() {
            logger.Log($"Start reading files from {InputFolder}");
            return ReadFiles().Select(ReadString);
        }

    }
}
