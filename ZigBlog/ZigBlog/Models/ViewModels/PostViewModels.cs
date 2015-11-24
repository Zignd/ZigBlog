using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZigBlog.Models.ViewModels
{
    public class PostNewViewModel
    {
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Content { get; set; }
    }

    public class PostShowViewModel
    {
        public Post Post { get; set; }
    }
}