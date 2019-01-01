using System.Collections.Generic;
using System.Threading.Tasks;
using FRA.Data.Models;

namespace FRA.Data.Abstract
{
    public interface IDocumentRepository
    {
        Task<OperationResult> AddToDocumentAsync(Document document);
        Task<OperationResult> RemoveFromDocumentAsync(Document document);
        Task<IEnumerable<Document>> GetDocumentById(string dataId);
        Task<Document> GetDocumentById(Document document);
    }
}
