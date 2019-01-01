using System.Collections.Generic;
using FRA.Data.Models;

namespace FRA.Data.View
{
    public class RiskDetailSectionScoreListView
    {
        public int ParticipantsNo { get; set; }
        public string RiskAssessmentID { get; set; }
        public IEnumerable<RiskDetailScoreView> ListRiskDetailScoreViews { get; set; }
        public IEnumerable<RiskSectionScoreView> ListRiskSectionScoreViews { get; set; }
        public IEnumerable<ContactPerson> ListContactPersons { get; set; }
        public IEnumerable<Document> ListDocuments { get; set; }
    }
}
