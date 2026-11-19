
using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Data;
using QuranSchoolAPI.Interfaces;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Services
{
    public class StudentService : IStudentService
    {
        private readonly QuranSchoolContext _context;

        public StudentService(QuranSchoolContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students
                .Include(s => s.Halaqa)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Student?> GetStudentById(int id)
        {
            return await _context.Students
                .Include(s => s.Halaqa)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Student> AddStudent(Student student)
        {
            student.StudentNumber = student.StudentNumber?.Trim() ?? string.Empty;
            student.FullName = student.FullName.Trim();

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return student;
        }

        public async Task<bool> UpdateStudent(int id, Student student)
        {
            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null)
                return false;

            existingStudent.StudentNumber = student.StudentNumber?.Trim() ?? string.Empty;
            existingStudent.FullName = student.FullName.Trim();
            existingStudent.Gender = student.Gender;
            existingStudent.Phone = student.Phone;
            existingStudent.EnrollmentDate = student.EnrollmentDate;
            existingStudent.HalaqaId = student.HalaqaId;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
                return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
