namespace ClubOverdose.Data
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Drink
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }
        public Type Types { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int MenuId { get; set; }
        public Menu Menu { get; set; } = null!;

        [Range(0.01, 100000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(1, 10000)]
        public int Volume { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateAdded { get; set; }


    }
}
