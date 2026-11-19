
using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public class RecitationProgress
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [Range(0, 100)]
        public int MemorizedPages { get; set; }

        [Range(0, 100)]
        public int ReviewPages { get; set; }

        [StringLength(200)]
        public string Notes { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public Student? Student { get; set; }
    }
}
