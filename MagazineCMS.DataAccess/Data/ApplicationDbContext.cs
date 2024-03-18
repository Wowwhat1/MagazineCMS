using MagazineCMS.Models;
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
        }

    }
}
