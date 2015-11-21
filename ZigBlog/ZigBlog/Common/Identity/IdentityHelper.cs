using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Models;

namespace ZigBlog.Common.Identity
{
    public static class IdentityHelper
    {
        public static AppUser CurrentUser
        {
            get
            {
                var task = UserManager.FindByNameAsync(HttpContext.Current.User.Identity.Name);

                task.Wait();

                return task.Result;
            }
        }

        public static AppUserManager UserManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}