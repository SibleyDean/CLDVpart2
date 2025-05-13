using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eventEasefour.Models
{
    public class Bookings
    {
        [Key]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Event is required")]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        public int VenueId { get; set; }

        public string? ImageUrl { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Booking Date")]
        [Required(ErrorMessage = "Booking date is required")]
        public DateTime BookingDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "Image File")]
        public IFormFile? ImageFile { get; set; }

        // Navigation Properties
        public virtual Event Event { get; set; }
        public virtual Venue Venue { get; set; }
    }
}