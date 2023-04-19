using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealSys.Modules.SysLib.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? MenuName { get; set; }
        public int OrderNo { get; set; }
        public string? Route { get; set; }
        public List<SubMenuItem> SubMenuItems { get; set; }

    }

    public class SubMenuItem
    {
        public int Id { get; set; }
        public int MainMenuId { get; set; }
        public string? MenuName { get; set; }
        public int OrderNo { get; set; }
        public string? Route { get; set; }

    }
}

