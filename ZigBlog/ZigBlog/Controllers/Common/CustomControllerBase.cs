using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Database;
using ZigBlog.Models;

namespace ZigBlog.Controllers.Common
{
    public class CustomControllerBase : Controller
    {
        private User _currentUser;

        public CustomControllerBase()
        {
            ViewBag.Parameters = ZigBlogDb.Parameters;
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null && User != null)
                {
                    var filter = Builders<User>.Filter.Eq(u => u.UsernameUpper, User.Identity.Name.ToUpper());
                    var task = ZigBlogDb.Users.Find(filter).FirstAsync();

                    task.Wait();

                    _currentUser = task.Result;
                }

                return _currentUser;
            }
        }
    }
}