using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FRA.Data.Models;

namespace FRA.Data.Abstract
{
    public interface ITableDataRepository<T>
    {
        Task<IEnumerable<T>> GetDataRocordsAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase);
        int GetTotalNumberOfRecords();

        Task<T> FindByIdAsync(string dataId);

        Task<IEnumerable<T>> GetAllRecords();

        Task<OperationResult> AddRecordyAsync(T data);

        Task<OperationResult> DeleteRecordAsync(T data);

        Task<OperationResult> EditRecordAsync(T data);
    }
}
