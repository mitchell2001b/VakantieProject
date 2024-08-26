using VakantieProject.Dtos;

namespace VakantieProject.RabbitMQServices
{
    public interface IMessageBusClient
    {
        void PublishNewCreatedHotel(HotelCreatedDto hotelCreatedDto);
    }
}
