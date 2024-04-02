using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User obj)
        {
            _db.Users.Update(obj);
        }

        public IEnumerable<User> GetUserByFacultyIdAndRole(int facultyId, string roleName)
        {
            var query = from userRole in _db.UserRoles
                        join user in _db.Users on userRole.UserId equals user.Id
                        join role in _db.Roles on userRole.RoleId equals role.Id
                        where user.FacultyId == facultyId && role.Name == roleName
                        select user;

            return query.ToList();
        }
    }
}
