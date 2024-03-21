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
        ISemesterRepository Semester { get; }
        IMagazineRepository Magazine { get; }
        IContributionRepository Contribution { get; }
        IDocumentRepository Document { get; }
        IFeedbackRepository Feedback { get; }

        void Save();
        Task<int> SaveAsync();

    }
}
