namespace ClubOverdose.Data
{
    public class Reservation
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Client Client { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public int NumberOfGuests { get; set; }
        public DateTime ReservationDate { get; set; }

    }
}
