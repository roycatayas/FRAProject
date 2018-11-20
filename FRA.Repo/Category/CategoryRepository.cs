using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Service.Abstract;

namespace FRA.Repo.Category
{
    public class CategoryRepository : ITableDataRepository<Catergory>
    {
        private readonly SqlConnection _sqlConnection;

        public CategoryRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }       

        public async Task<IEnumerable<Catergory>> GetDataRocordsAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase)
        {
            IEnumerable<Catergory> catergories = await _sqlConnection.QueryAsync<Catergory>("GetCategory", new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortExpression = sortExpression,
                SortDirection = sortDirection == SortDirection.Ascending ? "asc" : "desc",
                SearchPhrase = searchPhrase
            }, commandType: CommandType.StoredProcedure);

            return catergories;
        }

        public int GetTotalNumberOfRecords()
        {
            const string command = "SELECT * FROM v_GetTotalNumberOfCategorys;";

            return _sqlConnection.ExecuteScalar<int>(command);
        }

        public Task<Catergory> FindByIdAsync(string categoryId)
        {
            const string command = "select CategoryID,CategoryName from Category WHERE CategoryID = @CategoryID;";

            return _sqlConnection.QuerySingleOrDefaultAsync<Catergory>(command, new
            {
                CategoryID = categoryId
            });
        }

        public Task<IEnumerable<Catergory>> GetAllRecords()
        {
            const string command = "SELECT * FROM dbo.Category;";

            return _sqlConnection.QueryAsync<Catergory>(command);
        }

        public Task<OperationResult> AddRecordyAsync(Catergory data)
        {
            const string command = "INSERT INTO dbo.Category VALUES (@CategoryName);";            

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.CategoryName
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Category with name {data.CategoryName} could not be deleted from the dbo.Category table."
            }));
        }

        public Task<OperationResult> DeleteRecordAsync(Catergory data)
        {
            const string command = "DELETE FROM dbo.Category WHERE CategoryID = @CategoryID;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.CategoryID
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Category with name {data.CategoryName} could not be deleted from the dbo.Category table."
            }));
        }

        public Task<OperationResult> EditRecordAsync(Catergory data)
        {
            const string command = "UPDATE Category SET CategoryName = @CategoryName WHERE CategoryID = @CategoryID;";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.CategoryName,
                data.CategoryID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Category with name {data.CategoryName} could not be updated in the dbo.Category table."
            }));
        }
    }
}
