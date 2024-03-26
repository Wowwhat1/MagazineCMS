using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.DataAccess.Data;
using MagazineCMS.DataAccess.Repository.IRepository;
using MagazineCMS.Models;
using Microsoft.EntityFrameworkCore;

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

        public Document GetFirstOrDefault(Expression<Func<Document, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<Document> query = _db.Documents;

            // Apply filtering if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related entities if provided
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            // Return the first document matching the filter
            return query.FirstOrDefault();
        }

    }
}
