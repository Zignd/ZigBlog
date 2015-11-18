using Microsoft.AspNet.Identity.Owin;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Common.Identity;
using ZigBlog.Models;

namespace ZigBlog.Common.Validation
{
    public static class MultiSideValidation
    {
        public static bool UniqueUsername(string username)
        {
            var task = IdentityHelper.UserManager.FindByNameAsync(username);

            task.Wait();

            return task.Result == null;
        }

        public static bool UniqueEmailAddress(string emailAddress)
        {
            var task = IdentityHelper.UserManager.FindByEmailAsync(emailAddress);

            task.Wait();

            return task.Result == null;
        }
    }
}