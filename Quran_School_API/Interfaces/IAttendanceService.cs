using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAllAttendances();
        Task<Attendance?> GetAttendanceById(int id);
        Task<Attendance> AddAttendance(Attendance attendance);
        Task<bool> UpdateAttendance(int id, Attendance attendance);
        Task<bool> DeleteAttendance(int id);
    }
}