using System.Collections.Generic;
using System.Globalization;

namespace FRA.Data.View
{
    public class SectionView
    {        
        public string SectionID { get; set; }
        public string CategoryId { get; set; }        
        public string SectionName { get; set; }
        public string SelectedIndex { get; set; }
        public Dictionary<string, string> CategoryList { get; set; }        
    }
}
