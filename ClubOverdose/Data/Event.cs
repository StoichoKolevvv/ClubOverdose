namespace ClubOverdose.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public ICollection<Reservation> Reservations { get; set; }

    }
}
