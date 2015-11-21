using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Models.ViewModels
{
    public class HomePageViewModel
    {
        public int Page { get; set; }
        public int PostsPerPage { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class HomeAboutViewModel
    {
        public string AboutContent { get; set; }
    }
}