using AutoMapper;
using Banker.Core.DocumentWriters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banker.Core.Mappers {
    public class GoogleSpreadsheetTransactionModelMapperBuilder {
        public MapperConfiguration BuildConfiguration() {
            return new MapperConfiguration(cfg => cfg.CreateMap<Transaction, GoogleSpreadsheetTransactionModel>());
        }
    }
}
