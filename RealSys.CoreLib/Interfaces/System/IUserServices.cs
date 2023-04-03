using System.Collections.Generic;
using RealSys.CoreLib.Models.Security;

namespace RealSys.CoreLib.Interfaces.System
{
    public interface IUserServices
    {
        public List<AppUser> GetUserList();
    }
}
