using Newtonsoft.Json;

namespace VakantieProject.Dtos
{
    public class HotelDto
    {
        [JsonProperty("id")]
        public string Id { get; set; } = "";

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        public HotelDto()
        {
            
        }

        public HotelDto(string id, string name, string price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public HotelDto(string name, string price)
        {                     
            Name = name;
            Price = price;
        }

    }
}
