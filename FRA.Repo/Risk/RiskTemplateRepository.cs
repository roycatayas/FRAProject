using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Service.Abstract;

namespace FRA.Repo.Risk
{
    public class RiskTemplateRepository : ITableDataRepository<RiskTemplate>
    {
        private readonly SqlConnection _sqlConnection;

        public RiskTemplateRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }

        public async Task<IEnumerable<RiskTemplate>> GetDataRocordsAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase)
        {
            IEnumerable<RiskTemplate> riskTemplates = await _sqlConnection.QueryAsync<RiskTemplate>("GetRiskTemplate", new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortExpression = sortExpression,
                SortDirection = sortDirection == SortDirection.Ascending ? "asc" : "desc",
                SearchPhrase = searchPhrase
            }, commandType: CommandType.StoredProcedure);

            return riskTemplates;
        }

        public int GetTotalNumberOfRecords()
        {
            const string command = "SELECT * FROM v_GetTotalNumberOfRiskTemplates;";

            return _sqlConnection.ExecuteScalar<int>(command);
        }

        public Task<RiskTemplate> FindByIdAsync(string dataId)
        {
            const string command = "Select TemplateID, CategoryID, SectionID, TempNumber, Questions, ControlGuidelines, Impact from RiskTemplate WHERE TemplateID = @TemplateID;";

            return _sqlConnection.QuerySingleOrDefaultAsync<RiskTemplate>(command, new
            {
                TemplateID = dataId
            });
        }

        public Task<IEnumerable<RiskTemplate>> GetAllRecords()
        {
            const string command = "SELECT * FROM RiskTemplate;";

            return _sqlConnection.QueryAsync<RiskTemplate>(command);
        }

        public Task<OperationResult> AddRecordyAsync(RiskTemplate data)
        {
            const string command = "Insert into RiskTemplate (CategoryID,SectionID,TempNumber,Questions,ControlGuidelines,Impact) " +
                                   "Values (@CategoryID, @SectionID, @TempNumber, @Questions, @ControlGuidelines, @Impact)";

            int rowsAdded = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.CategoryID,
                data.SectionID,
                data.TempNumber,
                data.Questions,
                data.ControlGuidelines,
                data.Impact
            })).Result;

            return Task.FromResult(rowsAdded.Equals(1)
                ? OperationResult.Success
                : OperationResult.Failed(new OperationResultError
                {
                    Code = string.Empty,
                    Description = $"The data your trying to save have an error please try again."
                }));
        }

        public Task<OperationResult> DeleteRecordAsync(RiskTemplate data)
        {
            const string command = "DELETE FROM RiskTemplate WHERE TemplateID = @TemplateID;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.TemplateID
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The RiskTemplate with name {data.Questions} and {data.ControlGuidelines} could not be deleted from the dbo.RiskTemplate table."
            }));
        }

        public Task<OperationResult> EditRecordAsync(RiskTemplate data)
        {
            const string command = "Update RiskTemplate Set CategoryID = @CategoryID, SectionID = @SectionID, TempNumber = @TempNumber, " +
                                   "Questions = @Questions, ControlGuidelines = @ControlGuidelines, Impact = @Impact " +
                                   "Where TemplateID = @TemplateID;";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.TemplateID,
                data.CategoryID,
                data.SectionID,
                data.TempNumber,
                data.Questions,
                data.ControlGuidelines,
                data.Impact
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The RiskTemplate with name {data.Questions} and {data.ControlGuidelines} could not be updated in the dbo.Section table."
            }));
        }        

        
    }
}
