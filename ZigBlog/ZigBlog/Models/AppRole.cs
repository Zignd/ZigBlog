using AspNet.Identity.MongoDB;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Common.Identity;

namespace ZigBlog.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }

        public List<AppUser> GetUsers()
        {
            var filter = Builders<AppUser>.Filter.AnyIn(x => x.Roles, new string[] { Name });
            var task = ZigBlogDb.Users.Find(filter).ToListAsync();

            task.Wait();

            return task.Result;
        }
    }
}