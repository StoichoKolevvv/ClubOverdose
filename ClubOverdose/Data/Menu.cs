namespace ClubOverdose.Data
{
    using System.ComponentModel.DataAnnotations;

    public class Menu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Drink> Drinks { get; set; } = new List<Drink>();
    }
}
