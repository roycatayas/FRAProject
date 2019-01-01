using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Service.Abstract;

namespace FRA.Repo.ContactPerson
{
    public class ContactPersonRepository : IContactPersonRepository
    {
        private readonly SqlConnection _sqlConnection;

        public ContactPersonRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }

        public Task<OperationResult> AddToContactPersonAsync(Data.Models.ContactPerson person)
        {
            const string command = "INSERT INTO [dbo].[ContactPerson]([RiskAssessmentID],[FullName],[PhoneNumber]) " +
                                   "VALUES (@RiskAssessmentID,@FullName,@PhoneNumber);";
            
            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                person.RiskAssessmentID,
                person.FullName,
                person.PhoneNumber
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1)
                ? OperationResult.Success
                : OperationResult.Failed(new OperationResultError
                {
                    Code = string.Empty,
                    Description =
                        $"The Contact Person with name {person.FullName} could not be inserted from the dbo.ContactPerson table."
                }));
        }

        public Task<OperationResult> RemoveFromContactPersonAsync(Data.Models.ContactPerson person)
        {
            const string command = "DELETE FROM dbo.ContactPerson WHERE ContactPersonID = @ContactPersonID and RiskAssessmentID = @RiskAssessmentID;";

            int rowsDeleted = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                person.ContactPersonID,
                person.RiskAssessmentID
            })).Result;

            return Task.FromResult(rowsDeleted.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Contact Person with name {person.FullName} could not be deleted from the dbo.ContactPerson table."
            }));
        }

        public async Task<IEnumerable<Data.Models.ContactPerson>> GetContactPersonById(string dataId)
        {
            const string command = "SELECT * FROM ContactPerson where RiskAssessmentID = @RiskAssessmentID;";

            IEnumerable<Data.Models.ContactPerson> contactPersons = await _sqlConnection.QueryAsync<Data.Models.ContactPerson>(command, new
            {
                RiskAssessmentID = dataId
            });

            return contactPersons;
        }

        public Task<Data.Models.ContactPerson> GetContactPersonById(Data.Models.ContactPerson person)
        {
            const string command = "select * from ContactPerson WHERE ContactPersonID = @ContactPersonID and RiskAssessmentID = @RiskAssessmentID;";

            return _sqlConnection.QuerySingleOrDefaultAsync<Data.Models.ContactPerson>(command, new
            {
                person.ContactPersonID,
                person.RiskAssessmentID
            });
        }
    }
}
