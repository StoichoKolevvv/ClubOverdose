using ClubOverdose.Data;

namespace ClubOverdose.Models
{
    public class ReservationsIndexViewModel
    {
        public bool IsSuperAdmin { get; set; }

        public int? SelectedEventId { get; set; }

        public string? SelectedEventName { get; set; }

        public DateTime? SelectedEventDateTime { get; set; }

        public IReadOnlyList<Reservation> Reservations { get; set; } = new List<Reservation>();

        public IReadOnlyList<EventReservationSummaryViewModel> Events { get; set; } = new List<EventReservationSummaryViewModel>();
    }

    public class EventReservationSummaryViewModel
    {
        public Event Event { get; set; } = null!;

        public int ReservationCount { get; set; }
    }
}
