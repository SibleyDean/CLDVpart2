using System.ComponentModel.DataAnnotations;

namespace eventEasefour.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, ErrorMessage = "Event name cannot exceed 100 characters")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Event date is required")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Venue is required")]
        public int VenueId { get; set; }

        // ✅ Navigation Properties
        public virtual Venue Venue { get; set; } // An Event is linked to one Venue
        public virtual ICollection<Bookings>? Bookings { get; set; } = new List<Bookings>();
    }
}
