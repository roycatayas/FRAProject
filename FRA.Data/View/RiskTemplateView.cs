using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskTemplateView
    {
        public string DataId { get; set; }
        public string CategoryIndex { get; set; }
        public string CategoryName { get; set; }
        public string SectionIndex { get; set; }
        public string SectionName { get; set; }
        public string TempNumber { get; set; }
        public string Questions { get; set; }
        public string ControlGuidelines { get; set; }
        public string Impact { get; set; }
        public string FirstCategoryRecordId { get; set; }
        public string FirstSectionRecordId { get; set; }
        public Dictionary<string, string> CategoryList { get; set; }
        public Dictionary<string, string> SectionList { get; set; }
    }
}
