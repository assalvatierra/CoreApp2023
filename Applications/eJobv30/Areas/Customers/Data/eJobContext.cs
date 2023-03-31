using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;

namespace eJobv30.Data
{
    public class eJobContext : DbContext
    {
        public eJobContext (DbContextOptions<eJobContext> options)
            : base(options)
        {
        }

        public DbSet<RealSys.CoreLib.Models.Erp.CustCategory> CustCategory { get; set; } = default!;
    }
}
