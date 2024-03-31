using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineCMS.DataAccess.DBInitializer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DBInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            // migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error applying migrations: {ex.Message}");
            }

            // create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Student).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Student)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Coordinator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager)).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    Id = "AdminID1",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Firstname = "A",
                    Lastname = "Nguyen Van",
                    PhoneNumber = "1112223333",
                    AvatarUrl = SD.Default_Avatar,
                    FacultyId = 1,
                }, "Admin123*").GetAwaiter().GetResult();

                User userDB = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(userDB, SD.Role_Admin).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    Id = "ManagerID1",
                    UserName = "manager@gmail.com",
                    Email = "manager@gmail.com",
                    Firstname = "B",
                    Lastname = "Nguyen Thi",
                    PhoneNumber = "1112223333",
                    AvatarUrl = SD.Default_Avatar,
                    FacultyId = 1,
                }, "Admin123*").GetAwaiter().GetResult();

                userDB = _db.Users.FirstOrDefault(u => u.Email == "manager@gmail.com");
                _userManager.AddToRoleAsync(userDB, SD.Role_Manager).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    Id = "CoordinatorID1",
                    UserName = "coordinator@gmail.com",
                    Email = "coordinator@gmail.com",
                    Firstname = "C",
                    Lastname = "Dang Van",
                    PhoneNumber = "1112223333",
                    AvatarUrl = SD.Default_Avatar,
                    FacultyId = 2,
                }, "Admin123*").GetAwaiter().GetResult();

                userDB = _db.Users.FirstOrDefault(u => u.Email == "coordinator@gmail.com");
                _userManager.AddToRoleAsync(userDB, SD.Role_Coordinator).GetAwaiter().GetResult();

                _userManager.CreateAsync(new User
                {
                    Id = "StudentID1",
                    UserName = "student@gmail.com",
                    Email = "student@gmail.com",
                    Firstname = "D",
                    Lastname = "Dang Thi",
                    PhoneNumber = "1112223333",
                    AvatarUrl = SD.Default_Avatar,
                    FacultyId = 2,
                }, "Admin123*").GetAwaiter().GetResult();

                userDB = _db.Users.FirstOrDefault(u => u.Email == "student@gmail.com");
                _userManager.AddToRoleAsync(userDB, SD.Role_Student).GetAwaiter().GetResult();
            }

            //if (!_db.Semesters.Any())
            //{
            //    var semesters = new List<Semester>
            //    {
            //        new Semester { Id = 1, Name="Spring Term 2024", StartDate = new DateTime(2024,1,15, 0, 0, 0), EndDate = new DateTime(2024, 4,5)},
            //        new Semester { Id = 2, Name="Summer Term 2024", StartDate = new DateTime(2024,4,22, 0, 0, 0), EndDate = new DateTime(2024, 7,19)},
            //        new Semester { Id = 3, Name="Autumn Term 2024", StartDate = new DateTime(2024,9,25, 0, 0, 0), EndDate = new DateTime(2024, 12,15)}
            //    };

            //    _db.Semesters.AddRange(semesters);
            //    _db.SaveChanges();
            //}
            
            if (!_db.Magazines.Any())
            {
                // create some magazines
                var magazines = new List<Magazine>
                {
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
                };

                _db.Magazines.AddRange(magazines);
                    _db.SaveChanges();
            }

            if (!_db.Contributions.Any())
            {
                // create some contributions
                var contributions = new List<Contribution>
                {
                    new Contribution
                    {
                        Id = 1,
                        Title = "The Future of AI",
                        Status = SD.Status_Pending,
                        SubmissionDate = DateTime.Now,
                        UserId = "StudentID1",
                        MagazineId = 1
                    }
                };

                _db.Contributions.AddRange(contributions);
                _db.SaveChanges();
            }

            if (!_db.Documents.Any())
            {
                // create some documents
                var documents = new List<Document>
                {
                    new Document
                    {
                        Id = 1,
                        Type = SD.Document_Type_Word,
                        DocumentUrl = "~/contributions/StudentID1/File.doc",
                        ContributionId = 1
                    }
                };
                _db.Documents.AddRange(documents);
                _db.SaveChanges();
            }
        }

    }
}
