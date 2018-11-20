using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Service.Abstract;


namespace FRA.Repo.Section
{
    public class SectionRepository : ITableDataRepository<Data.Models.Section>
    {
        private readonly SqlConnection _sqlConnection;

        public SectionRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }

        public async Task<IEnumerable<Data.Models.Section>> GetDataRocordsAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection,
            string searchPhrase)
        {
            IEnumerable<Data.Models.Section> sections = await _sqlConnection.QueryAsync<Data.Models.Section>("GetSection", new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortExpression = sortExpression,
                SortDirection = sortDirection == SortDirection.Ascending ? "asc" : "desc",
                SearchPhrase = searchPhrase
            }, commandType: CommandType.StoredProcedure);

            return sections;
        }

        public int GetTotalNumberOfRecords()
        {
            const string command = "SELECT * FROM v_GetTotalNumberOfSections;";

            return _sqlConnection.ExecuteScalar<int>(command);
        }

        public Task<IEnumerable<Data.Models.Section>> GetAllRecords()
        {
            const string command = "SELECT * FROM Section;";

            return _sqlConnection.QueryAsync<Data.Models.Section>(command);
        }

        public Task<Data.Models.Section> FindByIdAsync(string dataId)
        {
            const string command = "select SectionID, CategoryID, SectionName from Section WHERE SectionID = @SectionID;";

            return _sqlConnection.QuerySingleOrDefaultAsync<Data.Models.Section>(command, new
            {
                SectionID = dataId
            });
        }

        public Task<OperationResult> AddRecordyAsync(Data.Models.Section data)
        {
            const string command = "INSERT INTO Section (CategoryID, SectionName)" +
                                   "VALUES (@CategoryID, @SectionName)";

            int rowsInserted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.CategoryID,
                data.SectionName
            })).Result;

            return Task.FromResult(rowsInserted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Section with name {data.SectionName} could not be inserted in the dbo.Section table."
            }));
        }

        public Task<OperationResult> DeleteRecordAsync(Data.Models.Section data)
        {
            const string command = "DELETE FROM Section WHERE SectionID = @SectionID;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.SectionID
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Section with name {data.SectionName} could not be deleted from the dbo.Section table."
            }));
        }

        public Task<OperationResult> EditRecordAsync(Data.Models.Section data)
        {
            const string command = "UPDATE Section SET CategoryID = @CategoryID, SectionName = @SectionName WHERE SectionID = @SectionID;";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.SectionName,
                data.CategoryID,
                data.SectionID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Section with name {data.SectionName} could not be updated in the dbo.Section table."
            }));
        }

        
    }
}
