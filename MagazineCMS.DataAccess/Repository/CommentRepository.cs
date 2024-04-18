using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagazineCMS.DataAccess.Repository
{
    internal class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private ApplicationDbContext _db;

        public CommentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Comment obj)
        {
            _db.Comments.Update(obj);
        }
    }
}
