using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.IdentityProvider.Entities;
using FRA.Service.Abstract;

namespace FRA.Repo.User
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly SqlConnection _sqlConnection;

        public UserRoleRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }
        public Task<OperationResult> AddToRoleAsync(ApplicationUser user, Guid roleId)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> RemoveFromRoleAsync(ApplicationUser user, Guid roleId)
        {
            const string command = "DELETE FROM dbo.UsersRoles WHERE UserId = @UserId AND RoleId = @RoleId;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                UserId = user.Id,
                RoleId = roleId
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Role could not be deleted from the UsersRoles table."
            }));
        }
    }
}
