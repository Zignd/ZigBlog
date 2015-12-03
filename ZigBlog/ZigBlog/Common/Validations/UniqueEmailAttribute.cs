using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Controllers;

namespace ZigBlog.Common.Validations
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            return MultiSideValidation.UniqueEmailAddress((string)value);
        }
    }
}