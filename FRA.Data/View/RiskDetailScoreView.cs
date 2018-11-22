using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskDetailScoreView
    {
        public int RiskDetailsID { get; set; }
        public int RiskAssessmentID { get; set; }
        public double Maturity { get; set; }
        public int Efficiency { get; set; }
        public string RiskLevel { get; set; }
        public string CategoryName { get; set; }
        public int ParticipantsNo { get; set; }
        public string CategoryID { get; set; }
        public int SectionDataCount { get; set; }
        public int MinRecordNo { get; set; }
        public int MaxRecordNo { get; set; }
        public List<string> CurrentCategory { get; set; }
        public IEnumerable<RiskSectionScoreView> ListRiskSectionScore { get; set; }        
    }
}
