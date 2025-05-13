using System;
using System.ComponentModel.DataAnnotations;

namespace eventEasefour.Models
{
    public class BookingDetails
    {
        [Display(Name = "Booking ID")]
        public int BookingId { get; set; }

        [Display(Name = "Booking Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime BookingDate { get; set; }

        // ✅ Event Information
        [Display(Name = "Event")]
        [Required(ErrorMessage = "Event name is required.")]
        [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters.")]
        public string EventName { get; set; } = string.Empty;

        [Display(Name = "Event Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}")]
        public DateTime EventDate { get; set; }

        [Display(Name = "Description")]
        public string? EventDescription { get; set; } = string.Empty;

        // ✅ Venue Information
        [Display(Name = "Venue")]
        [Required(ErrorMessage = "Venue name is required.")]
        [StringLength(100, ErrorMessage = "Venue name cannot exceed 100 characters.")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "Location")]
        public string? VenueLocation { get; set; } = string.Empty;

        [Display(Name = "Capacity")]
        [Range(1, 10000, ErrorMessage = "Capacity must be between 1-10,000.")]
        public int VenueCapacity { get; set; }

        [Display(Name = "Image")]
        public string? VenueImageUrl { get; set; } = string.Empty;

        // ✅ Computed Properties
        [Display(Name = "Status")]
        public string EventStatus
        {
            get
            {
                if (EventDate < DateTime.UtcNow)
                    return "Completed";
                if ((EventDate.Date - DateTime.UtcNow.Date).Days <= 7)
                    return "Soon";
                return "Upcoming";
            }
        }

        [Display(Name = "Days Until Event")]
        public int DaysUntilEvent => Math.Max(0, (EventDate.Date - DateTime.UtcNow.Date).Days);
    }
}
