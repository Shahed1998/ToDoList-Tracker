using System.ComponentModel.DataAnnotations;
using Web.Utilities;

namespace Web.Models.Business_Entities
{
    public class CreateTrackerViewModel
    {
        [Required]
        [DecimalPrecision(6, 3, ErrorMessage = "Eg. 123.456")]
        public decimal? Completed { get; set; }

        [Required]
        [DecimalPrecision(6, 3, ErrorMessage = "Eg. 123.456")]
        public decimal? Planned { get; set; }
    }
}
