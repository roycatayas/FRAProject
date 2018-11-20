using System.Collections.Generic;

namespace FRA.Data.View
{
    public class SectionView
    {        
        public string DataId { get; set; }
        public string SectionName { get; set; }
        public string SelectedIndex { get; set; }
        public Dictionary<int, string> CategoryList { get; set; }        
    }
}
