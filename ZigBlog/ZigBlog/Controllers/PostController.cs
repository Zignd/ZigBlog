using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    public class PostController : CustomControllerBase
    {
        public ActionResult New()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> New(PostNewViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var post = new Post
            {
                BloggerId = IdentityHelper.CurrentUser.Id,
                TitleUrl = await GenerateTitleUrl(viewModel.Title),
                Title = viewModel.Title,
                Content = viewModel.Content,
                Created = DateTime.Now
            };

            await ZigBlogDb.Posts.InsertOneAsync(post);

            return RedirectToAction("Page", "Home");
        }

        private async Task<string> GenerateTitleUrl(string title)
        {
            title = title.Replace(" ", "_").ToLower();

            var titleUrl = title;
            var counter = 1;

            while (await ZigBlogDb.Posts.Find(Builders<Post>.Filter.Eq(p => p.TitleUrl, titleUrl)).CountAsync() > 0)
                titleUrl = string.Format($"{title}_{counter++}");

            return titleUrl;
        }
    }
}