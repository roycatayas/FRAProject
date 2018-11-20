using System.Collections.Generic;
using FRA.Data.Models;

namespace FRA.Data.View
{
    public class RiskAssessmentListView
    {
        public IEnumerable<RiskAssessmentView> ListRiskAssessment { get; set; }
        public IEnumerable<Catergory> ListCategory { get; set; }       
    }
}
