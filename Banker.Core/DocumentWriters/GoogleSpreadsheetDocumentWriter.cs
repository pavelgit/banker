using System;
using System.Collections.Generic;
using System.Linq;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using System.Diagnostics;
using AutoMapper;
using Banker.Core.Loggers;

namespace Banker.Core.DocumentWriters {
    public class GoogleSpreadsheetDocumentWriter : IDocumentWriter {

        private class Cell {
            public uint Row;
            public uint Col;
            public string Id;
            public string Value;

            public Cell(uint row, uint col, string value) {
                Row = row;
                Col = col;
                Id = string.Format("R{0}C{1}", row, col);
                Value = value;
            }
        }

        private ISettingsStorage settingsStorage;
        private IMapper transactionMapper;
        private IGoogleAppDataProvider googleAppDataProvider;
        private ILogger logger;

        const string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";
        const string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";
        const string APPLICATION_NAME = "MySpreadsheetIntegration-v1";
        const string REFRESH_TOKEN_SESSINGS_KEY = "GoogleSpreadsheetDocumentWriterRefreshToken";
        const string SPREADSHEET_ID_KEY = "GoogleSpreadsheetDocumentWriterSpreadsheetId";
        const string MAIN_WORKSHEET_TITLE = "LiveList";
        const int DOCUMENT_DEFAULT_ROW_COUNT = 10000;

        public GoogleSpreadsheetDocumentWriter(
            ISettingsStorage settingsStorage,
            IMapper transactionMapper,
            IGoogleAppDataProvider googleAppDataProvider,
            ILogger logger
        ) {
            this.settingsStorage = settingsStorage;
            this.transactionMapper = transactionMapper;
            this.googleAppDataProvider = googleAppDataProvider;
            this.logger = logger;
        }

        public void WriteDocument(Document document) {
            logger.Log("Start writing document to google drive");
            var service = AuthorizeAndConnect();
            var spreadsheet = GetMainSpreadsheet(service);
            var worksheet = GetMainWorksheet(spreadsheet);
            FormatMainWorksheet(service, worksheet, document);
            WriteDataToWorksheet(service, worksheet, document);
            logger.Log("Finsihed writing document to google drive");
        }

        void GetAccessTokenThroughAuth(OAuth2Parameters parameters) {
            string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);
            Console.WriteLine(authorizationUrl);
            Process.Start(authorizationUrl);
            Console.WriteLine("Please visit the URL above to authorize your OAuth "
              + "request token.  Once that is complete, type in your access code to "
              + "continue...");
            parameters.AccessCode = Console.ReadLine();
            OAuthUtil.GetAccessToken(parameters);
        }

