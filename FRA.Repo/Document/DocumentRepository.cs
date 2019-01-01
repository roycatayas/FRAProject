using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Service.Abstract;

namespace FRA.Repo.Document
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly SqlConnection _sqlConnection;

        public DocumentRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }
        public Task<OperationResult> AddToDocumentAsync(Data.Models.Document document)
        {
            const string command = "INSERT INTO [dbo].[Document]([RiskAssessmentID],[DocumentName],[FileName],[FTPLink],[DocumentGUID]) " +
                                   "VALUES (@RiskAssessmentID,@DocumentName,@FileName,@FTPLink,@DocumentGUID);";

            int rowsAdded = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                document.RiskAssessmentID,
                document.DocumentName,
                document.FileName,
                document.FTPLink,
                document.DocumentGUID
            })).Result;

            return Task.FromResult(rowsAdded.Equals(1)
                ? OperationResult.Success
                : OperationResult.Failed(new OperationResultError
                {
                    Code = string.Empty,
                    Description =
                        $"The file with name {document.DocumentName} could not be inserted from the dbo.Document table."
                }));
        }

        public Task<OperationResult> RemoveFromDocumentAsync(Data.Models.Document document)
        {
            const string command = "DELETE FROM dbo.Document WHERE DocId = @DocId and RiskAssessmentID = @RiskAssessmentID;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                document.DocId,
                document.RiskAssessmentID
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The file with name {document.DocumentName} could not be deleted from the dbo.Document table."
            }));
        }

        public async Task<IEnumerable<Data.Models.Document>> GetDocumentById(string dataId)
        {
            const string command = "SELECT * FROM Document where RiskAssessmentID = @RiskAssessmentID;";

            IEnumerable<Data.Models.Document> documents = await _sqlConnection.QueryAsync<Data.Models.Document>(command, new
            {
                RiskAssessmentID = dataId
            });

            return documents;
        }

        public Task<Data.Models.Document> GetDocumentById(Data.Models.Document document)
        {
            const string command = "select * from Document WHERE DocId = @DocId and RiskAssessmentID = @RiskAssessmentID;";

            return _sqlConnection.QuerySingleOrDefaultAsync<Data.Models.Document>(command, new
            {
                document.DocId,
                document.RiskAssessmentID
            });
        }
    }
}
