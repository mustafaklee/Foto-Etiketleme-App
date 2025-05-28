using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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


            var fotograflar = new List<Fotograf>()
            {
                new Fotograf()
                {
                    Id=1,
                    FotografPath="fotograf.jpg",
                    DoktorId=1,
                    EtiketlenmeTarihi=new DateOnly(2023, 5, 30),
                },
                new Fotograf()
                {
                    Id=2,
                    FotografPath="2fotograf.jpg",
                    DoktorId=2,
                    EtiketlenmeTarihi=new DateOnly(2023, 5, 20),
                }
            };

            builder.Entity<Fotograf>().HasData(fotograflar);

            var fotografEtiketleri = new List<FotografEtiket>()
            {
                new FotografEtiket { Id=1,FotografId = 1, EtiketId = 1, DoktorId = 1 },
                new FotografEtiket {  Id=2,FotografId = 1, EtiketId = 2, DoktorId = 1 },
                new FotografEtiket {  Id=3,FotografId = 2, EtiketId = 2, DoktorId = 2 },
                new FotografEtiket {  Id=4,FotografId = 2, EtiketId = 3, DoktorId = 2 }
            };


            builder.Entity<FotografEtiket>().HasData(fotografEtiketleri);

            // Relationship configuration for Many-to-Many
            builder.Entity<FotografEtiket>()
                .HasKey(fe => new { fe.FotografId, fe.EtiketId });

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
