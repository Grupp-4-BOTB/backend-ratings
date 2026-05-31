using Microsoft.EntityFrameworkCore;
using Rating.Domain.Entities;

namespace Rating.Infrastructure.Data
{
    public class RatingDbContext(DbContextOptions<RatingDbContext> options)
        : DbContext(options)
    {
        public DbSet<RatingEntity> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RatingEntity>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.CourseId });

                entity.Property(e => e.Rating)
                    .IsRequired();

                entity.ToTable("Ratings", "ratings");
            });
        }
    }
}