using AdaKurumsal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdaKurumsal.DataLayer
{
    public class EFContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public EFContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_configuration.GetSection("ConnectionStrings").GetSection("MyConnection").Value)
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole { Id = 1, RolName = "Developer", isActive = true },
                new AppRole { Id = 2, RolName = "Admin", isActive = true }
            );
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = 1,
                UserName = "Feyza Köse",
                Password = Tools.Kit.HashString("123456"),
                Email = "kose.feyza@gmail.com",
                isActive = true,
                isConfirm = true
            });
            modelBuilder.Entity<AppUserRole>().HasData(new AppUserRole
            {
                Id = 1,
                UserId = 1,
                RolId = 1,
                isActive = true
            });
        }
    }
}
