using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebAPI.Controllers;
using WebAPI.Models.Domain;

namespace WebAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Fotograf> Fotograf { get; set; }
        public DbSet<BreastBirads> BreastBirads { get; set; }
        public DbSet<FotografEtiket> FotografEtiket { get; set; }
        public DbSet<Doktor> Doktor { get; set; }
        public DbSet<FindingCategories> FindingCategories { get; set; }
        public DbSet<Folder> Folder { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Initial data
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

            var breast_birads = new List<BreastBirads>()
            {
                new BreastBirads() { Id = 1, CategoryName = "BI-RADS 1" },
                new BreastBirads() { Id = 2, CategoryName = "BI-RADS 2" },
                new BreastBirads() { Id = 3, CategoryName = "BI-RADS 3" },
                new BreastBirads() { Id = 4, CategoryName = "BI-RADS 4" },
                new BreastBirads() { Id = 5, CategoryName = "BI-RADS 5" },
                new BreastBirads() { Id = 6, CategoryName = "BI-RADS 6" }
            };

            builder.Entity<BreastBirads>().HasData(breast_birads);

            var findingCategories = new List<FindingCategories>()
            {
                new FindingCategories() { Id = 1, CategoryName = "Mass" },
                new FindingCategories() { Id = 2, CategoryName = "Global Asymmetry" },
                new FindingCategories() { Id = 3, CategoryName = "Architectural Distortion" },
                new FindingCategories() { Id = 4, CategoryName = "Nipple Retraction, Mass" },
                new FindingCategories() { Id = 5, CategoryName = "Suspicious Calcification,Focal Asymmetry" },
                new FindingCategories() { Id = 6, CategoryName = "Focal Asymmetry" },
                new FindingCategories() { Id = 7, CategoryName = "Asymmetry" }
            };

            builder.Entity<FindingCategories>().HasData(findingCategories);

            var folder = new List<Folder>()
            {
                new Folder { Id = 1, FolderPath = "memography/0a0c5108270e814818c1ad002482ce74", DoktorId = new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301") }
            };

            builder.Entity<Folder>().HasData(folder);

            var fotograflar = new List<Fotograf>()
            {
                new Fotograf { Id = 1, FotografPath = "0a6a90bdc088e0cc62df8d2d58d14840.png", FolderId = 1 },
                new Fotograf { Id = 2, FotografPath = "1b66d3ea1dae116b7c0e87e3caab3340.png", FolderId = 1 },
                new Fotograf { Id = 3, FotografPath = "7a3df96890c90370590984ca196d1b40.png", FolderId = 1 },
                new Fotograf { Id = 4, FotografPath = "cb8a1b1282b4b16c0f322e9fc89a9c35.png", FolderId = 1 }
            };

            builder.Entity<Fotograf>().HasData(fotograflar);

            var fotografEtiketleri = new List<FotografEtiket>()
            {
                new FotografEtiket { Id = 1, imageId = 1, breast_biradsId = 1, finding_categoriesId = 1 },
                new FotografEtiket { Id = 2, imageId = 2, breast_biradsId = 1, finding_categoriesId = 1 },
                new FotografEtiket { Id = 3, imageId = 3, breast_biradsId = 3, finding_categoriesId = 5 },
                new FotografEtiket { Id = 4, imageId = 4, breast_biradsId = 3, finding_categoriesId = 5}
            };

            builder.Entity<FotografEtiket>().HasData(fotografEtiketleri);

            // Relationship configuration for Many-to-Many
            builder.Entity<FotografEtiket>()
                .HasKey(fe => fe.Id);

            builder.Entity<FotografEtiket>()
                .HasOne(fe => fe.Fotograf)
                .WithMany(f => f.FotografEtiketleri)
                .HasForeignKey(fe => fe.imageId);

            builder.Entity<FotografEtiket>()
                .HasOne(fe => fe.BreastBirads)
                .WithMany(b => b.FotografEtiket)
                .HasForeignKey(fe => fe.breast_biradsId);

            builder.Entity<FotografEtiket>()
                .HasOne(fe => fe.FindingCategories)
                .WithMany(fc => fc.FotografEtiket)
                .HasForeignKey(fe => fe.finding_categoriesId);

            // Folder ve Fotograf İlişkisini Kurma
            builder.Entity<Fotograf>()
                .HasOne(f => f.Folder)
                .WithMany(f => f.Fotograf) 
                .HasForeignKey(f => f.FolderId);

            // Folder ve Doktor İlişkisini Kurma
            builder.Entity<Folder>()
                .HasOne(f => f.Doktor)
                .WithMany(d => d.Folder)
                .HasForeignKey(f => f.DoktorId);
        }
    }
}
