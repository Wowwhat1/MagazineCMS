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

        protected override void OnModelCreating(ModelBuilder modelBuidlder)
        {
            base.OnModelCreating(modelBuidlder);

            modelBuidlder.Entity<Faculty>().HasData(
                new Faculty { Id = 1, Name="All"},
                new Faculty { Id = 2, Name="Computing"},
                new Faculty { Id = 3, Name="Business"},
                new Faculty { Id = 4, Name="Design"}
                );
        }
    }
}
