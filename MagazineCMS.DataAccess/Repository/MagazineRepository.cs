using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
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

        public void Update(Magazine obj, int Id)
        {
            _db.Magazines.Where(m => m.Id == Id).ExecuteUpdate(setter => setter.SetProperty(m => m.Name, obj.Name)
            .SetProperty(m => m.Description, obj.Description).SetProperty(m => m.StartDate, obj.StartDate)
            .SetProperty(m => m.EndDate, obj.EndDate)
            .SetProperty(m => m.FacultyId, obj.FacultyId)
            .SetProperty(m => m.SemesterId, obj.SemesterId));
        }

        public async Task<IdentityResult> CreateAsync(Magazine magazine)
        {
            try
            {
                _db.Magazines.Add(magazine);
                await _db.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Error while creating magazine: {ex.Message}" });
            }
        }


    }
}