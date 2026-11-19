using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Interfaces
{
    public interface IMonthlyReportService
    {
        Task<List<MonthlyReport>> GetAll();

        Task<MonthlyReport?> GetById(int id);

        Task<List<MonthlyReport>> GetByHalaqa(int halaqaId);

        Task<MonthlyReport> Create(MonthlyReport report);

        Task<MonthlyReport?> Update(int id, MonthlyReport report);

        Task<bool> Delete(int id);

        Task<bool> Submit(int id);

        Task<bool> Approve(int id, string adminNotes);

        Task<bool> ReturnToTeacher(int id, string adminNotes);
    }
}