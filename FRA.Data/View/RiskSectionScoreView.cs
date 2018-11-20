using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskSectionScoreView
    {
        public int RiskSectionScoreID { get; set; }
        public int RiskDetailsID { get; set; }
        public int RiskAssessmentID { get; set; }
        public string SectionName { get; set; }
        public double Maturity { get; set; }
        public string Efficiency { get; set; }
        public string RiskLevel { get; set; }
        public int TotalRecordNo { get; set; }
        public int MinRecordNo { get; set; }
        public int MaxRecordNo { get; set; }
        public IEnumerable<RiskGuidelinesScoreView> ListRiskGuidelinesScore { get; set; }
    }
}
