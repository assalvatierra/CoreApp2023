
//using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreLib.Inventory.Models.Users
{ 
    public class AppUser
    {
            public string Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
    }
}