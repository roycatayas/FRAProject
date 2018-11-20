using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Service.Abstract;

namespace FRA.Repo.User
{
    public class UserRepository : IUserRepository   
    {
        private readonly SqlConnection _sqlConnection;

        public UserRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }

        public int GetTotalNumberOfUsers()
        {
            const string command = "SELECT * FROM v_GetTotalNumberOfUsers;";

            return _sqlConnection.ExecuteScalar<int>(command);
        }

        public async Task<IEnumerable<Data.Models.User>> GetUsersAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase)
        {
            IEnumerable<Data.Models.User> users = await _sqlConnection.QueryAsync<Data.Models.User>("GetsUsers", new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortExpression = sortExpression,
                SortDirection = sortDirection == SortDirection.Ascending ? "asc" : "desc",
                SearchPhrase = searchPhrase
            }, commandType: CommandType.StoredProcedure);

            return users;
        }
    }
}
