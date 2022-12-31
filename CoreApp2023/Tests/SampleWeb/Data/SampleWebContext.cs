//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore;
//using JobsV1.Models;

//namespace SampleWeb.Data
//{
//    public class SampleWebContext : DbContext
//    {
//        public SampleWebContext (DbContextOptions<SampleWebContext> options)
//            : base(options)
//        {
//        }

//        public DbSet<JobsV1.Models.Supplier> Suppliers { get; set; } = default!;
//        public DbSet<JobsV1.Models.City> Cities { get; set; } = default!;
//        public DbSet<JobsV1.Models.Country> Countries{ get; set; } = default!;
//        public DbSet<JobsV1.Models.SupplierType> SupplierTypes { get; set; } = default!;

//    }
//}
