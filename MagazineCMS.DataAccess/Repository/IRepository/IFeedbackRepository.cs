using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.Models;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        void Update(Feedback obj);
    }
}
