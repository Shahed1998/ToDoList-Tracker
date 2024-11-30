using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.General_Entities
{
    [Table("MenuConfigs")]
    public class MenuConfig
    {
        [Key]
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public string? Icon { get; set; }

        [Required]
        public string? Url { get; set; }

        [Required, StringLength(450)]
        public string? UserRoleId { get; set; }

    }
}
