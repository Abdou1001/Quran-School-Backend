using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        public Halaqa? Halaqa { get; set; }
    }
}