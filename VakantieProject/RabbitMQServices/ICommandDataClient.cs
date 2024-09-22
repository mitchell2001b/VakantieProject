using VakantieProject.Dtos;

namespace VakantieProject.RabbitMQServices
{
    public interface ICommandDataClient
    {
        Task SendHotelToPriceGuard(HotelCreatedDto dto);
               
    }
}
