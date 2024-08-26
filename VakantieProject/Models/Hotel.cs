using System.ComponentModel.DataAnnotations;
namespace VakantieProject.Models
{
    public class Hotel
    {
        [Key]
        [Required]
        public Guid Id { get; private set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Price { get; set; }

        public Hotel()
        {
            Id = Guid.NewGuid();
        }
        public Hotel(Guid id, string name, string price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public Hotel(string name, string price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }

        
        
        public string toString()
        {
            return "Project{" +
                    "Id=" + Id +
                    ", Name='" + Name + '\'' +
                    ", Price'" + Price + '\'' +                    
                    '}';
        }
    }
}
