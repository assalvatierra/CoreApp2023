using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using JobsV1.Models;

namespace WebDemo.Data
{
    public class WebDemoContext : DbContext
    {
        public WebDemoContext (DbContextOptions<WebDemoContext> options)
            : base(options)
        {
        }

        public DbSet<JobsV1.Models.Supplier> Supplier { get; set; } = default!;
    }
}
