using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Interfaces
{
    public interface IRecitationProgressService
    {
        Task<List<RecitationProgress>> GetAllProgress();
        Task<RecitationProgress?> GetProgressById(int id);
        Task<RecitationProgress> AddProgress(RecitationProgress progress);
        Task<bool> UpdateProgress(int id, RecitationProgress progress);
        Task<bool> DeleteProgress(int id);
    }
}