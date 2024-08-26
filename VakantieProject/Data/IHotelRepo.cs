using VakantieProject.Models;

namespace VakantieProject.Data
{
    public interface IHotelRepo
    {
        Task<Hotel> CreateHotel(Hotel hotel);
        Task<IEnumerable<Hotel>> GetAllHotels();
        Task<Hotel> GetHotel(Guid id);
        Task<bool> SaveChanges();
        Task<bool> UpdateHotel(Hotel Hotel);
        Task<bool> DeleteHotel(Guid id);
    }
}
