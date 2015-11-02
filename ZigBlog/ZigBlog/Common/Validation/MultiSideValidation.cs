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
        public static async Task<bool> UniqueUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UsernameUpper, username.ToUpper());
            bool isUniqueUsername = (await ZigBlogDb.Users.Find(filter).CountAsync() == 0);

            return isUniqueUsername;
        }
    }
}