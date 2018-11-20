using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FRA.Web.Areas.Administration.Models
{
    public class UserViewModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Orginization { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please provide a valid email address")]
        [Remote("validate-email-address", "account", "")]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool LockoutEnabled { get; set; }
        public string Password { get; set; }
    }
}
