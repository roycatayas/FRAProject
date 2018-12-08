using System;
using System.Threading.Tasks;
using FRA.IdentityProvider.Entities;

namespace FRA.Data.Abstract
{
    public interface IUserRoleRepository
    {
        Task<OperationResult> AddToRoleAsync(ApplicationUser user, Guid roleId);
        Task<OperationResult> RemoveFromRoleAsync(ApplicationUser user, Guid roleId);
    }
}
