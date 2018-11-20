using System;
using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskAssessmentView 
    {
        public int RiskAssessmentID { get; set; }
        public string SubjectTitle { get; set; }
        public string Organization { get; set; }
        public DateTime EntryDate { get; set; }
        public string Owner { get; set; }
        public string DocumentNo { get; set; }
        public string ApprovedBy { get; set; }
        public string RevisionNo { get; set; }
        public string SurveyorName { get; set; }
        public string SurveyorTelephone { get; set; }
        public string SurveyorEmail { get; set; }
        public DateTime SurveyDate { get; set; }
        public string SiteName { get; set; }
        public string SiteCountry { get; set; }
        public string SiteAdress { get; set; }
        public string SiteStateProvince { get; set; }
        public string ContactPersonName { get; set; }
        public string ContactPersonEmail { get; set; }
        public string ContactPersonTelephone { get; set; }
        public string ContactPersonFaxNumber { get; set; }
        public string ContactPersonWebsiteUrl { get; set; }
        public IEnumerable<RiskDetailScoreView> ListRiskDetailScoreViews { get; set; }
    }
}
