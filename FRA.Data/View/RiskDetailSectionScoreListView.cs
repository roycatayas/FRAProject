using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskDetailSectionScoreListView
    {
        public int ParticipantsNo { get; set; }
        public IEnumerable<RiskDetailScoreView> ListRiskDetailScoreViews { get; set; }
        public IEnumerable<RiskSectionScoreView> ListRiskSectionScoreViews { get; set; }
    }
}
