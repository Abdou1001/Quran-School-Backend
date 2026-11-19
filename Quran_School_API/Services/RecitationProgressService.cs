using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class RecitationProgressService : IRecitationProgressService
    {
        private readonly QuranSchoolContext _context;

        public RecitationProgressService(QuranSchoolContext context)
        {
            _context = context;
        }

        public async Task<List<RecitationProgress>> GetAllProgress()
        {
            return await _context.RecitationProgresses
                .Include(p => p.Student)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<RecitationProgress?> GetProgressById(int id)
        {
            return await _context.RecitationProgresses
                .Include(p => p.Student)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<RecitationProgress> AddProgress(RecitationProgress progress)
        {
            progress.Date = progress.Date.Date;

            // التأكد من وجود الطالب
            bool studentExists = await _context.Students
                .AnyAsync(s => s.Id == progress.StudentId);

            if (!studentExists)
                throw new InvalidOperationException("الطالب غير موجود.");

            // منع التكرار
            bool duplicate = await _context.RecitationProgresses
                .AnyAsync(p =>
                    p.StudentId == progress.StudentId &&
                    p.Date == progress.Date);

            if (duplicate)
                throw new InvalidOperationException(
                    "تم تسجيل الحفظ والمراجعة لهذا الطالب في هذا اليوم مسبقاً.");

            _context.RecitationProgresses.Add(progress);

            await _context.SaveChangesAsync();

            return progress;
        }

        public async Task<bool> UpdateProgress(int id, RecitationProgress progress)
        {
            var existing = await _context.RecitationProgresses
                .FindAsync(id);

            if (existing == null)
                return false;

            progress.Date = progress.Date.Date;

            bool studentExists = await _context.Students
                .AnyAsync(s => s.Id == progress.StudentId);

            if (!studentExists)
                throw new InvalidOperationException("الطالب غير موجود.");

            bool duplicate = await _context.RecitationProgresses
                .AnyAsync(p =>
                    p.Id != id &&
                    p.StudentId == progress.StudentId &&
                    p.Date == progress.Date);

            if (duplicate)
                throw new InvalidOperationException(
                    "يوجد تسجيل مسبق لهذا الطالب في التاريخ المحدد.");

            existing.Date = progress.Date;
            existing.MemorizedPages = progress.MemorizedPages;
            existing.ReviewPages = progress.ReviewPages;
            existing.Notes = progress.Notes;
            existing.StudentId = progress.StudentId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteProgress(int id)
        {
            var progress = await _context.RecitationProgresses
                .FindAsync(id);

            if (progress == null)
                return false;

            _context.RecitationProgresses.Remove(progress);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}