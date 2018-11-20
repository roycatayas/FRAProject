﻿using System.Collections.Generic;

namespace FRA.Data.View
{
    public class RiskTemplateView
    {
        public string DataId { get; set; }
        public string CategoryIndex { get; set; }
        public string SectionIndex { get; set; }
        public string TempNumber { get; set; }
        public string Questions { get; set; }
        public string ControlGuidelines { get; set; }
        public string Impact { get; set; }
        public Dictionary<int, string> CategoryList { get; set; }
        public Dictionary<int, string> SectionList { get; set; }
    }
}
