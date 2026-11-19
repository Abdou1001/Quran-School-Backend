
using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public class Halaqa
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string Level { get; set; } = string.Empty;

        public int TeacherId { get; set; }

        public Teacher? Teacher { get; set; }

        public ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}
