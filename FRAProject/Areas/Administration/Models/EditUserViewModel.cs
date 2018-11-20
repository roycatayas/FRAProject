using System;
using System.ComponentModel.DataAnnotations;

namespace FRA.Web.Areas.Administration.Models
{
    public class EditUserViewModel : UserViewModelBase
    {
        [Required(AllowEmptyStrings = false)]
        public Guid Id { get; set; }
    }
}
