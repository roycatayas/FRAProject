namespace FRA.Data.Models
{
    public class RiskTemplate
    {
        public int TemplateID { get; set; }
        public string SectionID { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string SectionName { get; set; }
        public string TempNumber { get; set; }
        public string Questions { get; set; }
        public string ControlGuidelines { get; set; }
        public string Impact { get; set; }
    }
}
