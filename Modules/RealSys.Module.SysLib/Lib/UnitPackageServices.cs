using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.UnitPackages;
using RealSys.CoreLib.Models.Erp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class UnitPackageServices
    {

        ErpDbContext db;
        DateClass datetime;

        public UnitPackageServices(ErpDbContext _context, ILogger _logger)
        {
            db = _context;
            datetime = new DateClass();
        }



        //get Package list per unit used in Packages Rate and reporting
        public List<PackageperUnit> getPackageperUnitList(string status, string package, string unit, string group)
        {
            List<PackageperUnit> UnitPkgList = new List<PackageperUnit>();

            IEnumerable<CarRateUnitPackage> pkglist = db.CarRateUnitPackages.ToList();

            foreach (var list in pkglist)
            {
                int id = db.CarRateGroups.Where(c => c.CarRatePackageId == list.CarRatePackageId).FirstOrDefault() != null ? db.CarRateGroups.Where(c => c.CarRatePackageId == list.CarRatePackageId).FirstOrDefault().RateGroupId : 1;
                RateGroup groupPkg = db.RateGroups.Find(id);

                UnitPkgList.Add(new PackageperUnit
                {
                    Id = list.Id,
                    RateperDay = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Daily,
                    RateperWeek = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Weekly,
                    RateperMonth = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().Monthly,
                    AddOn = (decimal)list.DailyAddon,
                    FuelDaily = list.FuelDaily,
                    FuelLonghaul = list.FuelLonghaul,
                    Meals = list.CarRatePackage.DailyMeals,
                    Acc = list.CarRatePackage.DailyRoom,
                    PkgDesc = list.CarRatePackage.Description,
                    Unit = list.CarUnit.CarRates.Where(s => s.CarUnitId == list.CarUnitId).FirstOrDefault().CarUnit.Description,
                    Group = groupPkg.GroupName,
                    Status = list.CarRatePackage.Status
                });
            }

            UnitPkgList = UnitPkgList.ToList();

            if (status != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Status.ToLower().Contains(status.ToLower())).ToList();
            }

            if (package != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.PkgDesc.ToLower().Contains(package.ToLower())).ToList();
            }

            if (unit != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Unit.ToLower().Contains(unit.ToLower())).ToList();
            }

            if (group != "all")
            {
                UnitPkgList = UnitPkgList.Where(p => p.Group.ToLower().Contains(group.ToLower())).ToList();
            }

            return UnitPkgList;
        }


    }
}
