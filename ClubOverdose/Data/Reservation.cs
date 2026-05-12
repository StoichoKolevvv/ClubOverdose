namespace ClubOverdose.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Reservation
    {
        public int Id { get; set; }
        [Required]
        public string ClientId { get; set; } = string.Empty;
        public Client Client { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;

        [Range(1, 100)]
        public int NumberOfGuests { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ReservationDate { get; set; }

    }
}
