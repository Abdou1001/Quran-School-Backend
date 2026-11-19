
using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public class Attendance
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public bool IsPresent { get; set; }

        [StringLength(200)]
        public string Notes { get; set; } = string.Empty;

        public int StudentId { get; set; }

        public Student? Student { get; set; }
    }
}
