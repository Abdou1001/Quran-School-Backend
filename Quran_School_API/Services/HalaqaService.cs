using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class HalaqaService : IHalaqaService
    {
        private readonly QuranSchoolContext _context;

        public HalaqaService(QuranSchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Halaqa>> GetAllHalaqas()
        {
            return await _context.Halaqas
                .Include(h => h.Teacher)
                .ToListAsync();
        }

        public async Task<Halaqa?> GetHalaqaById(int id)
        {
            return await _context.Halaqas
                .Include(h => h.Teacher)
                .FirstOrDefaultAsync(h => h.Id == id);
        }

        public async Task<Halaqa> AddHalaqa(Halaqa halaqa)
        {
            _context.Halaqas.Add(halaqa);
            await _context.SaveChangesAsync();
            return halaqa;
        }

        public async Task<bool> UpdateHalaqa(int id, Halaqa halaqa)
        {
            var existingHalaqa = await _context.Halaqas.FindAsync(id);

            if (existingHalaqa == null)
                return false;

            existingHalaqa.Name = halaqa.Name;
            existingHalaqa.Level = halaqa.Level;
            existingHalaqa.TeacherId = halaqa.TeacherId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteHalaqa(int id)
        {
            var halaqa = await _context.Halaqas.FindAsync(id);

            if (halaqa == null)
                return false;

            _context.Halaqas.Remove(halaqa);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}