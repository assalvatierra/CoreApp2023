using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.SysDB
{
    public class SysDBContext : DbContext
    {
        public SysDBContext(DbContextOptions<SysDBContext> options)
            : base(options)
        {
        }

        //public DbSet<SysService> SysServices { get; set; } = default!;
        //public DbSet<City> Cities { get; set; } = default!;
        //public DbSet<Country> Countries { get; set; } = default!;
        //public DbSet<SupplierType> SupplierTypes { get; set; } = default!;
        public virtual DbSet<SysService>? SysServices { get; set; }
        //public virtual DbSet<EntBusiness> EntBusinesses { get; set; }
        //public virtual DbSet<EntServices> EntServices { get; set; }
        //public virtual DbSet<SysSetupType> SysSetupTypes { get; set; }
        //public virtual DbSet<EntSupportFile> EntSupportFiles { get; set; }
        //public virtual DbSet<EntAddress> EntAddresses { get; set; }
        //public virtual DbSet<EntContact> EntContacts { get; set; }
        //public virtual DbSet<SysMenu> SysMenus { get; set; }
        //public virtual DbSet<SysServiceMenu> SysServiceMenus { get; set; }
        //public virtual DbSet<SysAccessUser> SysAccessUsers { get; set; }
        //public virtual DbSet<SysAccessRole> SysAccessRoles { get; set; }
        //public virtual DbSet<SysCmdIdRef> SysCmdIdRefs { get; set; }
        //public virtual DbSet<EntSetting> EntSettings { get; set; }
        //public virtual DbSet<SysSetting> SysSettings { get; set; }

    }
}
