using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealSys.CoreLib.Models.SysDB;
using RealSys.CoreLib.Interfaces;
using RealSys.CoreLib.Interfaces.System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;

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
            List<int> sm = this.context.SysServiceMenus.Where(d => d.SysServiceId == s.Id).Select(s=>s.SysMenuId).ToList<int>();
            var m = this.context.SysMenus.Where(m => sm.Contains(m.Id) && m.ParentId==0).FirstOrDefault();

            string slink = (m != null ? "/" + m.Controller + "/" + m.Action : "");
            return slink;
        }


        public List<RealSys.Modules.SysLib.Models.MenuItem> GetMenuByName(string menu, string User)
        {

            var menuId = context.SysMenus.Where(s => s.Menu == menu && s.ParentId == 0).FirstOrDefault();

            if (menuId == null)
            {
                return new List<RealSys.Modules.SysLib.Models.MenuItem>();
            }

            var  MenuItem = new List<RealSys.Modules.SysLib.Models.MenuItem>();

            var UserMenuListIds = context.SysAccessUsers.Where(s => s.UserId == User).Select(s => s.SysMenuId);
            var MenuList = context.SysMenus.Where(s => (s.Id == menuId.Id || s.ParentId == menuId.Id) && UserMenuListIds.Contains(s.Id)).ToList();

            MenuList.Where(c => c.ParentId == 0).ToList().ForEach(m =>
            {
                var param = m.Params == null ? "" : "?" + m.Params;

                MenuItem.Add(
                   new RealSys.Modules.SysLib.Models.MenuItem()
                   {
                       Id = m.Id,
                       OrderNo = m.Seqno,
                       MenuName = m.Menu,
                       MenuNameHTMLId = m.Menu.Replace(' ', '-'),
                       Route = m.Controller + "/" + m.Action + param,
                       SubMenuItems = GetSubMenuItems(MenuList, m.Id)

                   });
            });

            return MenuItem;
        }



        public List<RealSys.Modules.SysLib.Models.SubMenuItem> GetSubMenuItems(List<SysMenu> MenuList, int MenuId)
        {

            List<RealSys.Modules.SysLib.Models.SubMenuItem> SubMenuItems = new List<RealSys.Modules.SysLib.Models.SubMenuItem>();

            MenuList.Where(c => c.ParentId == MenuId).ToList().ForEach(m => {
                var param = m.Params == null ? "" : "?" + m.Params;

                SubMenuItems.Add(
                  new RealSys.Modules.SysLib.Models.SubMenuItem()
                  {
                      Id = m.Id,
                      OrderNo = m.Seqno,
                      MenuName = m.Menu,
                      Route = m.Controller + "/" + m.Action + param

                  });
            });

            if (SubMenuItems.Count == 0)
            {
                return null;
            }

            return SubMenuItems;
        }

    }
}
