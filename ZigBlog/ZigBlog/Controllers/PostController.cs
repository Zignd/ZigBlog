using MarkdownDeep;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Database;
using ZigBlog.Common.Identity;
using ZigBlog.Controllers.Common;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;
using ZigBlog.Translations;

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
                ParsedContent = (new Markdown()).Transform(viewModel.Content),
                Created = DateTime.Now
            };

            await ZigBlogDb.Posts.InsertOneAsync(post);

            return RedirectToAction("Page", "Home");
        }

        [AllowAnonymous]
        public async Task<ActionResult> Show(string titleUrl)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl.ToLower());
            var post = await ZigBlogDb.Posts.Find(filter).FirstOrDefaultAsync();

            if (post == null)
                throw new Exception(Translation.ThisPostCouldNotBeFoundException);

            return View(new PostShowViewModel
            {
                Post = post
            });
        }

        private async Task<string> GenerateTitleUrl(string title)
        {
            // Remove accents
            var bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(title);
            title = System.Text.Encoding.ASCII.GetString(bytes);

            // Lower cases the title
            title = title.ToLower();

            // Remove invalid characters
            title = Regex.Replace(title, @"[^a-z0-9\s-]", "");

            // Converts multiple spaces into one space
            title = Regex.Replace(title, @"\s+", " ").Trim();

            // Limits the title url to 45 characters
            title = title.Substring(0, title.Length <= 45 ? title.Length : 45).Trim();

            // Convertes the spaces into hyphens
            title = Regex.Replace(title, @"\s", "-");

            var titleUrl = title;
            var counter = 1;

            while (await ZigBlogDb.Posts.Find(Builders<Post>.Filter.Eq(p => p.TitleUrl, titleUrl)).CountAsync() > 0)
                titleUrl = string.Format($"{title}-{counter++}");

            return titleUrl;
        }
    }
}