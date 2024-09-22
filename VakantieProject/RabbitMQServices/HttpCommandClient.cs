using VakantieProject.Dtos;

namespace VakantieProject.RabbitMQServices
{
    public class HttpCommandClient : ICommandDataClient
    {
        private readonly HttpClient httpClient;

        public HttpCommandClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public Task SendHotelToPriceGuard(HotelCreatedDto dto)
        {
            return null;
                
        }
    }
}
