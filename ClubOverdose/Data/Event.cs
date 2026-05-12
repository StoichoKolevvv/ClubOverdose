namespace ClubOverdose.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Event
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(2048)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31")]
        public DateTime EventDateTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Range(typeof(DateTime), "2020-01-01", "2100-12-31")]
        public DateTime LastUpdatedDate { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
