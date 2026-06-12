using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace LibraryService.WebAPI.Data
{
    public class FraudContext : DbContext
    {
        public FraudContext(DbContextOptions<FraudContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fraud>(entity =>
            {
                entity.ToTable("frauds");
                entity.Property(f => f.ImpostorDetails).IsRequired().HasMaxLength(2000);
                entity.Property(f => f.ContactInfo).IsRequired().HasMaxLength(500);
                entity.Property(f => f.Comments).HasMaxLength(4000);
                entity.Property(f => f.CreatedAt).IsRequired();
            });
        }

        public DbSet<Fraud> Frauds { get; set; }
    }

    public class Fraud
    {
        [Key]
        public int Id { get; set; }

        public string ImpostorDetails { get; set; } = string.Empty;

        public string ContactInfo { get; set; } = string.Empty;

        public string Comments { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
