using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreLib.Inventory.Models.Users
{
    public class AppUserRole
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }
}