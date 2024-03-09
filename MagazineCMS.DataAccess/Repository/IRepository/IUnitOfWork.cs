using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IFacultyRepository Faculty { get; }
        IUserRepository User { get; }

        void Save();
    }
}
