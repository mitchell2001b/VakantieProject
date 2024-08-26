using VakantieProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VakantieProject.Data
{
    public class HotelRepo : IHotelRepo
    {
        private readonly AppDbContext ctx;
        public HotelRepo(AppDbContext context)
        {
            ctx = context;
        }

        public async Task<Hotel> CreateHotel(Hotel hotel)
        {
            if (hotel == null)
            {
                throw new ArgumentNullException(nameof(hotel));
            }

            await ctx.Hotels.AddAsync(hotel);

            return hotel;
        }

        public async Task<bool> UpdateHotel(Hotel hotel)
        {
           ctx.Hotels.Update(hotel);

           return await SaveChanges();
        }
        public async Task<IEnumerable<Hotel>> GetAllHotels()
        {
            return await ctx.Hotels.ToListAsync();
        }

        public async Task<Hotel> GetHotel(Guid id)
        {
            return await ctx.Hotels.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> DeleteHotel(Guid id)
        {
            Hotel hotel = await ctx.Hotels.FirstOrDefaultAsync(p => p.Id == id);

            if(hotel == null)
            {
                throw new ArgumentNullException(nameof(hotel));
            }

            ctx.Hotels.Remove(hotel);
            return await SaveChanges();
        }
        public async Task<bool> SaveChanges()
        {
            return (await ctx.SaveChangesAsync() >= 0);
        }
    }
}

