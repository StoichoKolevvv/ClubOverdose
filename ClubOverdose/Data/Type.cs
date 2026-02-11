namespace ClubOverdose.Data
{
    public class Type
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Drink> Drinks { get; set; }

    }
}
