using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Tech_Shop.App_Start
{
    public class RoleOrder
    {
        public static readonly List<string> Roles = new List<string>
        {
        "User",
        "Moderator",
        "Admin",
        "PowerUser"
        };
        public static string GetRole(int id)
        {
            return Roles[id];
        }
    }
}