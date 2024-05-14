using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Tech_Shop.App_Start;

namespace Tech_Shop.Models
{
        
    public class AdminViewModel
    {
        public AdminViewModel() { }
        public List<ApplicationUser> UserList { get; set; }

        
    }
    public class ManageUserModel
    {
        public ApplicationUser User { get; set; }
        public IPrincipal Manager { get; set; }
        public string Role { get; set; }
        public ManageUserModel() { }
    }
}