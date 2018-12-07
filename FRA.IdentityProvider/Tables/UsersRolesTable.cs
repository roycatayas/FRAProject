using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using FRA.IdentityProvider.Entities;

namespace FRA.IdentityProvider.Tables
{
    public class UsersRolesTable
    {
        private readonly SqlConnection _sqlConnection;

        public UsersRolesTable(SqlConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }

        public Task AddToRoleAsync(ApplicationUser user, Guid roleId)
        {
            const string command = "INSERT INTO dbo.UsersRoles " +
                                   "VALUES (@UserId, @RoleId);";

            return _sqlConnection.ExecuteAsync(command, new
            {
                UserId = user.Id,
                RoleId = roleId
            });
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, Guid roleId)
        {
            const string command = "DELETE FROM dbo.UsersRoles " +
                                   "WHERE UserId = @UserId AND RoleId = @RoleId;";

            return _sqlConnection.ExecuteAsync(command, new
            {
                UserId = user.Id,
                RoleId = roleId
            });
        }

        public Task<IList<string>> GetRolesAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            const string command = "SELECT r.Name " +
                                   "FROM dbo.Roles as r " +
                                   "INNER JOIN dbo.UsersRoles AS ur ON ur.RoleId = r.Id " +
                                   "WHERE ur.UserId = @UserId;";

            IEnumerable<string> userRoles = Task.Run(() => _sqlConnection.QueryAsync<string>(command, new
            {
                UserId = user.Id
            }), cancellationToken).Result;

            return Task.FromResult<IList<string>>(userRoles.ToList());
        }

        public Task<IList<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            const string command = "SELECT * " +
                                   "FROM dbo.Users AS u " +
                                   "INNER JOIN dbo.UserRoles AS ur ON u.Id = ur.UserId " +
                                   "INNER JOIN dbo.Roles AS r ON ur.RoleId = r.Id " +
                                   "WHERE r.Name = @RoleName;";

            IEnumerable<ApplicationUser> userRoles = Task.Run(() => _sqlConnection.QueryAsync<ApplicationUser>(command, new
            {
                RoleName = roleName
            })).Result;

            return Task.FromResult<IList<ApplicationUser>>(userRoles.ToList());
        }
    }
}
