using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;

namespace MagazineCMS.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public IFacultyRepository Faculty { get; private set; } 

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Faculty = new FacultyRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
