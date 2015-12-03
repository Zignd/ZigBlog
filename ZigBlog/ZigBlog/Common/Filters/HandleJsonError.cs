using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ZigBlog.Common.Filters
{
    public class HandleJsonErrorAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            
            filterContext.Result = new JsonResult
            {
                Data = new { ErrorMessage = filterContext.Exception.Message },
                JsonRequestBehavior = filterContext.HttpContext.Request.HttpMethod.Equals("GET") ? JsonRequestBehavior.AllowGet : JsonRequestBehavior.DenyGet
            };

            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}
