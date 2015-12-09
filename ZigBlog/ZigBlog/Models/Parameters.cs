using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Models.Common;

namespace ZigBlog.Models
{
    public class Parameters : CustomModelBase
    {
        public string BlogTitle { get; set; }
        public string SmallDescription { get; set; }
        public string AboutContent { get; set; }
    }
}