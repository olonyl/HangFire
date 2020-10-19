using HangFire.Domain.Repositories;
using HangFire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangFire.Infrastructure.Repositories
{
    public class ScanningUrlRepository : EfRepository<ScanningUrl>, IScanningUrlRepository
    {
        ApplicationContext db;
        public ScanningUrlRepository(ApplicationContext db) : base(db)
        {
            this.db = db;
        }

        public IQueryable<ScanningUrl> TakeAll(int maxRecords)
        {
            return db.ScanningUrl.Take(maxRecords);
        }
    }
}
