using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebAPI.Controllers;
using WebAPI.Models.Domain;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions):base (dbContextOptions)
        {

        }

        public DbSet<Fotograf> Fotograf { get; set; }
        public DbSet<Etiket> Etiket { get; set; }

        public DbSet<FotografEtiket>  FotografEtiket {get;set;}
        public DbSet<Doktor> Doktor { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var fotograflar = new List<Fotograf>
            {
                new Fotograf
                {
                    Id = 1,
                    FotografPath = "cat.1.jpg"
                },
                new Fotograf
                {
                    Id = 2,
                    FotografPath = "cat.2.jpg"
                },

                new Fotograf
                {
                    Id = 3,
                    FotografPath = "cat.3.jpg"
                },
                new Fotograf
                {
                    Id = 4,
                    FotografPath ="cat.4.jpg"
                },
            };

            builder.Entity<Fotograf>().HasData(fotograflar);


            var doktorlar = new List<Doktor>
            {
                new Doktor
                {
                    Id = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"),
                    Email = "ali@example.com"
                },
                new Doktor
                {
                    Id = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"),
                    Email = "ayse@example.com"
                },
                new Doktor
                {
                    Id = new Guid("3f2504e0-abcd-11d3-9a0c-0305e82c1111"),
                    Email = "mehmet@example.com"
                },
                new Doktor
                {
                    Id = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c5555"),
                    Email = "fatma@example.com"
                }
            };

            builder.Entity<Doktor>().HasData(doktorlar);




            var etiketler = new List<Etiket>()
                {
                new Etiket()
                {
                    Id = 1,
                    EtiketAd = "A",
                },
                new Etiket()
                {
                    Id = 2,
                    EtiketAd = "B",
                },
                new Etiket()
                {
                    Id = 3,
                    EtiketAd = "C",
                }
            };

            builder.Entity<Etiket>().HasData(etiketler);


            var fotografEtiketleri = new List<FotografEtiket>()
            {
                new FotografEtiket { Id = 1, FotografId = 1, EtiketId = 1, DoktorId = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), EtiketTarihi = new DateOnly(2025, 6, 13) },
                new FotografEtiket { Id = 2, FotografId = 2, EtiketId = 1, DoktorId = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c8888"), EtiketTarihi = new DateOnly(2025, 6, 14) },
                new FotografEtiket { Id = 3, FotografId = 3, EtiketId = 2, DoktorId = new Guid("3f2504e0-abcd-11d3-9a0c-0305e82c1111"), EtiketTarihi = new DateOnly(2025, 6, 15) },
                new FotografEtiket { Id = 4, FotografId = 4, EtiketId = 3, DoktorId = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c5555"), EtiketTarihi = new DateOnly(2025, 6, 16) }
            };



            builder.Entity<FotografEtiket>().HasData(fotografEtiketleri);

                // Relationship configuration for Many-to-Many
                builder.Entity<FotografEtiket>()
                    .HasKey(fe => new { fe.Id});

                builder.Entity<FotografEtiket>()
                    .HasOne(fe => fe.Fotograf)
                    .WithMany(f => f.FotografEtiketleri)
                    .HasForeignKey(fe => fe.FotografId);

                builder.Entity<FotografEtiket>()
                    .HasOne(fe => fe.Etiket)
                    .WithMany(e => e.FotografEtiketleri)
                    .HasForeignKey(fe => fe.EtiketId);

                builder.Entity<FotografEtiket>()
                    .HasOne(fe => fe.Doktor)
                    .WithMany(d => d.FotografEtiketleri)
                    .HasForeignKey(fe => fe.DoktorId);
        }

    }
}
