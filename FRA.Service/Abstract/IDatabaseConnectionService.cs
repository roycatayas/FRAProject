using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace FRA.Service.Abstract
{
    public interface IDatabaseConnectionService : IDisposable
    {
        Task<SqlConnection> CreateConnectionAsync();
        SqlConnection CreateConnection();
    }
}
