using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.SysDB;
using RealSys.CoreLib.Interfaces;
using RealSys.CoreLib.Interfaces.System;

namespace RealSys.Modules.SysLib
{
    public class SystemServices: ISystemServices
    {
        private readonly SysDBContext context;
        public SystemServices(SysDBContext _context) { 
            this.context = _context;
        }

        public virtual IQueryable<SysService> getServices(int _userId)
        {
            return this.context.SysServices;
        }

        public virtual string getModuleLink(int Id)
        {
            var s = this.context.SysServices.Find(Id);
            if (s != null) return s.UrlPath;
            else return "";


        }
    }
}
