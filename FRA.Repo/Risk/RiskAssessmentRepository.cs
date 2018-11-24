using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using FRA.Data.Abstract;
using FRA.Data.Models;
using FRA.Data.View;
using FRA.Service.Abstract;

namespace FRA.Repo.Risk
{
    public class RiskAssessmentRepository : IRiskAssessmentRepository
    {
        private readonly SqlConnection _sqlConnection;

        public RiskAssessmentRepository(IDatabaseConnectionService databaseConnection)
        {
            _sqlConnection = databaseConnection.CreateConnection();
        }

        public async Task<IEnumerable<RiskAssessmentView>> GetRiskAssessmentAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase)
        {
            IEnumerable<RiskAssessmentView> riskAssessment = await _sqlConnection.QueryAsync<RiskAssessmentView>("GetRiskAssessment", new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortExpression = sortExpression,
                SortDirection = sortDirection == SortDirection.Ascending ? "asc" : "desc",
                SearchPhrase = searchPhrase
            }, commandType: CommandType.StoredProcedure);

            return riskAssessment;
        }

        public int GetTotalNumberOfAssemment()
        {
            throw new NotImplementedException();
        }

        public Task<RiskAssessmentView> FindByIdAsync(string dataId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("RiskAssessmentID", dataId);            

            return _sqlConnection.QuerySingleOrDefaultAsync<RiskAssessmentView>("FindByIdRiskAssessment", param, commandType: CommandType.StoredProcedure);
        }

        public Task<IEnumerable<RiskAssessmentView>> GetAllRecords()
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> AddRiskAsync(RiskAssessmentView data)
        {
            #region parameters
            DynamicParameters param = new DynamicParameters();
            param.Add("SubjectTitle", data.SubjectTitle);
            param.Add("Organization", data.Organization);
            param.Add("DocumentNo", data.DocumentNo);
            param.Add("Owner", data.Owner);
            param.Add("ApproveBy", data.ApprovedBy);
            param.Add("EntryDate", data.EntryDate);
            param.Add("RevisionNo", data.RevisionNo);
            param.Add("SurveyorName", data.SurveyorName);
            param.Add("SurveyorNumber", data.SurveyorTelephone);
            param.Add("SurveyorEmail", data.SurveyorEmail);
            param.Add("SurveyDate", data.SurveyDate);
            param.Add("SiteName", data.SiteName);
            param.Add("Country", data.SiteCountry);
            param.Add("Address", data.SiteAdress);
            param.Add("ProvinceState", data.SiteStateProvince);
            param.Add("PrimaryContactName", data.ContactPersonName);
            param.Add("PhoneNumber", data.ContactPersonTelephone);
            param.Add("FaxNumber", data.ContactPersonFaxNumber);
            param.Add("EmailAdress", data.ContactPersonEmail);
            param.Add("URLAdress", data.ContactPersonWebsiteUrl);
            param.Add("RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            #endregion

            _sqlConnection.Execute("InsertRiskAssessment", param, commandType: CommandType.StoredProcedure);
            int rowsAdded = param.Get<int>("RetVal");

            return Task.FromResult(rowsAdded.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
                {
                    Code = string.Empty,
                    Description = $"The data your trying to save have an error please try again."
                }));
        }

        public Task<OperationResult> EditRiskAsync(RiskAssessmentView data)
        {
            #region parameters
            DynamicParameters param = new DynamicParameters();
            param.Add("RiskAssessmentID", data.RiskAssessmentID);
            param.Add("SubjectTitle", data.SubjectTitle);
            param.Add("Organization", data.Organization);
            param.Add("DocumentNo", data.DocumentNo);
            param.Add("Owner", data.Owner);
            param.Add("ApproveBy", data.ApprovedBy);
            param.Add("EntryDate", data.EntryDate);
            param.Add("RevisionNo", data.RevisionNo);
            param.Add("SurveyorName", data.SurveyorName);
            param.Add("SurveyorNumber", data.SurveyorTelephone);
            param.Add("SurveyorEmail", data.SurveyorEmail);
            param.Add("SurveyDate", data.SurveyDate);
            param.Add("SiteName", data.SiteName);
            param.Add("Country", data.SiteCountry);
            param.Add("Address", data.SiteAdress);
            param.Add("ProvinceState", data.SiteStateProvince);
            param.Add("PrimaryContactName", data.ContactPersonName);
            param.Add("PhoneNumber", data.ContactPersonTelephone);
            param.Add("FaxNumber", data.ContactPersonFaxNumber);
            param.Add("EmailAdress", data.ContactPersonEmail);
            param.Add("URLAdress", data.ContactPersonWebsiteUrl);
            param.Add("RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            #endregion

            _sqlConnection.Execute("EditRiskAssessment", param, commandType: CommandType.StoredProcedure);
            int rowsAdded = param.Get<int>("RetVal");

            return Task.FromResult(rowsAdded.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The data your trying to update have an error please try again."
            }));
        }

        public Task<OperationResult> AddRiskDetailScoreAsync(RiskDetailScoreView data)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("RiskAssessmentID", data.RiskAssessmentID);
            param.Add("CategoryID", data.CategoryID);
            param.Add("CategoryName", data.CategoryName);
            param.Add("ParticipantsNo", data.ParticipantsNo);
            
            param.Add("RetVal", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            _sqlConnection.Execute("InsertRiskDetailSectionScore", param, commandType: CommandType.StoredProcedure);
            int rowsAdded = param.Get<int>("RetVal");

            return Task.FromResult(rowsAdded.Equals(1)
                ? OperationResult.Success
                : OperationResult.Failed(new OperationResultError
                {
                    Code = string.Empty,
                    Description = $"The data your trying to save have an error please try again."
                }));
        }

        public async Task<IEnumerable<RiskDetailScoreView>> GetRiskDetailScoreRecordsByRiskId(int riskAssessmentId)
        {
            const string command = "SELECT * FROM RiskDetailScore where RiskAssessmentID = @RiskAssessmentID;";            

            IEnumerable<RiskDetailScoreView> riskDetailSecore = await _sqlConnection.QueryAsync<RiskDetailScoreView>(command, new
            {
                RiskAssessmentID = riskAssessmentId
            });

            return riskDetailSecore;
        }

        public async Task<IEnumerable<RiskSectionScoreView>> GetRiskSectionScoreRecordsByRiskId(int riskAssessmentId, int riskDetailId)
        {
            const string command = "select * from RiskSectionScore where RiskAssessmentID = @RiskAssessmentID and RiskDetailsID = @RiskDetailsID;";
            
            IEnumerable<RiskSectionScoreView> riskSectionSecore = await _sqlConnection.QueryAsync<RiskSectionScoreView>(command, new
            {
                RiskAssessmentID = riskAssessmentId,
                RiskDetailsID = riskDetailId
            });

            return riskSectionSecore;
        }

        public async Task<IEnumerable<RiskGuidelinesScoreView>> GetRiskGuidelinesScoreViewByRiskId(int riskAssessmentId, int riskDetailsId, int riskSectionScoreId)
        {
            const string command = "select * from RiskGuidelinesScore where RiskAssessmentID = @RiskAssessmentID and RiskDetailsID = @RiskDetailsID and RiskSectionScoreID = @RiskSectionScoreID;";

            IEnumerable<RiskGuidelinesScoreView> riskSectionSecore = await _sqlConnection.QueryAsync<RiskGuidelinesScoreView>(command, new
            {
                RiskAssessmentID = riskAssessmentId,
                RiskDetailsID = riskDetailsId,
                RiskSectionScoreID = riskSectionScoreId
            });

            return riskSectionSecore;
        }

        public async Task<IEnumerable<RiskParticipantsScoreView>> GetRiskParticipantsScoreRecordsById(int riskGuidelinesScoreId, int riskSectionScoreId, int riskDetailsId)
        {
            const string command = "select * from RiskParticipantsScore where RiskGuidelinesScoreID = @RiskGuidelinesScoreID and RiskSectionScoreID = @RiskSectionScoreID and RiskDetailsID = @RiskDetailsID;";

            IEnumerable<RiskParticipantsScoreView> riskSectionSecore = await _sqlConnection.QueryAsync<RiskParticipantsScoreView>(command, new
            {
                RiskGuidelinesScoreID = riskGuidelinesScoreId,
                RiskSectionScoreID = riskSectionScoreId,
                RiskDetailsID = riskDetailsId
            });

            return riskSectionSecore;
        }

        public Task<OperationResult> UpdateRiskDetailScoreAsync(RiskDetailScoreView data)
        {
            const string command = "Update RiskDetailScore set Maturity = @Maturity, Efficiency = @Efficiency, RiskLevel = @RiskLevel "
                                 + "Where RiskDetailsID = @RiskDetailsID";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.Maturity,
                data.Efficiency,
                data.RiskLevel,
                data.RiskDetailsID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Maturity, Efficiency and RiskLevel with ID {data.RiskDetailsID} could not be updated in the dbo.RiskDetailScore table."
            }));
        }

        public Task<OperationResult> UpdateRiskSectionScoreAsync(RiskSectionScoreView data)
        {
            const string command = "Update RiskSectionScore set Maturity = @Maturity, Efficiency = @Efficiency, RiskLevel = @RiskLevel "
                                  +"Where RiskSectionScoreID = @RiskSectionScoreID";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.Maturity,
                data.Efficiency,
                data.RiskLevel,
                data.RiskSectionScoreID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Section score with name {data.SectionName} could not be updated in the dbo.RiskSectionScore table."
            }));
        }

        public Task<OperationResult> UpdateRiskParticipantsScoreAsync(RiskParticipantsScoreView data)
        {
            const string command = "Update RiskParticipantsScore set ParticipantScore = @ParticipantScore "
                                 + "where RiskParticipantsID = @RiskParticipantsID";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.ParticipantScore,
                data.RiskParticipantsID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Participant score with ID {data.RiskParticipantsID} could not be updated in the dbo.RiskParticipantsScore table."
            }));
        }

        public Task<OperationResult> UpdateRiskGuidelinesScoreAsync(RiskGuidelinesScoreView data)
        {
            const string command = "Update RiskGuidelinesScore set MaturityEarned = @MaturityEarned, Impact = @Impact, Comments = @Comments "
                                 + "Where RiskGuidelinesScoreID = @RiskGuidelinesScoreID";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                data.RiskGuidelinesScoreID,
                data.MaturityEarned,
                data.Impact,
                data.Comments
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The MaturityEarned score with ID {data.RiskGuidelinesScoreID} could not be updated in the dbo.RiskGuidelinesScore table."
            }));
        }

        public Task<OperationResult> UpdateRiskGuidelinesScoreAsync(string RiskGuidelinesScoreID, string Comments)
        {
            const string command = "Update RiskGuidelinesScore set Comments = @Comments "
                                 + "Where RiskGuidelinesScoreID = @RiskGuidelinesScoreID";

            int rowsUpdated = Task.Run(() => _sqlConnection.ExecuteAsync(command, new
            {
                Comments,
                RiskGuidelinesScoreID
            })).Result;

            return Task.FromResult(rowsUpdated.Equals(1) ? OperationResult.Success : OperationResult.Failed(new OperationResultError
            {
                Code = string.Empty,
                Description = $"The Comments with ID {RiskGuidelinesScoreID} could not be updated in the dbo.RiskGuidelinesScore table."
            }));
        }
    }
}
