﻿using MarkdownDeep;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common;
using ZigBlog.Common.Database;
using ZigBlog.Common.Filters;
using ZigBlog.Common.Identity;
using ZigBlog.Common.Validations;
using ZigBlog.Controllers.Common;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;
using ZigBlog.Translations;

namespace ZigBlog.Controllers
{
    public class HomeController : CustomControllerBase
    {
        // GET: / or /page/{arg} or /page/{arg}/{postsPerPage}
        [HandleError]
        public async Task<ActionResult> Page(int page = 1, [Range(1, 50)]int postsPerPage = 15)
        {
            var sort = Builders<Post>.Sort.Descending(p => p.Created);

            var viewModel = new HomePageViewModel
            {
                CurrentPage = page,
                PostsPerPage = postsPerPage,
                TotalPostsCount = await ZigBlogDb.Posts.CountAsync(_ => true),
                Posts = await ZigBlogDb.Posts.Find(_ => true).Sort(sort).Skip((page - 1) * postsPerPage).Limit(postsPerPage).ToListAsync()
            };

            return View(viewModel);
        }
        
        // GET: /{year}/{month}/{day}/{titleUrl}
        [HandleError]
        public async Task<ActionResult> Show(string titleUrl)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl.ToLower());
            var post = await ZigBlogDb.Posts.Find(filter).SingleOrDefaultAsync();

            if (post == null)
                throw new Exception(Translation.ThisPostCouldNotBeFoundException);

