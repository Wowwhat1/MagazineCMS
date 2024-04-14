using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.Models;
using MagazineCMS.Models.ViewModels;
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
           /* // Fake firstNames and LastNames
            List<string> firstNames = new List<string> { "A", "B", "C", "D", "E" };
            List<string> lastNames = new List<string> { "Nguyen", "Tran", "Le", "Pham", "Hoang" };

            // Create roles if that invalid
            if (!_roleManager.RoleExistsAsync(SD.Role_Student).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Student)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Coordinator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager)).GetAwaiter().GetResult();
            }*/

            //// Loop for create accounts
            //// Create accounts for Computing follow Id from 1 to 100
            //for (int i = 0; i < 100; i++)
            //{
            //    string firstName = firstNames[i % firstNames.Count];
            //    string lastName = lastNames[i % lastNames.Count];
            //    string email = $"user{i}@gmail.com";

            //    var user = new User
            //    {
            //        Id = $"UserID{i}",
            //        UserName = email,
            //        Email = email,
            //        Firstname = firstName,
            //        Lastname = lastName,
            //        PhoneNumber = $"1112223{i.ToString().PadLeft(3, '0')}",
            //        AvatarUrl = SD.Default_Avatar,
            //        FacultyId = 2 // Create value 2 for Facuty
            //    };

            //    var result = _userManager.CreateAsync(user, "Password123*").GetAwaiter().GetResult();

            //    if (result.Succeeded)
            //    {
            //        _userManager.AddToRoleAsync(user, SD.Role_Student).GetAwaiter().GetResult();
            //    }
            //}

            //// Create accounts for Business follow Id from 101 to 200
            //for (int i = 100; i < 200; i++)
            //{
            //    string firstName = firstNames[i % firstNames.Count];
            //    string lastName = lastNames[i % lastNames.Count];
            //    string email = $"user{i}@gmail.com";

            //    var user = new User
            //    {
            //        Id = $"UserID{i}",
            //        UserName = email,
            //        Email = email,
            //        Firstname = firstName,
            //        Lastname = lastName,
            //        PhoneNumber = $"1112223{i.ToString().PadLeft(3, '0')}",
            //        AvatarUrl = SD.Default_Avatar,
            //        FacultyId = 3 // Create value 3 for Facuty
            //    };

            //    var result = _userManager.CreateAsync(user, "Password123*").GetAwaiter().GetResult();

            //    if (result.Succeeded)
            //    {
            //        _userManager.AddToRoleAsync(user, SD.Role_Student).GetAwaiter().GetResult();
            //    }
            //}

            //// Create accounts for Design follow Id from 201 to 250
            //for (int i = 200; i < 250; i++)
            //{
            //    string firstName = firstNames[i % firstNames.Count];
            //    string lastName = lastNames[i % lastNames.Count];
            //    string email = $"user{i}@gmail.com";

            //    var user = new User
            //    {
            //        Id = $"UserID{i}",
            //        UserName = email,
            //        Email = email,
            //        Firstname = firstName,
            //        Lastname = lastName,
            //        PhoneNumber = $"1112223{i.ToString().PadLeft(3, '0')}",
            //        AvatarUrl = SD.Default_Avatar,
            //        FacultyId = 4 // Create value 4 for Facuty
            //    };

            //    var result = _userManager.CreateAsync(user, "Password123*").GetAwaiter().GetResult();

            //    if (result.Succeeded)
            //    {
            //        _userManager.AddToRoleAsync(user, SD.Role_Student).GetAwaiter().GetResult();
            //    }
            //}


            //// Loop for create contributions
            //if (!_db.Contributions.Any())
            //{
            //    // create some contributions
            //    var contributions = new List<Contribution>();

            //    // Create contribution for computing
            //    for (int i = 1; i <= 20; i++)
            //    {
            //        var userId = $"UserID{i}";
            //        var userExists = _db.Users.Any(u => u.Id == userId);

            //        if (!userExists)
            //        {
            //            // Handle the case where the user does not exist
            //            // For example, you could continue to the next iteration of the loop
            //            continue;
            //        }

            //        contributions.Add(new Contribution
            //        {
            //            Title = $"The Future of AI {i}",
            //            Status = SD.Status_Pending,
            //            SubmissionDate = DateTime.Now,
            //            UserId = userId,
            //            MagazineId = 1
            //        });
            //    }

            //    // Create contribution for business
            //    for (int i = 100; i <= 152; i++)
            //    {
            //        var userId = $"UserID{i}";
            //        var userExists = _db.Users.Any(u => u.Id == userId);

            //        if (!userExists)
            //        {
            //            // Handle the case where the user does not exist
            //            // For example, you could continue to the next iteration of the loop
            //            continue;
            //        }

            //        contributions.Add(new Contribution
            //        {
            //            Title = $"AI in business {i}",
            //            Status = SD.Status_Pending,
            //            SubmissionDate = DateTime.Now,
            //            UserId = userId,
            //            MagazineId = 2
            //        });
            //    }

            //    _db.Contributions.AddRange(contributions);
            //    _db.SaveChanges();
            //}

            //// Loop for create feedbacks
            //var feedbacks = new List<Feedback>();

            //// Create feedback for contributions
            //for (int i = 2; i < 20; i++)
            //{
            //    feedbacks.Add(new Feedback
            //    {
            //        Comment = "good",
            //        FeedbackDate = DateTime.Now,
            //        UserId = "CoordinatorID1",
            //        ContributionId = i // Assuming the ContributionId starts from 2
            //    });
            //}

            //// Create feedback for business
            //for (int i = 25; i <= 60; i++)
            //{
            //    feedbacks.Add(new Feedback
            //    {
            //        Comment = "good",
            //        FeedbackDate = DateTime.Now,
            //        UserId = "CoordinatorID1",
            //        ContributionId = i // Assuming the ContributionId starts from 30
            //    });
            //}

            //_db.Feedbacks.AddRange(feedbacks);
            //_db.SaveChanges();

            //-------------------------------------------------------------------------------------------------------------------


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

            if (!_db.Semesters.Any())
            {
                var semesters = new List<Semester>
                {
                    new Semester { Id = 1, Name="Spring Term 2024", StartDate = new DateTime(2024,1,15, 0, 0, 0), EndDate = new DateTime(2024, 4,5)},
                    new Semester { Id = 2, Name="Summer Term 2024", StartDate = new DateTime(2024,4,22, 0, 0, 0), EndDate = new DateTime(2024, 7,19)},
                    new Semester { Id = 3, Name="Autumn Term 2024", StartDate = new DateTime(2024,9,25, 0, 0, 0), EndDate = new DateTime(2024, 12,15)},
                    new Semester { Id = 4, Name="Fall Term 2024", StartDate = new DateTime(2023,9,4, 0, 0, 0), EndDate = new DateTime(2023, 12,21)}
                };

                _db.Semesters.AddRange(semesters);
                _db.SaveChanges();
            }

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
                       Description = "This is business magazine",
                       StartDate = DateTime.Now.AddDays(-7),
                       EndDate = DateTime.Now.AddDays(7),
                       FacultyId = 3,
                       SemesterId = 1
                   }
                };

                _db.Magazines.AddRange(magazines);
                _db.SaveChanges();
            }


            //if (!_db.Contributions.Any())
            //{
            //    // create some contributions
            //    var contributions = new List<Contribution>
            //    {
            //        new Contribution
            //        {
            //            Id = 1,
            //            Title = "The Future of AI",
            //            Status = SD.Status_Pending,
            //            SubmissionDate = DateTime.Now,
            //            UserId = "StudentID1",
            //            MagazineId = 1
            //        }
            //    };

            //    _db.Contributions.AddRange(contributions);
            //    _db.SaveChanges();
            //}

            //if (!_db.Documents.Any())
            //{
            //    // create some documents
            //    var documents = new List<Document>();

            //    for (int i = 1; i <= 70; i++)
            //    {
            //        var document = new Document
            //        {
            //            Id = i,
            //            Type = SD.Document_Type_Word,
            //            DocumentUrl = $"~/contributions/UserID{i}/File.doc",
            //            ContributionId = i
            //        };

            //        documents.Add(document);
            //    }

            //    _db.Documents.AddRange(documents);
            //    _db.SaveChanges();
            //}

            //if (!_db.Documents.Any())
            //{
            //    // create some documents
            //    var documents = new List<Document>
            //    {
            //        new Document
            //        {
            //            Id = 1,
            //            Type = SD.Document_Type_Word,
            //            DocumentUrl = "~/contributions/StudentID1/File.doc",
            //            ContributionId = 1
            //        }
            //    };
            //    _db.Documents.AddRange(documents);
            //    _db.SaveChanges();
            //}
        }

    }
}