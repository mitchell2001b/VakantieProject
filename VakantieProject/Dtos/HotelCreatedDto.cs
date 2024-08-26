namespace VakantieProject.Dtos
{
    public class HotelCreatedDto
    {
       
        public string Id { get; set; }
        
        public string Name { get; set; }
              
        public HotelCreatedDto()
        {

        }

        public HotelCreatedDto(string id, string name)
        {
            Id = id;
            Name = name;
            
        }
       
    }
}
