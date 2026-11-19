using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly QuranSchoolContext _context;

        public TeacherService(QuranSchoolContext context)
        {
            _context = context;
        }

        // عرض جميع المعلمين مع حلقاتهم
        public async Task<List<Teacher>> GetAllTeachers()
        {
            return await _context.Teachers
                .Include(t => t.Halaqa)
                .ToListAsync();
        }

        // عرض معلم محدد
        public async Task<Teacher?> GetTeacherById(int id)
        {
            return await _context.Teachers
                .Include(t => t.Halaqa)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // إضافة معلم
        public async Task<Teacher> AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);

            await _context.SaveChangesAsync();

            return teacher;
        }

        // تعديل بيانات معلم
        public async Task<bool> UpdateTeacher(int id, Teacher teacher)
        {
            var existingTeacher = await _context.Teachers
                .FindAsync(id);

            if (existingTeacher == null)
                return false;

            existingTeacher.FullName = teacher.FullName;
            existingTeacher.Phone = teacher.Phone;

            await _context.SaveChangesAsync();

            return true;
        }

        // حذف معلم
        public async Task<bool> DeleteTeacher(int id)
        {
            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher == null)
                return false;

            // منع حذف المعلم إذا كان مرتبطًا بحلقة
            bool hasHalaqa = await _context.Halaqas
                .AnyAsync(h => h.TeacherId == id);

            if (hasHalaqa)
                return false;

            _context.Teachers.Remove(teacher);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}