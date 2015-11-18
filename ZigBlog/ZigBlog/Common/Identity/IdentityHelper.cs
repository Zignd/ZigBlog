using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Common.Identity
{
    public static class IdentityHelper
    {
        private static AppUserManager _userManager;

        public static AppUserManager UserManager
        {
            get
            {
                if (_userManager == null)
                    _userManager = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();

                return _userManager;
            }
        }
    }
}