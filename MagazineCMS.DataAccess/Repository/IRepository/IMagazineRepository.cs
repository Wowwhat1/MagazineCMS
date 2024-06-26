﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagazineCMS.Models;
using Microsoft.AspNetCore.Identity;

namespace MagazineCMS.DataAccess.Repository.IRepository
{
    public interface IMagazineRepository : IRepository<Magazine>
    {
        void Update(Magazine obj);

        IEnumerable<Magazine> GetAllMagazineWithPublicContributions();

    }
}
