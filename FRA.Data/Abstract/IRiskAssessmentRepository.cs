using System.Collections.Generic;
using System.Threading.Tasks;
using FRA.Data.Models;
using FRA.Data.View;

namespace FRA.Data.Abstract
{
    public interface IRiskAssessmentRepository
    {
        Task<IEnumerable<RiskAssessmentView>> GetRiskAssessmentAsync(int pageNumber, int pageSize, int sortExpression, SortDirection sortDirection, string searchPhrase);
        int GetTotalNumberOfAssemment();
        Task<RiskAssessment> FindByIdAsync(string dataId);
        Task<IEnumerable<RiskAssessmentView>> GetAllRecords();
        Task<OperationResult> AddRiskAsync(RiskAssessmentView data);
        Task<OperationResult> AddRiskDetailScoreAsync(RiskDetailScoreView data);
        Task<IEnumerable<RiskDetailScoreView>> GetRiskDetailScoreRecordsByRiskId(int riskAssessmentId);        
        Task<IEnumerable<RiskSectionScoreView>> GetRiskSectionScoreRecordsByRiskId(int riskAssessmentId, int riskDetailId);
        Task<IEnumerable<RiskGuidelinesScoreView>> GetRiskGuidelinesScoreViewByRiskId(int riskAssessmentId, int riskDetailsId, int riskSectionScoreId);
        Task<IEnumerable<RiskParticipantsScoreView>> GetRiskParticipantsScoreRecordsById(int riskGuidelinesScoreId, int riskSectionScoreId, int riskDetailsId);
        Task<OperationResult> UpdateRiskDetailScoreAsync(RiskDetailScoreView data);
        Task<OperationResult> UpdateRiskSectionScoreAsync(RiskSectionScoreView data);
        Task<OperationResult> UpdateRiskParticipantsScoreAsync(RiskParticipantsScoreView data);
        Task<OperationResult> UpdateRiskGuidelinesScoreAsync(RiskGuidelinesScoreView data);
        Task<OperationResult> UpdateRiskGuidelinesScoreAsync(string riskGuidelinesScoreId, string comment);
    }
}
