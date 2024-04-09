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
        public IUserRepository User { get; private set; }
        public ISemesterRepository Semester { get; private set; }
        public IMagazineRepository Magazine { get; private set; }
        public IContributionRepository Contribution { get; private set; }
        public IDocumentRepository Document { get; private set; }
        public IFeedbackRepository Feedback { get; private set; }
        public INotificationRepository Notification { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Faculty = new FacultyRepository(_db);
            User = new UserRepository(_db);
            Semester = new SemesterRepository(_db);
            Magazine = new MagazineRepository(_db);
            Contribution = new ContributionRepository(_db);
            Document = new DocumentRepository(_db);
            Feedback = new FeedbackRepository(_db);
            Notification = new NotificationRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
        public async Task<int> SaveAsync()
        {
            return await _db.SaveChangesAsync();
        }
    }
}
