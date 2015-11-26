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

namespace ZigBlog.Common.Validation
{
    public class AdministratorOrAuthorAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.User.Identity.IsAuthenticated)
            {
                var titleUrl = httpContext.Request.RequestContext.RouteData.Values["titleUrl"].ToString();

                if (titleUrl == null)
                    throw new Exception("The AdministratorOrAuthorAttribute requires a post's title URL as a route data in order to perform the authentication");
                
                // Looks for a post in which the current user is the author and which has the provided title URL
                var filter = Builders<Post>.Filter.Eq(x => x.BloggerId, IdentityHelper.CurrentUser.Id) & Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl);
                var task = ZigBlogDb.Posts.Find(filter).CountAsync();

                task.Wait();

                // Check if current user is in the "Administrator" role or if the current user is the author of the post,
                // is so, the user is allowed to edit the post
                if (httpContext.User.IsInRole("Administrator") || task.Result != 0)
                    return true;
            }

            return false;
        }
    }
}