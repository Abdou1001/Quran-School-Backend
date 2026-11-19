using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Interfaces
{
    public interface ITeacherService
    {
        Task<List<Teacher>> GetAllTeachers();
        Task<Teacher?> GetTeacherById(int id);
        Task<Teacher> AddTeacher(Teacher teacher);
        Task<bool> UpdateTeacher(int id, Teacher teacher);
        Task<bool> DeleteTeacher(int id);
    }
}