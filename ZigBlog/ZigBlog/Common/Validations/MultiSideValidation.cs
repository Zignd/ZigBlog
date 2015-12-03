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

namespace ZigBlog.Common.Validations
{
    public static class MultiSideValidation
    {
        public static bool UniqueUsername(string username)
        {
            var filter = Builders<AppUser>.Filter.Eq(x => x.UserNameLower, username.ToLower());
            var task = ZigBlogDb.Users.Find(filter).SingleOrDefaultAsync();

            task.Wait();

            return task.Result == null;
        }

        public static bool UniqueEmailAddress(string emailAddress)
        {
            var filter = Builders<AppUser>.Filter.Eq(x => x.Email, emailAddress.ToLower());
            var task = ZigBlogDb.Users.Find(filter).SingleOrDefaultAsync();

            task.Wait();

            return task.Result == null;
        }
    }
}