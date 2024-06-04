using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Tech_Shop.App_Start;
using Tech_Shop.Models;

namespace Tech_Shop.Controllers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // User is not authenticated, redirect to login page or show custom error message
                filterContext.Result = new HttpUnauthorizedResult("You are not authenticated.");
            }
            else
            {
                // User is authenticated but doesn't have the required role, show custom error message
                filterContext.Result = new HttpStatusCodeResult(403, "You are not authorized to access this resource.");
            }
        }
    }
    
    [AdminAuthorize(Roles = "Moderator,Admin,PowerUser")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public AdminController()
        {
        }


        public AdminController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var model = new AdminViewModel();
            model.UserList = db.Users.ToList();
            return View(model);
        }

        public ActionResult ManageUser(string UId)
        {
            ManageUserModel model = new ManageUserModel();
            model.User = db.Users.FirstOrDefault(u => u.Id == UId);
            model.Manager = User;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeRole(ApplicationUser ManagedUser, string role)
        {
            if (RoleManager.RoleExists(role)){
                await UserManager.RemoveFromRolesAsync(ManagedUser.Id, RoleManager.GetUserRoleStr(ManagedUser));
                await UserManager.AddToRoleAsync(ManagedUser.Id, role);
            }
            else
            {
                throw new Exception("Role doesn't exist");
            }
            return RedirectToAction("Index", "Admin");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}