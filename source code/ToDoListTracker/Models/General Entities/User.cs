using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.General_Entities
{
    [Table("Users")]
    public class User : IdentityUser
    {
        public string? Gender { get; set; }
    }
}
