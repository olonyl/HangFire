using HangFire.Domain.Seedwork;
using HangFire.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HangFire.Infrastructure
{
    public class ApplicationContext : DbContext
    {
        public DbSet<ScanningUrl> ScanningUrl { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
           : base(options)
        {
        }     
    }
}
