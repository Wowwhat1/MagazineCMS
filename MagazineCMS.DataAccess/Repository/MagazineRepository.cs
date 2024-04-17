using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using MagazineCMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MagazineCMS.DataAccess.Repository
{
    public class MagazineRepository : Repository<Magazine>, IMagazineRepository
    {
        private ApplicationDbContext _db;

        public MagazineRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Magazine obj)
        {
            _db.Magazines.Update(obj);
        }

        public IEnumerable<Magazine> GetAllMagazineWithPublicContributions()
        {
            var query = from magazine in _db.Magazines
                .Include(m => m.Faculty)
                .Include(m => m.Semester)
                        let publicContributions = magazine.Contributions.Where(c => c.Status == SD.Status_Public)
                        where publicContributions.Any()
                        select new Magazine
                        {
                            Id = magazine.Id,
                            Name = magazine.Name,
                            Description = magazine.Description,
                            StartDate = magazine.StartDate,
                            EndDate = magazine.EndDate,
                            Faculty = magazine.Faculty,
                            Semester = magazine.Semester,
                            Contributions = publicContributions.ToList()
                        };

            return query.ToList();
        }
    }
}