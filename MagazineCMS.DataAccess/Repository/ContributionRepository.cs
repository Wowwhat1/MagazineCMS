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
    public class ContributionRepository : Repository<Contribution>, IContributionRepository
    { 
        private ApplicationDbContext _db;

        public ContributionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Contribution obj)
        {
            _db.Contributions.Update(obj);
        }

        public Contribution GetById(int id)
        {
            return _db.Contributions.FirstOrDefault(c => c.Id == id);
        }
    }
}
