using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.SysDB;

namespace RealSys.CoreLib.Interfaces.System
{
    public interface ISystemServices
    {
        public IQueryable<SysService> getServices(int _userId);
        public string getModuleLink(int Id);
    }
}
