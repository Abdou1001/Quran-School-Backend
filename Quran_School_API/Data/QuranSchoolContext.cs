using Microsoft.EntityFrameworkCore;
using QuranSchoolAPI.Models;

namespace QuranSchoolAPI.Data
{
    public class QuranSchoolContext : DbContext
    {
        public QuranSchoolContext(DbContextOptions<QuranSchoolContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Halaqa> Halaqas { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<RecitationProgress> RecitationProgresses { get; set; }
        public DbSet<MonthlyReport> MonthlyReports { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Halaqa>()
                .HasOne(h => h.Teacher)
                .WithOne(t => t.Halaqa)
                .HasForeignKey<Halaqa>(h => h.TeacherId);

            // منع تكرار حضور الطالب في اليوم نفسه
            modelBuilder.Entity<Attendance>()
                .HasIndex(a => new { a.StudentId, a.Date })
                .IsUnique();

            // منع تكرار الحفظ والمراجعة للطالب في اليوم نفسه
            modelBuilder.Entity<RecitationProgress>()
                .HasIndex(p => new { p.StudentId, p.Date })
                .IsUnique();

            modelBuilder.Entity<MonthlyReport>()
                .HasOne(r => r.Halaqa)
                .WithMany()
                .HasForeignKey(r => r.HalaqaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MonthlyReport>()
                .HasIndex(r => new { r.HalaqaId, r.Month, r.Year })
                .IsUnique();
        }
    }
}