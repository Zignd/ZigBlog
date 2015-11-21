using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Database;
using ZigBlog.Common.Identity;
using ZigBlog.Controllers.Common;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;

namespace ZigBlog.Controllers
{
    public class HomeController : CustomControllerBase
    {
        // GET: / or /Page/{number}
        public async Task<ActionResult> Page(int arg = 1, int postsPerPage = 10)
        {
            var sort = Builders<Post>.Sort.Descending(p => p.Created);
            
            var viewModel = new HomePageViewModel
            {
                Posts = await ZigBlogDb.Posts.Find(_ => true).Sort(sort).Skip((arg - 1) * postsPerPage).Limit(postsPerPage).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /About
        public async Task<ActionResult> About()
        {
            var viewModel = new HomeAboutViewModel
            {
                AboutContent = ZigBlogDb.Parameters.AboutContent
            };

            return View(viewModel);
        }
    }
}