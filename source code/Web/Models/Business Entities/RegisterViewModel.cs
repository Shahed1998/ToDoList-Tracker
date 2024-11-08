using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Web.Models.Business_Entities
{
    public class RegisterViewModel
    {
        [Required]
        [Remote(action: "IsUsernameInUse", controller: "User")]
        public string? Username { get; set; }

        [Required]
        [Remote(action: "IsEmailInUse", controller: "User")]
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
