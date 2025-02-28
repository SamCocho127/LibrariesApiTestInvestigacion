using System.ComponentModel.DataAnnotations;
using HackerRank1.Data;
using Microsoft.EntityFrameworkCore;

namespace LearningService.WebAPI.Data;

public class StudentsContext : DbContext
{
    public StudentsContext(DbContextOptions<StudentsContext> options)
        : base(options)
    { }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Students> Students { get; set; }
    public DbSet<StudentActivity> StudentActivities { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(a => a.Location)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(a => a.ActivityType)
                  .HasConversion<int>() // Store enum as int
                  .IsRequired();

            entity.Property(a => a.ActivityDate)
                  .IsRequired();

            entity.Property(a => a.Status)
                  .IsRequired();
        });

        modelBuilder.Entity<Students>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Name)
                  .IsRequired(false)
                  .HasMaxLength(255);

            entity.Property(a => a.activityId)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(a => a.Email)
                  .HasConversion<int>() // Store enum as int
                  .IsRequired(false);

            entity.Property(a => a.GithubUrl)
                  .HasConversion<int>() // Store enum as int
                  .IsRequired(false);            
        });

        modelBuilder.Entity<StudentActivity>()
            .HasOne(sa => sa.Activity)
            .WithMany(a => a.StudentActivities)
            .HasForeignKey(sa => sa.StudentId)
            .HasForeignKey(sa => sa.ActivityId);

        base.OnModelCreating(modelBuilder);
    }
}   