        bool TryRefreshAccessToken(OAuth2Parameters parameters, string refreshToken) {
            if(refreshToken == null) {
                return false;
            }
            parameters.RefreshToken = refreshToken;
            try {
                OAuthUtil.RefreshAccessToken(parameters);
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        SpreadsheetsService AuthorizeAndConnect() {
            logger.Log("Start connection and auth to google drive");
            OAuth2Parameters parameters = new OAuth2Parameters() {
                ClientId = googleAppDataProvider.GetClientId(),
                ClientSecret = googleAppDataProvider.GetClientSecret(),
                RedirectUri = REDIRECT_URI,
                Scope = SCOPE,
            };
            string refreshToken;
            if (
                !settingsStorage.TryGet(REFRESH_TOKEN_SESSINGS_KEY, out refreshToken) ||
                !TryRefreshAccessToken(parameters, refreshToken)
            ) {
                GetAccessTokenThroughAuth(parameters);
                settingsStorage.Set(REFRESH_TOKEN_SESSINGS_KEY, parameters.RefreshToken);
            }
            GOAuth2RequestFactory requestFactory =
                new GOAuth2RequestFactory(null, APPLICATION_NAME, parameters);
            SpreadsheetsService service = new SpreadsheetsService(APPLICATION_NAME);
            service.RequestFactory = requestFactory;
            logger.Log("Connected and authorized to google drive");
            return service;
        }

        SpreadsheetEntry GetMainSpreadsheet(SpreadsheetsService service) {
            logger.Log("Getting main spreadsheet");
            var query = new SpreadsheetQuery();
            var feed = service.Query(query);
            var spreadsheetId = settingsStorage.Get(SPREADSHEET_ID_KEY);
            var spreadsheet = (SpreadsheetEntry)(feed.Entries.Where(
                entry => entry.AlternateUri.Content.Contains(spreadsheetId)).FirstOrDefault());
            return spreadsheet;
        }

        WorksheetEntry GetMainWorksheet(SpreadsheetEntry spreadsheet) {
            logger.Log("Getting main worksheet");
            return (WorksheetEntry)spreadsheet.Worksheets.Entries.Where(
                worksheet => worksheet.Title.Text == MAIN_WORKSHEET_TITLE).First();
        }

        void FormatMainWorksheet(SpreadsheetsService service, WorksheetEntry worksheet, Document document) {
            logger.Log("Formatting main spreadsheet");
            var dummy = new GoogleSpreadsheetTransactionModel();
            worksheet.Cols = (uint)dummy.GetPairs().Count();
            worksheet.Rows = 1;
            worksheet.Update();
            worksheet.Rows = DOCUMENT_DEFAULT_ROW_COUNT;
            worksheet.Update();
            FillInHeaderData(service, worksheet);
        }

        void WriteDataToWorksheet(SpreadsheetsService service, WorksheetEntry worksheet, Document document) {
            logger.Log("Writing data to the worksheet");
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            CellFeed cellFeed = service.Query(cellQuery);
            var cells = CreateCellList(document);
            var queryBatchResponse = CellBatchQuery(service, cellFeed, cells);
            var updateBatchResponse = CellBatchUpdate(service, cellQuery, cellFeed, queryBatchResponse, cells);
        }

        void FillInHeaderData(SpreadsheetsService service, WorksheetEntry worksheet) {
            CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            CellFeed cellFeed = service.Query(cellQuery);
            var dummy = new GoogleSpreadsheetTransactionModel();
            uint col = 1;
            foreach (var pair in dummy.GetPairs()) {
                cellFeed.Insert(new CellEntry(1, col, pair.Key));
                col++;
            }
        }

        List<Cell> CreateCellList(Document document) {
            var cells = new List<Cell>();
            uint row = 2;
            foreach (var transaction in document.Transactions.OrderByDescending(
                transaction => transaction.CreateDateTime)
            ) {
                var transactionModel = transactionMapper.Map<GoogleSpreadsheetTransactionModel>(transaction);
                uint col = 1;
                foreach (var pair in transactionModel.GetPairs()) {
                    cells.Add(new Cell(row, col, pair.Value));
                    col++;
                }
                row++;
            }
            return cells;
        }

        CellFeed CellBatchQuery(SpreadsheetsService service, CellFeed cellFeed, IEnumerable<Cell> cells) {
            var batchRequest = new CellFeed(new Uri(cellFeed.Self), service);
            foreach (var cell in cells) {
                var batchEntry = new CellEntry(cell.Row, cell.Col, cell.Id);
                batchEntry.Id = new AtomId(string.Format("{0}/{1}", cellFeed.Self, cell.Id));
                batchEntry.BatchData = new GDataBatchEntryData(cell.Id, GDataBatchOperationType.query);
                batchRequest.Entries.Add(batchEntry);
            }
            CellFeed queryBatchResponse = (CellFeed)service.Batch(batchRequest, new Uri(cellFeed.Batch));
            return queryBatchResponse; 
        }

        CellFeed CellBatchUpdate(SpreadsheetsService service, CellQuery cellQuery, CellFeed cellFeed, 
            CellFeed queryBatchResponse, IEnumerable<Cell> cells) {
            var cellEntries = queryBatchResponse.Entries.ToDictionary(entry => entry.BatchData.Id);
            CellFeed batchRequest = new CellFeed(cellQuery.Uri, service);
            foreach (Cell cellAddr in cells) {
                CellEntry batchEntry = (CellEntry)cellEntries[cellAddr.Id];
                batchEntry.InputValue = cellAddr.Value;
                batchEntry.BatchData = new GDataBatchEntryData(cellAddr.Id, GDataBatchOperationType.update);
                batchRequest.Entries.Add(batchEntry);
            }
            CellFeed batchResponse = (CellFeed)service.Batch(batchRequest, new Uri(cellFeed.Batch));
            return batchResponse;
        }
    }
   
}
