using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ZigBlog.Common.Validation
{
    // http://stackoverflow.com/questions/14005773/use-asp-net-mvc-validation-with-jquery-ajax
    public class ValidateAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.HttpContext.Request.IsAjaxRequest())
                return;

            var modelState = filterContext.Controller.ViewData.ModelState;
            if (!modelState.IsValid)
            {
                var clientModelState = from key in modelState.Keys
                                       select new
                                       {
                                           Key = key,
                                           Value = modelState[key].Value.RawValue,
                                           Errors = (from error in modelState[key].Errors
                                                     select error.ErrorMessage).ToArray()
                                       };

                filterContext.Result = new JsonResult
                {
                    Data = clientModelState
                };

                filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}