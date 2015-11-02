using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Models.Common;

namespace ZigBlog.Models
{
    public class Parameters : ModelBase
    {
        public string BlogTitle { get; set; }
        public string AboutContent { get; set; }
    }
}