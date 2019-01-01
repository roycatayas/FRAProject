using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FRA.Data.Models;

namespace FRA.Data.View
{
    public class RiskAssessmentView 
    {
        public int RiskAssessmentID { get; set; }
        public string SubjectTitle { get; set; }
        public string Organization { get; set; }        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EntryDate { get; set; }
        public string Owner { get; set; }
        public string DocumentNo { get; set; }
        public string ApprovedBy { get; set; }
        public string RevisionNo { get; set; }
        public string SurveyorName { get; set; }
        public string SurveyorTelephone { get; set; }
        public string SurveyorEmail { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
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
        public string Country { get; set; }
        public string Address { get; set; }
        public string ProvinceState { get; set; }
        public string SurveyorNumber { get; set; }
        public string PrimaryContactName { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string EmailAdress { get; set; }
        public string URLAdress { get; set; }
        public IEnumerable<RiskDetailScoreView> ListRiskDetailScoreViews { get; set; }
        public IEnumerable<ContactPerson> ListContactPersons { get; set; }
        public IEnumerable<Document> ListDocuments { get; set; }
    }
}
