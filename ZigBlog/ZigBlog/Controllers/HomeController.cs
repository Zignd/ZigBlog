using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Database;
using ZigBlog.Controllers.Common;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;

namespace ZigBlog.Controllers
{
    public class HomeController : CustomControllerBase
    {
        // GET: / or /Page/{number}
        public async Task<ActionResult> Page(int number = 1)
        {
            try
            {
                // TODO: Make use of the number parameter to perform pagination.
                var sort = Builders<Post>.Sort.Descending(p => p.Created);

                var viewModel = new HomePageViewModel
                {
                    Posts = await ZigBlogDb.Posts.Find(_ => true).Sort(sort).ToListAsync()
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new SharedErrorViewModel(ex));
            }
        }

        // GET: /About
        public ActionResult About()
        {
            try
            {
                var viewModel = new HomeAboutViewModel
                {
                    AboutContent = ZigBlogDb.Parameters.AboutContent
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                return View("Error", new SharedErrorViewModel(ex));
            }
        }
    }
}