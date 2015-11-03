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
            if (value == null)
                return false;

            return MultiSideValidation.UniqueUsername((string)value);
        }
    }
}