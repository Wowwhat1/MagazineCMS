using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository
{
    public class SemesterRepository : Repository<Semester>, ISemesterRepository
    { 
        private ApplicationDbContext _db;

        public SemesterRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Semester obj)
        {
            _db.Semesters.Update(obj);
        }
    }
}
