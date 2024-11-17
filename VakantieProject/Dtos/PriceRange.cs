namespace VakantieProject.Dtos
{
    public class PriceRange
    {
        public Guid Id { get; set; }
        public float MaxAmount { get; set; }
        public float MinAmount { get; set; }
        public bool Percentage { get; set; }
        public float Boundary { get; set; }
    }
}
