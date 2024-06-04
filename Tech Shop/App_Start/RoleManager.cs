using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.WebPages;
using Tech_Shop.Models;

namespace Tech_Shop.App_Start
{
    public class RoleManager
    {
        public static readonly List<string> Roles = new List<string>
        {
        "User",
        "Moderator",
        "Admin",
        "PowerUser"
        };


        public static List<SelectListItem> GetRoleListForUser(IPrincipal User)
        {
            List<SelectListItem> list = new List<SelectListItem> { };
            foreach(var role in Roles)
            {
                if (Roles.IndexOf(role) < GetUserRoleId(User)){
                    list.Add(new SelectListItem()
                    {
                        Text = role,
                        Value = role,
                        Selected = false
                    });
                }
            }
            return list;
        }
        public static List<SelectListItem> GetRoleListForUser(ApplicationUser User)
        {
            List<SelectListItem> list = new List<SelectListItem> { };
            foreach (var role in Roles)
            {
                if (Roles.IndexOf(role) < GetUserRoleId(User))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = role,
                        Value = role,
                        Selected = false
                    });
                }
            }
            return list;
        }
        public static string GetRoleString(int id)
        {
            return Roles[id];
        }

        public static int GetUserRoleId(IPrincipal User)
        {
            var maxRole = 0;
            foreach (var role in Roles)
            {
                if (User.IsInRole(role))
                {
                    maxRole = Roles.IndexOf(role) > maxRole ? Roles.IndexOf(role) : maxRole;
                }
            }
            return maxRole;
        }
        public static int GetUserRoleId(ApplicationUser User)
        {
            var maxRole = 0;
            if (User.Roles.Count > 0)
            {
                foreach (var role in Roles)
                {
                    if (GetRoleString(User.Roles.First().RoleId.AsInt()) == role)
                    {
                        maxRole = Roles.IndexOf(role) >= maxRole ? Roles.IndexOf(role) : maxRole;
                    }
                }
            }
            return maxRole;
        }

        public static string GetUserRoleStr(ApplicationUser user)
        {
            return GetRoleString(GetUserRoleId(user));
        }
        public static string GetUserRoleStr(IPrincipal user)
        {
            return GetRoleString(GetUserRoleId(user));
        }
        // @RoleManager.GetRoleString(Model.User.Roles)
        public static string GetUserRoleStr(ICollection<IdentityUserRole> roleList)
        {
            if (roleList.Count > 0)
            {
                return GetRoleString(roleList.First().RoleId.AsInt());
            }
            else
            {
                return GetRoleString(0);
            }
        }
        public static Boolean CheckForRoles(ApplicationUser user, string roleName)
        {
            int targetRole = Roles.IndexOf (roleName);
            var maxRole = GetUserRoleId(user);
            return maxRole >= targetRole;
        }
        public static Boolean CheckForRoles(IPrincipal user, string roleName)
        {
            int targetRole = Roles.IndexOf(roleName);
            var maxRole = GetUserRoleId(user);
            return maxRole >= targetRole;
        }

        public static Boolean RoleExists(string roleName)
        {
            return Roles.Contains(roleName);
        }

    }
}