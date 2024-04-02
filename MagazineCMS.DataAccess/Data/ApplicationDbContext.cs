using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Contribution> Contributions { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuidlder)
        {
            base.OnModelCreating(modelBuidlder);

            modelBuidlder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name="All"},
                new Faculty { Id = 2, Name="Computing"},
                new Faculty { Id = 3, Name="Business"},
                new Faculty { Id = 4, Name="Design"}
                );

            modelBuidlder.Entity<Semester>().HasData(
                new Semester { Id = 1, Name="Spring Term 2024", StartDate = new DateTime(2024,1,15, 0, 0, 0), EndDate = new DateTime(2024, 4,5)},
                new Semester { Id = 2, Name="Summer Term 2024", StartDate = new DateTime(2024,4,22, 0, 0, 0), EndDate = new DateTime(2024, 7,19)},
                new Semester { Id = 3, Name="Autumn Term 2024", StartDate = new DateTime(2024,9,25, 0, 0, 0), EndDate = new DateTime(2024, 12,15)}
            );

            modelBuidlder.Entity<Magazine>().HasData(
               new Magazine
               {
                   Id = 1,
                   Name = "Computing Magazine - Spring 2024",
                   Description = "Welcome to the Spring 2024 issue of Cutting-Edge Tech, your ultimate guide to the latest innovations and developments in the world of computing. In this edition, we delve into the forefront of technology, exploring groundbreaking advancements that are shaping the future of computing.",
                   StartDate = DateTime.Now.AddDays(-7),
                   EndDate = DateTime.Now.AddDays(7),
                   FacultyId = 2,
                   SemesterId = 1
               },
               new Magazine
               {
                   Id = 2,
                   Name = "Business Magazine - Spring 2024",
                   Description = "Welcome",
                   StartDate = DateTime.Now.AddDays(-7),
                   EndDate = DateTime.Now.AddDays(7),
                   FacultyId = 3,
                   SemesterId = 1
               }
            );

            modelBuidlder.Entity<Contribution>().HasData(
                new Contribution
                {
                    Id = 1,
                    Title = "The Future of AI",
                    Status = SD.Status_Pending,
                    SubmissionDate = DateTime.Now,
                    UserId = "StudentID1",
                    MagazineId = 1
                }
            );

            modelBuidlder.Entity<Document>().HasData(
                 new Document
                 {
                     Id = 1,
                     Type = SD.Document_Type_Word,
                     DocumentUrl = "~/contributions/StudentID1/File.docx",
                     ContributionId = 1
                 }
            );
        }

    }
}
