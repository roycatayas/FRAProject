using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskGuidelinesScoreView
    {
        public int RiskGuidelinesScoreID { get; set; }
        public int RiskDetailsID { get; set; }
        public int RiskSectionScoreID { get; set; }
        public int RiskAssessmentID { get; set; }
        public string GuidelinesNo { get; set; }
        public string Questions { get; set; }
        public string ControlGuidelines { get; set; }
        public double Impact { get; set; }
        public double MaturityEarned { get; set; }
        public string Comments { get; set; }
        public IEnumerable<RiskParticipantsScoreView> ListRiskParticipantsScore { get; set; }
    }
}