            return View(post);
        }

        // POST: /home/postcomment?titleUrl={titleUrl}
        [HttpPost]
        [HandleJsonError]
        [ValidateAjax]
        public async Task<JsonResult> PostComment(HomePostCommentPartialViewModel viewModel)
        {
            var newComment = new Comment
            {
                CommenterId = IdentityHelper.CurrentUser.Id,
                Content = viewModel.Content,
                Created = DateTime.Now,
                IsTopLevel = viewModel.IsTopLevel,
                ParentId = viewModel.ParentId,
                ParsedContent = Markdown.Transform(viewModel.Content),
                PostId = viewModel.PostId
            };

            await ZigBlogDb.Comments.InsertOneAsync(newComment);

            // ASP.NET MVC is pretty evil, check this on SO: http://stackoverflow.com/a/2678956/1324082
            ModelState.Clear();

            // Returns a JSON with the new comment rendered in a string
            // and with some other properties required to properly place this
            // new comment in the comments section
            return Json(new
            {
                Id = newComment.Id,
                ParentId = newComment.ParentId,
                IsTopLevel = newComment.IsTopLevel,
                PartialView = RenderViewToString("_Comment", newComment)
            });
        }

        // GET: /new
        [Authorize(Roles = "Administrator,Blogger")]
        [HandleError]
        public ActionResult New()
        {
            return View("NewEdit", new HomeNewEditViewModel
            {
                IsNewMode = true
            });
        }

        // POST: /new
        [Authorize(Roles = "Administrator,Blogger")]
        [HandleError]
        [HttpPost]
        public async Task<ActionResult> New(HomeNewEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("NewEdit", viewModel);

            var post = new Post
            {
                BloggerId = IdentityHelper.CurrentUser.Id,
                TitleUrl = await GenerateTitleUrl(viewModel.Title),
                Title = viewModel.Title,
                Content = viewModel.Content,
                ParsedContent = Markdown.Transform(viewModel.Content),
                Created = DateTime.Now
            };

            await ZigBlogDb.Posts.InsertOneAsync(post);

            return RedirectToAction("Show", new { titleUrl = post.TitleUrl });
        }

        // GET: /{year}/{month}/{day}/{titleUrl}/edit
        [AdministratorOrAuthor]
        [HandleError]
        public async Task<ActionResult> Edit(string titleUrl)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl.ToLower());
            var post = await ZigBlogDb.Posts.Find(filter).SingleOrDefaultAsync();

            if (post == null)
                throw new Exception(Translation.ThisPostCouldNotBeFoundException);

            return View("NewEdit", new HomeNewEditViewModel
            {
                TitleUrl = post.TitleUrl,
                Title = post.Title,
                Content = post.Content,
                IsNewMode = false
            });
        }

        // POST: /{year}/{month}/{day}/{titleUrl}/edit
        [AdministratorOrAuthor]
        [HandleError]
        [HttpPost]
        public async Task<ActionResult> Edit(HomeNewEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("NewEdit", viewModel);

            var newTitleUrl = await GenerateTitleUrl(viewModel.Title);

            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, viewModel.TitleUrl);
            var update = Builders<Post>.Update.Set(x => x.Title, viewModel.Title)
                                              .Set(x => x.TitleUrl, newTitleUrl)
                                              .Set(x => x.Content, viewModel.Content)
                                              .Set(x => x.ParsedContent, Markdown.Transform(viewModel.Content));
            await ZigBlogDb.Posts.UpdateOneAsync(filter, update);

            return RedirectToAction("Show", new { titleUrl = newTitleUrl });
        }

        // POST: /home/delete?titleUrl={titleUrl}
        [AdministratorOrAuthor]
        [HandleError]
        [HttpPost]
        public async Task<ActionResult> Delete(string titleUrl)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl);
            await ZigBlogDb.Posts.DeleteOneAsync(filter);

            return RedirectToAction("Page");
        }

        // POST: /home/likepost?titleUrl={titleUrl}
        [Authorize]
        [HandleJsonError]
        [HttpPost]
        public async Task<JsonResult> LikePost(string titleUrl)
        {
            var filter = Builders<Post>.Filter.Eq(x => x.TitleUrl, titleUrl);
            var post = await ZigBlogDb.Posts.Find(filter).SingleOrDefaultAsync();

            if (post == null)
                throw new Exception(Translation.ThisPostCouldNotBeFoundException);

            bool userLikes;
            UpdateDefinition<Post> update = null;

            if (post.LikersIds.Contains(IdentityHelper.CurrentUser.Id))
            {
                post.LikersIds.Remove(IdentityHelper.CurrentUser.Id);

                userLikes = false;
                update = Builders<Post>.Update.Pull(x => x.LikersIds, IdentityHelper.CurrentUser.Id);
            }
            else
            {
                post.LikersIds.Add(IdentityHelper.CurrentUser.Id);

                userLikes = true;
                update = Builders<Post>.Update.Push(x => x.LikersIds, IdentityHelper.CurrentUser.Id);
            }

            await ZigBlogDb.Posts.UpdateOneAsync(filter, update);

            return Json(new
            {
                PostId = post.Id,
                UserLikes = userLikes,
                LikesCount = post.LikersIds.Count
            });
        }

        // POST: /home/likecomment?id={id}
        [Authorize]
        [HandleJsonError]
        [HttpPost]
        public async Task<JsonResult> LikeComment(int id)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.Id, id);
            var comment = await ZigBlogDb.Comments.Find(filter).SingleOrDefaultAsync();

            if (comment == null)
                throw new Exception(Translation.ThisCommentCouldNotBeFoundException);

            bool userLikes;
            UpdateDefinition<Comment> update = null;

            if (comment.LikersIds.Contains(IdentityHelper.CurrentUser.Id))
            {
                comment.LikersIds.Remove(IdentityHelper.CurrentUser.Id);

                userLikes = false;
                update = Builders<Comment>.Update.Pull(x => x.LikersIds, IdentityHelper.CurrentUser.Id);
            }
            else
            {
                comment.LikersIds.Add(IdentityHelper.CurrentUser.Id);

                userLikes = true;
                update = Builders<Comment>.Update.Push(x => x.LikersIds, IdentityHelper.CurrentUser.Id);
            }

            await ZigBlogDb.Comments.UpdateOneAsync(filter, update);

            return Json(new
            {
                CommentId = comment.Id,
                UserLikes = userLikes,
                LikesCount = comment.LikersIds.Count
            });
        }

        // GET: /about
        [HandleError]
        public async Task<ActionResult> About()
        {
            await ZigBlogDb.Posts.DeleteManyAsync(_ => true);
            await ZigBlogDb.Comments.DeleteManyAsync(_ => true);

            var content = @"***Lorem ipsum dolor sit amet, consectetur adipiscing elit.***

----------

Praesent quis arcu non massa vehicula ultricies.

* Praesent ante quam, malesuada eget ante vitae, vulputate rutrum metus.
* Curabitur interdum accumsan pellentesque.
* Donec tellus metus, vehicula a egestas ac, pretium a leo.
* In et blandit enim.

Pellentesque sed condimentum lectus. Curabitur justo tortor, vestibulum at elementum at, posuere quis ex. Sed eget aliquam nibh. Proin lacinia pretium ipsum, ac lacinia lectus. Nullam sed ex id mauris tincidunt tincidunt.Integer pulvinar, ligula sed tristique ornare, eros mauris dapibus enim, sit amet fermentum neque elit et felis. Duis finibus sem quis sem lacinia, eu tristique augue volutpat.Nulla sed nulla finibus, malesuada lorem in, convallis est.

![Yo yo doggy!](https://s-media-cache-ak0.pinimg.com/736x/c0/72/4a/c0724afebaa3ac1376cbb9ba24402ac6.jpg)

Ut sollicitudin, dolor ut efficitur venenatis, lacus magna cursus libero, vel consequat sem dui tincidunt leo.Maecenas in ipsum et neque tincidunt maximus quis vitae purus. Nulla facilisi. Duis blandit facilisis eros vitae consectetur. In dolor metus, vestibulum non tortor vitae, feugiat fringilla nisi. Duis in risus felis. Cras malesuada congue nisi, eget molestie ante euismod in.

> Maecenas enim erat, sagittis eu est vel, dictum viverra ipsum. Praesent faucibus felis sed augue consequat pretium.Praesent ornare tempus sem, ut euismod mauris semper vel. Donec lobortis nibh id nulla varius posuere.Praesent laoreet dolor eu orci bibendum tempus.Nulla vel libero consequat, porttitor augue volutpat, convallis metus.Integer interdum malesuada urna, eu rhoncus mauris aliquam id. Nam nec vulputate risus. Nullam hendrerit ligula id risus facilisis finibus.Proin urna turpis, mollis eleifend condimentum non, iaculis dictum odio. Morbi erat magna, pellentesque ut malesuada nec, venenatis vel ex. Ut facilisis nisl in rutrum volutpat. Duis purus lacus, vulputate euismod nunc sed, pellentesque sollicitudin quam. Pellentesque ut enim suscipit, aliquam diam suscipit, feugiat nunc. Vivamus vehicula lectus ipsum, ac vehicula est eleifend a. Curabitur a elit eget neque pellentesque vehicula non nec ligula. Etiam maximus neque sit amet molestie bibendum.Nullam auctor maximus nisi ut posuere. Donec venenatis sit amet mi sit amet tincidunt. Nam dignissim turpis rutrum justo pharetra, non consequat nisi tincidunt.In cursus semper diam eu venenatis. Maecenas sed vulputate quam, et porta nibh. Donec laoreet tellus risus, in tincidunt urna pharetra at.

    Suspendisse vitae augue in nulla pretium imperdiet.Nullam ullamcorper ultrices elit.
    Suspendisse leo quam, placerat facilisis lobortis vel, cursus ut lacus.
    Nullam ultrices nec libero eget rhoncus.
    Donec hendrerit ut lorem in euismod.

Praesent accumsan molestie convallis. Nullam nec sodales sapien. In imperdiet eros et erat sagittis porttitor.Pellentesque nec semper ipsum. Vivamus laoreet lorem et eros porta semper.Donec lacinia sem in nibh scelerisque, id rhoncus leo malesuada.Phasellus molestie erat eu orci semper fringilla.Curabitur a tempus enim, eget volutpat nulla. Curabitur consectetur rhoncus auctor. Sed ac ultrices ante, non dapibus augue.";

            var data = DateTime.Now;

            for (var i = 0; i < 100; i++)
            {
                await ZigBlogDb.Posts.InsertOneAsync(new Post
                {
                    BloggerId = IdentityHelper.CurrentUser.Id,
                    TitleUrl = $"lorem_ipsum_dolor_sit_amet_{i + 1}",
                    Title = $"Lorem ipsum dolor sit amet {i + 1}",
                    Content = content,
                    ParsedContent = Markdown.Transform(content),
                    Created = data
                });

                data = data.AddDays(1);
            }

            var viewModel = new HomeAboutViewModel
            {
                AboutContent = ZigBlogDb.Parameters.AboutContent
            };

            return View(viewModel);
        }
        
        /// <summary>
        /// Generates a unique title URL.
        /// </summary>
        /// <param name="title">Post's title</param>
        /// <returns>Unique title URL</returns>
        [HandleError]
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

        private Markdown Markdown
        {
            get
            {
                return new Markdown
                {
                    PrepareImage = (tag, _) =>
                    {
                        tag.attributes.Add("class", "zg-post-image-width");
                        return true;
                    }
                };
            }
        }
    }
}