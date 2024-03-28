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
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    { 
        private ApplicationDbContext _db;

        public NotificationRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Notification obj)
        {
            _db.Notifications.Update(obj);
        }
    }
}
