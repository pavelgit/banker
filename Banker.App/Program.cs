using Banker.Core;
using Banker.Core.DocumentReaders;
using Banker.Core.DocumentWriters;
using Banker.Core.Loggers;
using Banker.Core.Mappers;
using Banker.Core.Reports;
using Banker.Core.Tags;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.App {
    class Program {
        static void Main(string[] args) {

            var logger = new ConsoleLogger();
            var documentAggregationService = new DocumentAggregationService();

            var documentReaders = new IDocumentReader[] {
                new CommerzbankDocumentReader(logger) { InputFolder = @"docs\commerzbank" },
                new ConsorsbankDocumentReader(logger) { InputFolder = @"docs\consorsbank-pasha" },
                new ConsorsbankDocumentReader(logger) { InputFolder = @"docs\consorsbank-dasha" },
            };

            var tagService = new YamlListTagService(logger);
            tagService.InitFromYaml(File.ReadAllText(@"docs\tags.yaml"));
            logger.Log($"Read {tagService.TagServices.Length} tags");
  
            var aggregatedDocuments = documentReaders
                .Select(reader => documentAggregationService.Aggregate(reader.Read())).ToArray();

            var finalDocument = new Document() {
                Transactions = aggregatedDocuments.SelectMany(v => v.Transactions).ToArray()
            };

            tagService.ApplyTags(finalDocument);

            var googleDocumentWriter = new GoogleSpreadsheetDocumentWriter(
                new ApplicationSettingsStorage(Properties.Settings.Default),
                new GoogleSpreadsheetTransactionModelMapperBuilder().BuildConfiguration().CreateMapper(),
                new DefaultGoogleAppDataProvider(),
                logger
            );
           
            googleDocumentWriter.WriteDocument(finalDocument);
        }
    }
}
