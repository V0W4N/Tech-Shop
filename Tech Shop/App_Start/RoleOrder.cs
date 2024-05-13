using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
        public static Boolean CheckForRoles(IPrincipal user, string roleName)
        {
            var currRole = 0;
            int targetRole = Roles.IndexOf(roleName);
            foreach (var role in Roles)
            {
                if (user.IsInRole(role))
                {
                    currRole = Roles.IndexOf(role);
                }
            }
            return currRole>=targetRole;
        }
    }
}