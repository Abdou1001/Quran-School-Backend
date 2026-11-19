using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly QuranSchoolContext _context;

        public AttendanceService(QuranSchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Attendance>> GetAllAttendances()
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .ToListAsync();
        }

        public async Task<Attendance?> GetAttendanceById(int id)
        {
            return await _context.Attendances
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Attendance> AddAttendance(Attendance attendance)
        {
            attendance.Date = attendance.Date.Date;

            bool exists = await _context.Attendances.AnyAsync(a =>
                a.StudentId == attendance.StudentId &&
                a.Date == attendance.Date);

            if (exists)
                throw new InvalidOperationException(
                    "تم تسجيل حضور هذا الطالب في هذا اليوم مسبقًا.");

            _context.Attendances.Add(attendance);

            await _context.SaveChangesAsync();

            return attendance;
        }

        public async Task<bool> UpdateAttendance(int id, Attendance attendance)
        {
            var existing = await _context.Attendances.FindAsync(id);

            if (existing == null)
                return false;

            attendance.Date = attendance.Date.Date;

            bool duplicate = await _context.Attendances.AnyAsync(a =>
                a.Id != id &&
                a.StudentId == attendance.StudentId &&
                a.Date == attendance.Date);

            if (duplicate)
                throw new InvalidOperationException(
                    "يوجد سجل حضور لهذا الطالب في هذا اليوم.");

            existing.Date = attendance.Date;
            existing.IsPresent = attendance.IsPresent;
            existing.Notes = attendance.Notes;
            existing.StudentId = attendance.StudentId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAttendance(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}