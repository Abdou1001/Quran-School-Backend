
using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public class Student
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string StudentNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        public DateTime EnrollmentDate { get; set; }

        public int? HalaqaId { get; set; }

        public Halaqa? Halaqa { get; set; }

        public ICollection<Attendance> Attendances { get; set; }
            = new List<Attendance>();

        public ICollection<RecitationProgress> RecitationProgresses { get; set; }
            = new List<RecitationProgress>();
    }
}
