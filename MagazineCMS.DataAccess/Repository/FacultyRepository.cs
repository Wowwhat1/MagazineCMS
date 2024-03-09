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
    public class FacultyRepository : Repository<Faculty>, IFacultyRepository
    { 
        private ApplicationDbContext _db;

        public FacultyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Faculty obj)
        {
            _db.Faculties.Update(obj);
        }
    }
}
