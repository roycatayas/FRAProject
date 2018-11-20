using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Models.DataTables
{
    public class Search
    {
        public string Value { get; set; }

        [FromForm(Name = "regex")]
        public bool RegularExpression { get; set; }
    }
}
