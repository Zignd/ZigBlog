using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Models;

namespace ZigBlog.Common.Validation
{
    public static class MultiSideValidation
    {
        public static bool UniqueUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UsernameUpper, username.ToUpper());
            var task = ZigBlogDb.Users.Find(filter).CountAsync();

            task.Wait();

            return task.Result == 0;
        }
    }
}