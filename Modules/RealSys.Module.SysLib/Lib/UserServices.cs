using Microsoft.Extensions.Logging;
using RealSys.CoreLib.Models.DTO.Common;
using RealSys.CoreLib.Models.Erp;
using RealSys.CoreLib.Models.SysDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Lib
{
    public class UserServices
    {

        ErpDbContext db;
        SysDBContext sdb;
        DateClass datetime;

        public UserServices(ErpDbContext _context, SysDBContext _sysDBContext, ILogger _logger)
        {
            db = _context;
            sdb = _sysDBContext;
            datetime = new DateClass();
        }

        public IList<AppUser> getUsers()
        {
            var data = db.Database.SqlQuery<AppUser>("Select UserName from AspNetUsers");
            return data.ToList();
        }

        public IList<AppUser> getUsers_wdException()
        {
            var data = db.Database.SqlQuery<AppUser>("Select UserName from AspNetUsers Where UserName NOT IN " +
                " ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', " +
                " 'admin@gmail.com' ,'demo@gmail.com', 'assalvatierra@yahoo.com', 'abel@yahoo.com' " +
                ")");
            return data.ToList();
        }

        public IEnumerable<AppUser> getUsersModules(int moduleId)
        {

            //all users
            List<AppUser> users = getUsers().ToList();

            //active users in the module
            List<AppUser> actUsers = new List<AppUser>();

            //get list of users from sys access
            var modules = sdb.SysAccessUsers.Where(s => s.SysMenuId == moduleId).ToList();

            foreach (var mod in modules)
            {
                actUsers.Add(new AppUser() { UserName = mod.UserId });
                //actUsers.Add(new AppUser() { UserName = mod.UserId + " - " + mod.SysMenuId });
            }

            //users not found in the module
            var appUserEqualityComparer = new AppUserEqualityComparer();
            IEnumerable<AppUser> Eusers = users.Except(actUsers, appUserEqualityComparer).ToList();


            return Eusers;
        }

        public IEnumerable<AppUser> getUsersModulesTest(int moduleId)
        {
            //all users
            List<AppUser> users = getUsers().ToList();

            //active users in the module
            List<AppUser> actUsers = new List<AppUser>();

            //get list of users from sys access
            var modules = sdb.SysAccessUsers.Where(s => s.SysMenuId == moduleId).ToList();

            foreach (var mod in modules)
            {
                actUsers.Add(new AppUser() { UserName = mod.UserId });
                //actUsers.Add(new AppUser() { UserName = mod.UserId + " - " + mod.SysMenuId });
            }

            //users not found in the module
            var appUserEqualityComparer = new AppUserEqualityComparer();
            IEnumerable<AppUser> Eusers = users.Except(actUsers, appUserEqualityComparer).ToList();

            //actUsers.Add(new AppUser() { UserName = "jahdielvillosa@gmail.com" });
            //actUsers.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });

            List<AppUser> actUsers1 = new List<AppUser>();
            List<AppUser> actUsers2 = new List<AppUser>();
            actUsers1.Add(new AppUser() { UserName = "jahdielvillosa@gmail.com" });
            actUsers1.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });
            actUsers1.Add(new AppUser() { UserName = "test@gmail.com" });

            actUsers2.Add(new AppUser() { UserName = "assalvatierra@gmail.com" });

            IEnumerable<AppUser> Exusers = actUsers1.Except(actUsers2);

            //set3 = set1.Where((item) => !set2.Any((item2) => item.id == item2.id));
            appUserEqualityComparer = new CoreLib.Models.DTO.Common.AppUserEqualityComparer();
            var common = actUsers1.Except(actUsers2, appUserEqualityComparer);

            return Eusers;
        }
    }
}
