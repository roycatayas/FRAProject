﻿using System.ComponentModel.DataAnnotations;

namespace FRA.Web.Models.Account
{
    public class AccountViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Please provide a valid email address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please specify a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
