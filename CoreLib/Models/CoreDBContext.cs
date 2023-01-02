using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;

namespace CoreLib.Models
{
    public class CoreDBContext : DbContext
    {
        public CoreDBContext(DbContextOptions<CoreDBContext> options)
            : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; } = default!;
        public DbSet<City> Cities { get; set; } = default!;
        public DbSet<Country> Countries { get; set; } = default!;
        public DbSet<SupplierType> SupplierTypes { get; set; } = default!;
    }
}
