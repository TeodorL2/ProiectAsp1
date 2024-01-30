using Drive.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Drive.Data
{
    public class DriveContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccessToBaseDirectory> UserAccessToBaseDirectorys { get; set; }
        public DbSet<BaseDirectory> BaseDirectorys { get; set; }
        public DbSet<DirectoryDesc> DirectoryDescs { get; set; }
        public DriveContext(DbContextOptions<DriveContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // one-to-many
            modelBuilder.Entity<User>()
                        .HasMany(u => u.BaseDirectorys)
                        .WithOne(b => b.User)
                        .HasForeignKey(b => b.Author)
                        .OnDelete(DeleteBehavior.NoAction);


            // many-to-many
            modelBuilder.Entity<UserAccessToBaseDirectory>()
                        .HasOne(ua => ua.User)
                        .WithMany(u => u.UserAccessToBaseDirectorys)
                        .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserAccessToBaseDirectory>()
                         .HasOne(ua => ua.BaseDirectory)
                         .WithMany(b => b.UserAccessToBaseDirectorys)
                         .HasForeignKey(ua => ua.BaseDirectoryId);
            
           
            // one-to-one
            modelBuilder.Entity<DirectoryDesc>()
                        .HasOne(desc => desc.BaseDirectory)
                        .WithOne(b => b.DirectoryDesc)
                        .HasForeignKey<DirectoryDesc>(desc => desc.DirectoryId);

            modelBuilder.Entity<UserAccessToBaseDirectory>().HasKey(ua => new { ua.UserId, ua.BaseDirectoryId });
            


            base.OnModelCreating(modelBuilder);
        }
    }
}
