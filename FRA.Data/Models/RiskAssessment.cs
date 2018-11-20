using System;

namespace FRA.Data.Models
{
    public class RiskAssessment
    {
        public int RiskAssessmentID { get; set; }
        public string SubjectTitle { get; set; }
        public string Organization { get; set; }
        public string DocumentNo { get; set; }
        public string Owner { get; set; }
        public string ApproveBy { get; set; }
        public DateTime EntrerDate { get; set; }
        public string RevisionNo { get; set; }
        public string SurveyorName { get; set; }
        public string SurveyorNumber { get; set; }
        public string SurveyorEmail { get; set; }
        public string SiteLocation { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string ProvinceState { get; set; }
        public string PrimaryContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAdress { get; set; }
        public string URLAdress { get; set; }
        public int LocationID { get; set; }
        public int SurveyorID { get; set; }
    }
}
