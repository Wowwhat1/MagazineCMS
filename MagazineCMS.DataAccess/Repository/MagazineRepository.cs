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

        public void Update(Magazine obj)
        {
            _db.Magazines.Update(obj);
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