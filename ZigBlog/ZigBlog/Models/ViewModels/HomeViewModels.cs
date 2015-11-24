using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZigBlog.Models.ViewModels
{
    public class HomePageViewModel
    {
        public int CurrentPage { get; set; }
        public int PostsPerPage { get; set; }
        public long TotalPostsCount { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class HomeNewViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }

    public class HomeShowViewModel
    {
        public Post Post { get; set; }
    }

    public class HomeAboutViewModel
    {
        public string AboutContent { get; set; }
    }
}