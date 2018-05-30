using System.ComponentModel.DataAnnotations;

namespace Kastra.Web.Models.Install
{
    public class IndexViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Database server")]
        public string DatabaseServer { get; set; }

        [Display(Name = "Database name")]
        public string DatabaseName { get; set; }

        [Display(Name = "Database login")]
        public string DatabaseLogin { get; set; }

        [Display(Name = "Database password")]
        [DataType(DataType.Password)]
        public string DatabasePassword { get; set; }

        [Display(Name = "Use integrated security")]
        public bool IntegratedSecurity { get; set; }
    }
}
