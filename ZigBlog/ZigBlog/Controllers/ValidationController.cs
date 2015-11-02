using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Validation;

namespace ZigBlog.Controllers
{
    public class ValidationController : Controller
    {
        public async Task<JsonResult> UniqueUsername(string username)
        {
            try
            {
                return Json(await MultiSideValidation.UniqueUsername(username), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Error = string.Format("An exception occurred: {0}", ex.Message) }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}