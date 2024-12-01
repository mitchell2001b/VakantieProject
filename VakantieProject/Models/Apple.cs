namespace VakantieProject.Models
{
    public class Apple
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public DateTime CreatedAt {  get ; set; }

        public fruitvalue fruitvalue { get; set; }  
    }
}
