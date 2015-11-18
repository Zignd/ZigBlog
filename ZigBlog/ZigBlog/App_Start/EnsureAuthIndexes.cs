using AspNet.Identity.MongoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;

namespace ZigBlog
{
    public class EnsureAuthIndexes
    {
        public static void Exist()
        {
            IndexChecks.EnsureUniqueIndexOnUserName(ZigBlogDb.Users);
            IndexChecks.EnsureUniqueIndexOnRoleName(ZigBlogDb.Roles);
        }
    }
}