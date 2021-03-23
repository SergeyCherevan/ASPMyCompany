using ASPMyCompany.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPMyCompany.Domain
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<TextField> TextFields { get; set; }
        public DbSet<ServiceItem> ServiceItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(new IdentityRole()
            {
                Id = "85CA2F04-5F0E-4379-B33B-7597F6331ED2",

                Name = "admin",
                NormalizedName = "ADMIN",
            });

            modelBuilder.Entity<IdentityUser>().HasData(new IdentityUser()
            {
                Id = "205C2FF5-544B-483E-A4E4-5766C36CB740",

                UserName = "admin",
                NormalizedUserName = "ADMIN",

                Email = "Admin@MySuperCompany.com",
                NormalizedEmail = "ADMIN@MYSUPERCOMPANY.COM",
                EmailConfirmed = true,

                PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(null, "superpassword"),
                SecurityStamp = string.Empty,
            });

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>()
            {
                RoleId = "85CA2F04-5F0E-4379-B33B-7597F6331ED2",
                UserId = "205C2FF5-544B-483E-A4E4-5766C36CB740"
            });


            /* ---------------------------------*/


            modelBuilder.Entity<TextField>().HasData(new TextField()
            {
                Id = new Guid("5DAAA96D-B2CD-44FD-8360-52DE781F2A4F"),
                CodeWord = "PageIndex",
                Title = "Главная"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField()
            {
                Id = new Guid("B71ADEEE-4183-4AB3-83ED-C4DC92CCA914"),
                CodeWord = "PageServices",
                Title = "Наши услуги"
            });

            modelBuilder.Entity<TextField>().HasData(new TextField()
            {
                Id = new Guid("C61EED6D-BE53-4D69-B806-998BD733BC7B"),
                CodeWord = "PageContacts",
                Title = "Наши контакты"
            });
        }
    }
}
