using Banker.Core.DocumentReaders;
using Banker.Core.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core
{
    public class Account
    {
        public string Name { get; set; }
        private IDocumentReader DocumentReader { get; set; }
        private IDocumentWriter DocumentWriter { get; set; }
        private ITagService TagService { get; set; }

        public Account(
            IDocumentReader documentReader,
            IDocumentWriter documentWriter,
            ITagService tagService
        ) {
            DocumentReader = documentReader;
            DocumentWriter = documentWriter;
            TagService = tagService;
        }



    }
}
