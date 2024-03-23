using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.EntityFrameworkCore;

namespace MagazineCMS.DataAccess.Repository
{
    public class ContributionRepository : Repository<Contribution>, IContributionRepository
    { 
        private ApplicationDbContext _db;

        public ContributionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public Contribution GetFirstOrDefault(Expression<Func<Contribution, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Contribution> query = _db.Set<Contribution>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }
        public async Task<Contribution> GetFirstOrDefaultAsync(Expression<Func<Contribution, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Contribution> query = _db.Set<Contribution>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync();
        }
        public void Update(Contribution obj)
        {
            _db.Contributions.Update(obj);
        }
    }
}
