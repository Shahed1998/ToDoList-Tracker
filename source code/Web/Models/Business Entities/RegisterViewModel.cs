using System.ComponentModel.DataAnnotations;

namespace Web.Models.Business_Entities
{
    public class RegisterViewModel
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and Confirm password do not match ")]
        public string? ConfirmPassword { get; set; }
    }
}
