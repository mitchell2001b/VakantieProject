﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VakantieProject.Data;
using VakantieProject.Dtos;
using VakantieProject.Models;
using VakantieProject.RabbitMQServices;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VakantieProject.Controllers
{

    [Route("api/Hotel")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelRepo hotelRepo;

        private readonly IMessageBusClient messageBusClient;

        public HotelController(IHotelRepo repo, IMessageBusClient messageClient) 
        { 
            hotelRepo = repo;
            messageBusClient = messageClient;
        }


       



        // GET: api/hotel/select
        [HttpGet("select")]
        public async Task<IEnumerable<HotelDto>> GetHotel()
        {
            List<Hotel> hotels = (List<Hotel>)await hotelRepo.GetAllHotels();

            // Convert each Hotel entity to HotelDto
            IEnumerable<HotelDto> hotelDtos = hotels.Select(h => new HotelDto(h.Id.ToString(), h.Name, h.Price));

            var influxUrl = "http://localhost:8086";
            var token = "devtoken";
            var org = "devorg";
            var bucket = "devbucket";
            using (var client = InfluxDBClientFactory.Create(influxUrl, token))
            {

                var hotel = new Hotel("Hotel California", "199.99");

                
                var point = PointData
                    .Measurement("hotels")                   
                    .Tag("id", hotel.Id.ToString())         
                    .Field("name", hotel.Name)                
                    .Field("price", hotel.Price)               
                    .Timestamp(DateTime.UtcNow, WritePrecision.Ns);  

                
                var writeApiAsync = client.GetWriteApiAsync();
                await writeApiAsync.WritePointAsync(point, bucket, org);
                Console.WriteLine("Hotel data written to InfluxDB");


            }

            return hotelDtos;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddHotel([FromBody] HotelDto hotelDto)
        {
            if (hotelDto == null)
            {
                return BadRequest("Hotel data is null.");
            }
           
            Hotel hotel = new Hotel(hotelDto.Name, hotelDto.Price);
            Hotel createdHotel = null;
            try
            {
               createdHotel = await hotelRepo.CreateHotel(hotel);
                await hotelRepo.SaveChanges();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            HotelCreatedDto dto = new HotelCreatedDto(createdHotel.Id.ToString(), createdHotel.Name);
            
            messageBusClient.PublishNewCreatedHotel(dto);

            
            return CreatedAtAction(nameof(GetHotel), new { id = hotel.Id }, hotelDto);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> EditHotel(Guid id, [FromBody] HotelDto hotelDto)
        {
            if (hotelDto == null)
            {
                return BadRequest("Hotel data is null.");
            }

            var existingHotel = await hotelRepo.GetHotel(id);

            if (existingHotel == null)
            {
                return NotFound("Hotel not found.");
            };

            existingHotel.Name = hotelDto.Name;
            existingHotel.Price = hotelDto.Price;

            await hotelRepo.UpdateHotel(existingHotel);
          

            return NoContent();
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteHotel(Guid id)
        {
           
            if(await hotelRepo.GetHotel(id) == null)
            {
                return NotFound("Hotel not found.");
            };

            await hotelRepo.DeleteHotel(id);
            return NoContent();
        }

        [HttpGet("heyo")]
        public async Task<IActionResult> TestStored()
        {
            SqlPriceRangeDal dal = new SqlPriceRangeDal("Server=localhost;Database=hoteldb;Trusted_Connection=True;");
            PriceRange range = new PriceRange();
            range.CreatedAt = DateTime.Now;
            range.Percentage = false;
            range.Boundary = 50;
            range.MaxAmount = 500;
            range.MinAmount = 100;
            await dal.SavePriceRange(range);
            return Ok("ok");

        }
        /*// POST api/<HotelController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }*/
        [HttpGet("heyoApple")]
        public async Task<IActionResult> TestStored2()
        {
            dashboardService s = new dashboardService();
            Apple testApple = new Apple
            {
                Id = Guid.NewGuid(),
                Name = "Golden Apple",
                Description = "A rare and delicious golden apple.",
                Price = 12.99f,
                CreatedAt = DateTime.Now,
                fruitvalue = new fruitvalue
                {
                    CalculatedDifference = 2.5f,
                    OldValue = 10.49f,
                    NewValue = 12.99f
                }
            };
            //await  s.CreateAppleIndexWithMappingAsync();

            await s.CreateApples(testApple);

            return Ok("ok");
        }

    }
}
