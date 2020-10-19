using HangFire.Domain.Seedwork;
using HangFire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HangFire.Domain.Repositories
{
    public interface IScanningUrlRepository : IAsyncRepository<ScanningUrl>
    {
        IQueryable<ScanningUrl> TakeAll(int maxRecords);
    }
}
