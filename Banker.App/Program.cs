using Banker.Core;
using Banker.Core.DocumentReaders;
using Banker.Core.DocumentWriters;
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

            var documentAggregationService = new DocumentAggregationService();

            var docuentReaders = new IDocumentReader[] {
                new CommerzbankDocumentReader() { InputFolder = @"docs\commerzbank" },
                new ConsorsbankDocumentReader() { InputFolder = @"docs\consorsbank-pasha" },
                new ConsorsbankDocumentReader() { InputFolder = @"docs\consorsbank-dasha" },
            };

            var tagService = new YamlListTagService();
            tagService.InitFromYaml(File.ReadAllText(@"docs\tags.yaml"));

            var aggregatedDocuments = docuentReaders
                .Select(reader => documentAggregationService.Aggregate(reader.Read())).ToArray();

            var finalDocument = new Document() {
                Transactions = aggregatedDocuments.SelectMany(v => v.Transactions).ToArray()
            };

            tagService.ApplyTags(finalDocument);

            var googleDocumentWriter = new GoogleSpreadsheetDocumentWriter(
                new ApplicationSettingsStorage(Properties.Settings.Default),
                new GoogleSpreadsheetTransactionModelMapperBuilder().BuildConfiguration().CreateMapper(),
                new DefaultGoogleAppDataProvider()
            );
           
            googleDocumentWriter.WriteDocument(finalDocument);
        }
    }
}
