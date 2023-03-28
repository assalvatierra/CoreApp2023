using System.Web;
using System.Web.Mvc;

namespace RealSys.CoreLib.Models.Erp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
