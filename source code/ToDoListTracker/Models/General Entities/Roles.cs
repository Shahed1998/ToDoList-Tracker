using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.General_Entities
{
    [Table("Roles")]
    public class CreateRole
    {
        [StringLength(256)]
        public string? Name { get; set; }
    }
}
