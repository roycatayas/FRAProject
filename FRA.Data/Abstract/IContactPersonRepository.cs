using System.Collections.Generic;
using System.Threading.Tasks;
using FRA.Data.Models;

namespace FRA.Data.Abstract
{
    public interface IContactPersonRepository
    {        
        Task<OperationResult> AddToContactPersonAsync(ContactPerson person);
        Task<OperationResult> RemoveFromContactPersonAsync(ContactPerson person);
        Task<IEnumerable<ContactPerson>> GetContactPersonById(string dataId);
        Task<ContactPerson> GetContactPersonById(ContactPerson person);
    }
}
