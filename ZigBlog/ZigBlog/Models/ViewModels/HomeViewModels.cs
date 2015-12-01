using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZigBlog.Common;
using ZigBlog.Translations;

namespace ZigBlog.Models.ViewModels
{
    public class HomePageViewModel
    {
        public int CurrentPage { get; set; }
        public int PostsPerPage { get; set; }
        public long TotalPostsCount { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class HomePostPartialViewModel
    {
        public Post Post { get; set; }
        public bool IsHomePageMode { get; set; }
    }

    public class HomePostCommentPartialViewModel
    {
        public int PostId { get; set; }

        public bool IsTopLevel { get; set; }

        public int? ParentId { get; set; }

        [Required(ErrorMessageResourceName = "PostCommentContentValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        public string Content { get; set; }
    }

    public class HomeNewEditViewModel
    {
        public string TitleUrl { get; set; }

        [Required(ErrorMessageResourceName = "HomeNewEditTitleValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        public string Title { get; set; }

        [Required(ErrorMessageResourceName = "HomeNewEditContentValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        public string Content { get; set; }

        public bool IsNewMode { get; set; }
    }

    public class HomeAboutViewModel
    {
        public string AboutContent { get; set; }
    }
}