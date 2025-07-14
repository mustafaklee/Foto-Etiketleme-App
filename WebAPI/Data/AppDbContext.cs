using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebAPI.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace WebAPI.Data
{
    public class AppDbContext : IdentityDbContext<Doctor, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Image> Image { get; set; }
        public DbSet<BreastBirads> BreastBirads { get; set; }
        public DbSet<BreastBiradsEntity> BreastBiradsEntities { get; set; }
        public DbSet<FindingCategories> FindingCategories { get; set; }
        public DbSet<Folder> Folder { get; set; }
        public DbSet<FolderDoctorEntity> FolderDoctorEntities { get; set; }
        public DbSet<FindingCategoriesEntity> FindingCategoriesEntities { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ---------------- SEED DATA ----------------
            // Roller
            var adminRoleId = 1;
            var doctorRoleId = 2;
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityRole<int>>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityRole<int> { Id = adminRoleId, Name = "admin", NormalizedName = "ADMIN" },
                new Microsoft.AspNetCore.Identity.IdentityRole<int> { Id = doctorRoleId, Name = "doktor", NormalizedName = "DOKTOR" }
            );

            // Kullanıcılar
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Doctor>();
            var adminUser = new Doctor {
                Id = 1, UserName = "admin@example.com", NormalizedUserName = "ADMIN@EXAMPLE.COM", Email = "admin@example.com", NormalizedEmail = "ADMIN@EXAMPLE.COM", EmailConfirmed = true, SecurityStamp = "", PasswordHash = hasher.HashPassword(null, "Admin123*")
            };
            var doctorA = new Doctor {
                Id = 2, UserName = "doctorA@example.com", NormalizedUserName = "DOCTORA@EXAMPLE.COM", Email = "doctorA@example.com", NormalizedEmail = "DOCTORA@EXAMPLE.COM", EmailConfirmed = true, SecurityStamp = "", PasswordHash = hasher.HashPassword(null, "Doctor123*")
            };
            var doctorB = new Doctor
            {
                Id = 3,
                UserName = "doctorB@example.com",
                NormalizedUserName = "DOCTORB@EXAMPLE.COM",
                Email = "doctorB@example.com",
                NormalizedEmail = "DOCTORB@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = "",
                PasswordHash = hasher.HashPassword(null, "Doctor123*")
            };
            builder.Entity<Doctor>().HasData(adminUser, doctorA,doctorB);

            // Kullanıcı-Rol ilişkisi
            builder.Entity<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>().HasData(
                new Microsoft.AspNetCore.Identity.IdentityUserRole<int> { UserId = adminUser.Id, RoleId = adminRoleId },
                new Microsoft.AspNetCore.Identity.IdentityUserRole<int> { UserId = doctorA.Id, RoleId = doctorRoleId },
                new Microsoft.AspNetCore.Identity.IdentityUserRole<int> { UserId = doctorB.Id, RoleId = doctorRoleId }
            );

            // BI‑RADS kategorileri
            builder.Entity<BreastBirads>().HasData(
                new BreastBirads { Id = 1, CategoryName = "BI-RADS 1" },
                new BreastBirads { Id = 2, CategoryName = "BI-RADS 2" },
                new BreastBirads { Id = 3, CategoryName = "BI-RADS 3" },
                new BreastBirads { Id = 4, CategoryName = "BI-RADS 4" },
                new BreastBirads { Id = 5, CategoryName = "BI-RADS 5" },
                new BreastBirads { Id = 6, CategoryName = "BI-RADS 6" }
            );

            builder.Entity<BreastBiradsEntity>().HasData(
                new BreastBiradsEntity { Id=1,ImageId=1, DoctorId = 2, BreastBiradsId =1 },
                new BreastBiradsEntity { Id = 2, ImageId = 1, DoctorId= 3, BreastBiradsId = 2 },
                new BreastBiradsEntity { Id = 3, ImageId = 2, DoctorId = 2, BreastBiradsId = 4 },
                new BreastBiradsEntity { Id = 4, ImageId = 2, DoctorId = 3, BreastBiradsId = 3 }
                );

            // FindingCategories
            builder.Entity<FindingCategories>().HasData(
                new FindingCategories { Id = 1, CategoryName = "Mass" },
                new FindingCategories { Id = 2, CategoryName = "Global Asymmetry" },
                new FindingCategories { Id = 3, CategoryName = "Architectural Distortion" },
                new FindingCategories { Id = 4, CategoryName = "Nipple Retraction" },
                new FindingCategories { Id = 5, CategoryName = "Suspicious Calcification" },
                new FindingCategories { Id = 6, CategoryName = "Focal Asymmetry" },
                new FindingCategories { Id = 7, CategoryName = "Asymmetry" }
            );

            builder.Entity<FindingCategoriesEntity>().HasData(
                new FindingCategoriesEntity { Id = 1, ImageId = 1, DoctorId = 2, FindingCategoriesId = 1 },
                new FindingCategoriesEntity { Id = 2, ImageId = 1, DoctorId = 2, FindingCategoriesId = 2 },
                new FindingCategoriesEntity { Id = 3, ImageId = 1, DoctorId = 3, FindingCategoriesId = 3 },
                new FindingCategoriesEntity { Id = 4, ImageId = 1, DoctorId = 3, FindingCategoriesId = 4 }
            );

            builder.Entity<FolderDoctorEntity>().HasData(
                new FolderDoctorEntity {Id=1,  FolderId=1, DoctorId = 2 },
                new FolderDoctorEntity { Id = 2, FolderId = 1, DoctorId = 3}
            );



            // Örnek klasör 
            var folder1 = new Folder { Id = 1, FolderPath = "0a0c5108270e814818c1ad002482ce74" };
            builder.Entity<Folder>().HasData(folder1);

            builder.Entity<Image>().HasData(
                new Image { Id = 1, FotografPath = "0a6a90bdc088e0cc62df8d2d58d14840.png", FolderId = 1, laterality_id = 1, view_position_id = 2 },
                new Image { Id = 2, FotografPath = "1b66d3ea1dae116b7c0e87e3caab3340.png", FolderId = 1, laterality_id = 2, view_position_id = 1 }
            );


            builder.Entity<laterality>().HasData(
                new laterality { id = 1, laterality_name = "R" },
                new laterality { id = 2, laterality_name = "L" }
            );

            builder.Entity<view_position>().HasData(
                new view_position { id = 1, view_position_name = "CC" },
                new view_position { id = 2, view_position_name = "MLO" }
            );

            // ---------------- RELATIONSHIPS ----------------
            // Image → laterality & view_position

            //id tanımları
            builder.Entity<Image>()
                   .HasKey(fe => fe.Id);
            builder.Entity<BreastBirads>()
                   .HasKey(fe => fe.Id);
            builder.Entity<BreastBiradsEntity>()
                   .HasKey(fe => fe.Id);
            builder.Entity<FindingCategories>()
                   .HasKey(fe => fe.Id);
            builder.Entity<FindingCategoriesEntity>()
                   .HasKey(fe => fe.Id);
            builder.Entity<Folder>()
                   .HasKey(fe => fe.Id);
            builder.Entity<laterality>()
                   .HasKey(fe => fe.id);
            builder.Entity<view_position>()
                   .HasKey(fe => fe.id);




            //BreastBiradsEntity
            builder.Entity<BreastBiradsEntity>()
                .HasOne(fe => fe.Image)
                .WithMany(ce => ce.BreastBiradsEntities)
                .HasForeignKey(se => se.ImageId);

            builder.Entity<BreastBiradsEntity>()
                 .HasOne(fe => fe.BreastBirads)
                 .WithMany(ce => ce.BreastBiradsEntities)
                 .HasForeignKey(se => se.BreastBiradsId);

            builder.Entity<BreastBiradsEntity>()
                 .HasOne(fe => fe.Doctor)
                 .WithMany(ce => ce.BreastBiradsEntities)
                 .HasForeignKey(se => se.DoctorId);


            //Image
            builder.Entity<Image>()
                   .HasOne(c => c.laterality)
                   .WithMany(l => l.Image)
                   .HasForeignKey(f => f.laterality_id);

            builder.Entity<Image>()
                   .HasOne(f => f.view_Position)
                   .WithMany(vp => vp.Image)
                   .HasForeignKey(f => f.view_position_id);

            builder.Entity<Image>()
                   .HasOne(ce => ce.Folder)
                   .WithMany(s => s.Image)
                   .HasForeignKey(c => c.FolderId);

            //FolderDoctorEntity

            builder.Entity<FolderDoctorEntity>()
                .HasOne(ce => ce.Folder)
                .WithMany(m => m.FolderDoctorEntities)
                .HasForeignKey(k => k.FolderId);
            builder.Entity<FolderDoctorEntity>()
                .HasOne(ce => ce.Doctor)
                .WithMany(ı => ı.FolderDoctorEntities)
                .HasForeignKey(l => l.DoctorId);
        }
    }
}
