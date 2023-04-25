using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealSys.CoreLib.Models.Erp;

    public class ErpContactsContext : DbContext
    {
        public ErpContactsContext (DbContextOptions<ErpContactsContext> options)
            : base(options)
        {
        }

    public DbSet<RealSys.CoreLib.Models.Erp.Customer> Customers { get; set; } = default!;

    public DbSet<RealSys.CoreLib.Models.Erp.CustCategory> CustCategory { get; set; } = default!;

    public DbSet<RealSys.CoreLib.Models.Erp.CustEntCat>? CustEntCat { get; set; }

    public DbSet<RealSys.CoreLib.Models.Erp.CustEntClauses>? CustEntClauses { get; set; }

    public DbSet<RealSys.CoreLib.Models.Erp.CustEntAddress>? CustEntAddress { get; set; }

    public DbSet<RealSys.CoreLib.Models.Erp.SupplierItem>? SupplierItem { get; set; }

    public DbSet<RealSys.CoreLib.Models.Erp.Supplier>? Supplier { get; set; }



}
