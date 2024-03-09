using System;
using System.Collections.Generic;
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
            catch (Exception ex) { }

            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Student).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Student)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Coordinator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Manager)).GetAwaiter().GetResult();

                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new User
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Firstname = "Admin",
                    Lastname = "PublishPro",
                    PhoneNumber = "1112223333",
                    AvatarUrl = "test 123 Ave",
                    FacultyId = 1,
                }, "Admin123*").GetAwaiter().GetResult();

                User user = _db.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
