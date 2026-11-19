
using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class MonthlyReportService : IMonthlyReportService
    {
        private readonly QuranSchoolContext _context;

        public MonthlyReportService(QuranSchoolContext context)
        {
            _context = context;
        }

        public async Task<List<MonthlyReport>> GetAll()
        {
            return await _context.MonthlyReports
                .Include(r => r.Halaqa)
                .OrderByDescending(r => r.Year)
                .ThenByDescending(r => r.Month)
                .ToListAsync();
        }

        public async Task<MonthlyReport?> GetById(int id)
        {
            return await _context.MonthlyReports
                .Include(r => r.Halaqa)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<MonthlyReport>> GetByHalaqa(int halaqaId)
        {
            return await _context.MonthlyReports
                .Include(r => r.Halaqa)
                .Where(r => r.HalaqaId == halaqaId)
                .OrderByDescending(r => r.Year)
                .ThenByDescending(r => r.Month)
                .ToListAsync();
        }

        public async Task<MonthlyReport> Create(MonthlyReport report)
        {
            bool halaqaExists = await _context.Halaqas
                .AnyAsync(h => h.Id == report.HalaqaId);

            if (!halaqaExists)
                throw new InvalidOperationException("الحلقة غير موجودة.");

            bool alreadyExists = await _context.MonthlyReports
                .AnyAsync(r =>
                    r.HalaqaId == report.HalaqaId &&
                    r.Month == report.Month &&
                    r.Year == report.Year);

            if (alreadyExists)
                throw new InvalidOperationException(
                    "يوجد تقرير لهذه الحلقة في الشهر والسنة المحددين.");

            report.Id = 0;
            report.Status = MonthlyReportStatus.Draft;
            report.CreatedAt = DateTime.Now;
            report.SubmittedAt = null;
            report.ReviewedAt = null;
            report.AdminNotes = string.Empty;

            _context.MonthlyReports.Add(report);
            await _context.SaveChangesAsync();

            return await GetById(report.Id) ?? report;
        }

        public async Task<MonthlyReport?> Update(int id, MonthlyReport report)
        {
            var existing = await _context.MonthlyReports.FindAsync(id);

            if (existing == null)
                return null;

            if (existing.Status != MonthlyReportStatus.Draft &&
                existing.Status != MonthlyReportStatus.Returned)
                throw new InvalidOperationException(
                    "لا يمكن تعديل التقرير في حالته الحالية.");

            bool duplicate = await _context.MonthlyReports.AnyAsync(r =>
                r.Id != id &&
                r.HalaqaId == report.HalaqaId &&
                r.Month == report.Month &&
                r.Year == report.Year);

            if (duplicate)
                throw new InvalidOperationException(
                    "يوجد تقرير آخر لهذه الحلقة في الفترة المحددة.");

            bool halaqaExists = await _context.Halaqas
                .AnyAsync(h => h.Id == report.HalaqaId);

            if (!halaqaExists)
                throw new InvalidOperationException("الحلقة غير موجودة.");

            existing.HalaqaId = report.HalaqaId;
            existing.Month = report.Month;
            existing.Year = report.Year;
            existing.TeacherNotes = report.TeacherNotes;

            await _context.SaveChangesAsync();

            return await GetById(id);
        }

        public async Task<bool> Delete(int id)
        {
            var report = await _context.MonthlyReports.FindAsync(id);

            if (report == null)
                return false;

            if (report.Status != MonthlyReportStatus.Draft)
                throw new InvalidOperationException(
                    "لا يمكن حذف تقرير تم إرساله أو اعتماده.");

            _context.MonthlyReports.Remove(report);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Submit(int id)
        {
            var report = await _context.MonthlyReports.FindAsync(id);

            if (report == null)
                return false;

            if (report.Status != MonthlyReportStatus.Draft &&
                report.Status != MonthlyReportStatus.Returned)
                throw new InvalidOperationException(
                    "لا يمكن إرسال التقرير في حالته الحالية.");

            report.Status = MonthlyReportStatus.Submitted;
            report.SubmittedAt = DateTime.Now;
            report.ReviewedAt = null;
            report.AdminNotes = string.Empty;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Approve(int id, string adminNotes)
        {
            var report = await _context.MonthlyReports.FindAsync(id);

            if (report == null)
                return false;

            if (report.Status != MonthlyReportStatus.Submitted)
                throw new InvalidOperationException(
                    "لا يمكن اعتماد تقرير لم يتم إرساله.");

            report.Status = MonthlyReportStatus.Approved;
            report.AdminNotes = adminNotes;
            report.ReviewedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ReturnToTeacher(int id, string adminNotes)
        {
            var report = await _context.MonthlyReports.FindAsync(id);

            if (report == null)
                return false;

            if (report.Status != MonthlyReportStatus.Submitted)
                throw new InvalidOperationException(
                    "لا يمكن إعادة تقرير لم يتم إرساله.");

            if (string.IsNullOrWhiteSpace(adminNotes))
                throw new InvalidOperationException(
                    "يرجى كتابة سبب إعادة التقرير.");

            report.Status = MonthlyReportStatus.Returned;
            report.AdminNotes = adminNotes;
            report.ReviewedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
