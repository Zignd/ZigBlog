using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Controllers;

namespace ZigBlog.Common.Validation
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var task = MultiSideValidation.UniqueUsername((string)value);
            task.Wait();

            return task.Result;
        }
    }
}