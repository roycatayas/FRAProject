using System.Collections.Generic;
using FRA.Data.Models;

namespace FRA.Web.Areas.Administration.Models
{
    public class UserListView
    {
        public IEnumerable<User> ListUser { get; set; }
    }
}
