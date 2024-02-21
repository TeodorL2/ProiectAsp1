using Drive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Drive.Data
{
    public class DriveContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccessToBaseDir> UserAccessToBaseDirs { get; set; }
        public DbSet<BaseDirectory> BaseDirectories { get; set; }
        public DbSet<BaseDirDescription> BaseDirDescriptions { get; set; }

        public DriveContext(DbContextOptions<DriveContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // one-to-many
            modelBuilder.Entity<User>()
                        .HasMany(u => u.BaseDirectories)
                        .WithOne(b => b.User)
                        .HasForeignKey(b => b.Author)
                        .OnDelete(DeleteBehavior.NoAction);

            // many-to-many
            modelBuilder.Entity<UserAccessToBaseDir>()
                        .HasOne(ua => ua.User)
                        .WithMany(u => u.UserAccessToBaseDirs)
                        .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAccessToBaseDir>()
                        .HasOne(ua => ua.BaseDirectory)
                        .WithMany(b => b.UserAccessToBaseDirs)
                        .HasForeignKey(ua => ua.BaseDirId);

            // one-to-one
            modelBuilder.Entity<BaseDirDescription>()
                        .HasOne(d => d.BaseDirectory)
                        .WithOne(b => b.BaseDirDescription)
                        .HasForeignKey<BaseDirDescription>(d => d.BaseDirId);

            modelBuilder.Entity<UserAccessToBaseDir>().HasKey(ua => new { ua.UserId, ua.BaseDirId });

            base.OnModelCreating(modelBuilder);
        }
    }
}
