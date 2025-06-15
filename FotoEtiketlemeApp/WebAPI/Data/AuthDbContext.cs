using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Yeni roller için sabit GUID'ler
            var adminRoleId = "A1B2C3D4-E5F6-4789-ABCD-1234567890AB";
            var doctorRoleId = "D1E2F3A4-B5C6-4789-EFGH-0987654321DC";

            // Yeni rollerin tanımı
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = doctorRoleId,
                    ConcurrencyStamp = doctorRoleId,
                    Name = "Doctor",
                    NormalizedName = "DOCTOR"
                }
            };

            // Veritabanına bu roller ekleniyor
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
