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
            //migrations if they are not applied
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

            //create roles if they are not created
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

        }

    }
}
