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
    public class DocumentRepository : Repository<Document>, IDocumentRepository
    { 
        private ApplicationDbContext _db;

        public DocumentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Document obj)
        {
            _db.Documents.Update(obj);
        }
    }
}
