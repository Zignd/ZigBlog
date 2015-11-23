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
                CurrentPage = arg,
                PostsPerPage = postsPerPage,
                TotalPostsCount = await ZigBlogDb.Posts.CountAsync(_ => true),
                Posts = await ZigBlogDb.Posts.Find(_ => true).Sort(sort).Skip((arg - 1) * postsPerPage).Limit(postsPerPage).ToListAsync()
            };

            return View(viewModel);
        }

        // GET: /About
        public async Task<ActionResult> About()
        {
            // TODO: This is just for testing, don't forget to remove it xD
            await ZigBlogDb.Posts.DeleteManyAsync(_ => true);

            var content = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent quis arcu non massa vehicula ultricies. Praesent ante quam, malesuada eget ante vitae, vulputate rutrum metus. Curabitur interdum accumsan pellentesque. Donec tellus metus, vehicula a egestas ac, pretium a leo. In et blandit enim. Pellentesque sed condimentum lectus. Curabitur justo tortor, vestibulum at elementum at, posuere quis ex. Sed eget aliquam nibh.

Proin lacinia pretium ipsum, ac lacinia lectus. Nullam sed ex id mauris tincidunt tincidunt.Integer pulvinar, ligula sed tristique ornare, eros mauris dapibus enim, sit amet fermentum neque elit et felis. Duis finibus sem quis sem lacinia, eu tristique augue volutpat.Nulla sed nulla finibus, malesuada lorem in, convallis est. Ut sollicitudin, dolor ut efficitur venenatis, lacus magna cursus libero, vel consequat sem dui tincidunt leo.Maecenas in ipsum et neque tincidunt maximus quis vitae purus. Nulla facilisi. Duis blandit facilisis eros vitae consectetur. In dolor metus, vestibulum non tortor vitae, feugiat fringilla nisi. Duis in risus felis. Cras malesuada congue nisi, eget molestie ante euismod in. Maecenas enim erat, sagittis eu est vel, dictum viverra ipsum. Praesent faucibus felis sed augue consequat pretium.Praesent ornare tempus sem, ut euismod mauris semper vel.

Donec lobortis nibh id nulla varius posuere.Praesent laoreet dolor eu orci bibendum tempus.Nulla vel libero consequat, porttitor augue volutpat, convallis metus.Integer interdum malesuada urna, eu rhoncus mauris aliquam id. Nam nec vulputate risus. Nullam hendrerit ligula id risus facilisis finibus.Proin urna turpis, mollis eleifend condimentum non, iaculis dictum odio. Morbi erat magna, pellentesque ut malesuada nec, venenatis vel ex. Ut facilisis nisl in rutrum volutpat. Duis purus lacus, vulputate euismod nunc sed, pellentesque sollicitudin quam. Pellentesque ut enim suscipit, aliquam diam suscipit, feugiat nunc.

Vivamus vehicula lectus ipsum, ac vehicula est eleifend a. Curabitur a elit eget neque pellentesque vehicula non nec ligula. Etiam maximus neque sit amet molestie bibendum.Nullam auctor maximus nisi ut posuere. Donec venenatis sit amet mi sit amet tincidunt. Nam dignissim turpis rutrum justo pharetra, non consequat nisi tincidunt.In cursus semper diam eu venenatis. Maecenas sed vulputate quam, et porta nibh. Donec laoreet tellus risus, in tincidunt urna pharetra at. Suspendisse vitae augue in nulla pretium imperdiet.Nullam ullamcorper ultrices elit. Suspendisse leo quam, placerat facilisis lobortis vel, cursus ut lacus. Nullam ultrices nec libero eget rhoncus.

Donec hendrerit ut lorem in euismod.Praesent accumsan molestie convallis. Nullam nec sodales sapien. In imperdiet eros et erat sagittis porttitor.Pellentesque nec semper ipsum. Vivamus laoreet lorem et eros porta semper.Donec lacinia sem in nibh scelerisque, id rhoncus leo malesuada.Phasellus molestie erat eu orci semper fringilla.Curabitur a tempus enim, eget volutpat nulla. Curabitur consectetur rhoncus auctor. Sed ac ultrices ante, non dapibus augue.";

            var data = DateTime.Now;

            for (var i = 0; i < 100; i++)
            {
                await ZigBlogDb.Posts.InsertOneAsync(new Post
                {
                    BloggerId = IdentityHelper.CurrentUser.Id,
                    Content = content,
                    Created = data,
                    Title = $"Lorem ipsum dolor sit amet {i + 1}",
                    TitleUrl = $"lorem_ipsum_dolor_sit_amet_{i + 1}"
                });

                data = data.AddDays(1);
            }

            var viewModel = new HomeAboutViewModel
            {
                AboutContent = ZigBlogDb.Parameters.AboutContent
            };

            return View(viewModel);
        }
    }
}