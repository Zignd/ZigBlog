using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZigBlog.Models.ViewModels
{
    public class SharedErrorViewModel
    {
        public SharedErrorViewModel(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; set; }
    }
}