using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IContributionRepository : IRepository<Contribution>
    {
        Contribution GetFirstOrDefault(Expression<Func<Contribution, bool>> filter = null, string includeProperties = null);
        Task<Contribution> GetFirstOrDefaultAsync(Expression<Func<Contribution, bool>> filter = null, string includeProperties = null);
        void Update(Contribution obj);
    }
}
