using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Common.Identity
{
    public static class IdentityHelper
    {
        public static AppUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}