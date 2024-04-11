using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IDocumentRepository : IRepository<Document>
    {
        void Update(Document obj);
        Document GetFirstOrDefault(Expression<Func<Document, bool>> filter = null, string includeProperties = null);
    }
}
