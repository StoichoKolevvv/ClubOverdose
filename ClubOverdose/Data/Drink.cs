namespace ClubOverdose.Data
{
    public class Drink
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TypeId { get; set; }
        public Type Types { get; set; }

        public decimal Price { get; set; }
        public int Volume { get; set; }

        public DateTime DateAdded { get; set; }


    }
}
