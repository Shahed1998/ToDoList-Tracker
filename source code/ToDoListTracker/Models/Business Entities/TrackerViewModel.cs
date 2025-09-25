using System.ComponentModel.DataAnnotations;
using Web.Utilities;
using Web.Models.General_Entities;

namespace Web.Models.Business_Entities
{
    public class TrackerViewModel
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [DecimalPrecision(6, 3, ErrorMessage = "Eg. 123.456")] 
        public decimal? Completed { get; set; }

        [Required]
        [DecimalPrecision(6, 3, ErrorMessage = "Eg. 123.456")] 
        public decimal? Planned { get; set; }

        [Required]
        [DecimalPrecision(6, 3, ErrorMessage = "Eg. 123.456")] 
        public decimal? Percentage { get; set; }

        public DateTime? Date { get; set; }

        public string? UserId { get; set; }

        private bool? _isFlagged;

        public bool? IsFlagged { get { return _isFlagged; } set { _isFlagged = value; } }

        public bool IsFlaggedTmp { get { return _isFlagged ?? false; } set { IsFlagged = value;  } } 

        #region Mapping
        public static explicit operator TrackerViewModel(Tracker t) => new TrackerViewModel()
        {
            Id = t.Id,
            Completed = t.Completed,
            Planned = t.Planned,
            Percentage = t.Percentage,
            Date = t.Date,
            UserId = t.UserId,
            IsFlagged = t.IsFlagged 
    };

        public static explicit operator Tracker(TrackerViewModel t) => new Tracker()
        {
            Completed = t.Completed,
            Planned = t.Planned,
            Date = t.Date,
            Id = t.Id,
            UserId = t.UserId,
            IsFlagged = t.IsFlagged
        };
        #endregion
    }
}
