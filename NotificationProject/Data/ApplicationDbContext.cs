using Microsoft.EntityFrameworkCore;
using NotificationProject.Models;

namespace NotificationProject.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
                //PostgreSQL
                //entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");
                //SQL Server
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
