using System.Collections.Generic;
using System.Threading.Tasks;
using FRA.Data.Models;

namespace FRA.Data.Abstract
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase);
        int GetTotalNumberOfUsers();
    }
}
