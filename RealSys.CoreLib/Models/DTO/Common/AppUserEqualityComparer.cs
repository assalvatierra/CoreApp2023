using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.CoreLib.Models.DTO.Common
{
    public class AppUserEqualityComparer : IEqualityComparer<AppUser>
    {
        public bool Equals(AppUser x, AppUser y)
        {
            if (Object.ReferenceEquals(x, y)) return true;

            if (Object.ReferenceEquals(x, null) ||

                Object.ReferenceEquals(y, null))

                return false;

            return x.UserName == y.UserName;
        }

        public int GetHashCode(AppUser appuser)
        {
            if (Object.ReferenceEquals(appuser, null)) return 0;

            int hashTextual = appuser.UserName == null

                ? 0 : appuser.UserName.GetHashCode();

            int hashDigital = appuser.UserName.GetHashCode();

            return hashTextual ^ hashDigital;
        }
    }
}
