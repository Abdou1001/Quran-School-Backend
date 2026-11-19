using System.ComponentModel.DataAnnotations;

namespace QuranSchoolAPI.Models
{
    public enum MonthlyReportStatus
    {
        Draft = 1,       // مسودة
        Submitted = 2,   // مرسل للإدارة
        Approved = 3,    // معتمد
        Returned = 4     // معاد للمعلم
    }

    public class MonthlyReport
    {
        public int Id { get; set; }

        // الفترة الزمنية
        [Range(1, 12)]
        public int Month { get; set; }

        [Range(2020, 2100)]
        public int Year { get; set; }

        // حالة التقرير
        public MonthlyReportStatus Status { get; set; }
            = MonthlyReportStatus.Draft;

        // ملاحظات المعلم
        [StringLength(2000)]
        public string TeacherNotes { get; set; } = string.Empty;

        // ملاحظات الإدارة
        [StringLength(2000)]
        public string AdminNotes { get; set; } = string.Empty;

        // تواريخ الإجراءات
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? SubmittedAt { get; set; }

        public DateTime? ReviewedAt { get; set; }

        // الحلقة المرتبطة بالتقرير
        public int HalaqaId { get; set; }

        public Halaqa? Halaqa { get; set; }
    }
}