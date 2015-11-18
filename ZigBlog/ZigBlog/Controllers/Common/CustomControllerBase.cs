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
        private AppUser _currentUser;
        private IAuthenticationManager _authManager;
        private AppUserManager _userManager;

        public CustomControllerBase()
        {
            ViewBag.Parameters = ZigBlogDb.Parameters;
            ViewBag.CurrentUser = CurrentUser;
        }

        public AppUser CurrentUser
        {
            get
            {
                if (_currentUser == null && User != null)
                {
                    var task = UserManager.FindByNameAsync(User.Identity.Name);

                    task.Wait();

                    _currentUser = task.Result;
                }

                return _currentUser;
            }
        }

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
    }
}