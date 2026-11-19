using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Interfaces
{
    public interface IHalaqaService
    {
        Task<List<Halaqa>> GetAllHalaqas();
        Task<Halaqa?> GetHalaqaById(int id);
        Task<Halaqa> AddHalaqa(Halaqa halaqa);
        Task<bool> UpdateHalaqa(int id, Halaqa halaqa);
        Task<bool> DeleteHalaqa(int id);
    }
}