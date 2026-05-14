namespace ClubOverdose.Models
{
    public class AdminDashboardViewModel
    {
        public int TotalEvents { get; set; }

        public int TotalReservations { get; set; }

        public int TotalDrinks { get; set; }

        public int TotalMenus { get; set; }

        public int TotalTypes { get; set; }

        public int TotalUsers { get; set; }

        public int UpcomingEvents { get; set; }

        public int UpcomingEventReservations { get; set; }

        public IReadOnlyList<AdminRecentReservationViewModel> RecentReservations { get; set; } = new List<AdminRecentReservationViewModel>();

        public IReadOnlyList<AdminUpcomingEventViewModel> UpcomingEventItems { get; set; } = new List<AdminUpcomingEventViewModel>();
    }

    public class AdminRecentReservationViewModel
    {
        public int Id { get; set; }

        public string ClientName { get; set; } = string.Empty;

        public string EventName { get; set; } = string.Empty;

        public DateTime ReservationDate { get; set; }

        public int NumberOfGuests { get; set; }
    }

    public class AdminUpcomingEventViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ImageUrl { get; set; } = string.Empty;

        public DateTime EventDateTime { get; set; }

        public int ReservationCount { get; set; }
    }
}
