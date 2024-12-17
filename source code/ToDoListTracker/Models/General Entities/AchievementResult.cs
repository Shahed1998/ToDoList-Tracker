using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Models.General_Entities
{
    [Table("Achievement")]
    public class AchievementResult
    {
        [Key]
        public int Id { get; set; } 
        public decimal? Result { get; set; }
        public string? UserId { get; set; }
    }
}
