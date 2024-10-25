using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Models.Business_Entities;
using Web.Models.CustomValidationAttribute;

namespace Web.Models.General_Entities
{
    [Table("Tracker")]
    public class Tracker
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [DecimalPrecision(6, 3)] // Custom attribute specifying precision and scale
        public decimal? Completed { get; set; }

        [Required]
        [DecimalPrecision(6, 3)] // Custom attribute specifying precision and scale
        public decimal? Planned { get; set; }

        [Required]
        [DecimalPrecision(6, 3)] // Custom attribute specifying precision and scale
        public decimal? Percentage { get; set; }

        public DateTime? Date { get; set; }
    }
}
