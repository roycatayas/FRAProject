using System;
using System.Collections.Generic;
using System.Text;

namespace FRA.Data.Models
{
    public class Document
    {
        public string DocId { get; set; }
        public string RiskAssessmentID { get; set; }
        public string DocumentName { get; set; }
        public string FileName { get; set; }
        public string FTPLink { get; set; }
        public string DocumentGUID { get; set; }
        public string DownloadLocation { get; set; }
    }
}
