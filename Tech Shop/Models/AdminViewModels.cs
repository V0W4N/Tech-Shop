using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tech_Shop.App_Start;

namespace Tech_Shop.Models
{
        
    public class AdminViewModel
    {
        public AdminViewModel() { }
        public List<ApplicationUser> UserList { get; set; }

        
    }
    public class ChangeName
    {
        [Required]
        [Display(Name = "Current Name")]
        public string Name { get; set; }
    }
}