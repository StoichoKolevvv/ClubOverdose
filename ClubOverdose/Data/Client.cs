using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace ClubOverdose.Data
{
    public class Client : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

    }
}
