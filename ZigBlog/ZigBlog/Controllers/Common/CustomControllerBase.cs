using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Database;
using ZigBlog.Common.Identity;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;

namespace ZigBlog.Controllers.Common
{
    public class CustomControllerBase : Controller
    {
        private IAuthenticationManager _authManager;
        private AppUserManager _userManager;
        private AppRoleManager _roleManager;
        
        protected IAuthenticationManager AuthManager
        {
            get
            {
                if (_authManager == null)
                    _authManager = HttpContext.GetOwinContext().Authentication;

                return _authManager;
            }
        }

        protected AppUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();

                return _userManager;
            }
        }

        protected AppRoleManager RoleManager
        {
            get
            {
                if (_roleManager == null)
                    _roleManager = HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();

                return _roleManager;
            }
        }
    }
}