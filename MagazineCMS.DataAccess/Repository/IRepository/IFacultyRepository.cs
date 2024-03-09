using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IFacultyRepository : IRepository<Faculty>
    {
        void Update(Faculty obj);
    }
}
