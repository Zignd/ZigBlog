using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ZigBlog
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/zgcommentpartial").Include(
                "~/Scripts/zg.commentpartial.js"));

            bundles.Add(new ScriptBundle("~/bundles/zgeditor").Include(
                "~/Scripts/MarkdownDeep.min.js",
                "~/Scripts/MarkdownDeepEditor.min.js",
                "~/Scripts/zg.editor.js"));

            bundles.Add(new ScriptBundle("~/bundles/zgmanage").Include(
                "~/Scripts/zg.manage.js"));

            bundles.Add(new ScriptBundle("~/bundles/zgsignup").Include(
                "~/Scripts/zg.signup.js"));

            bundles.Add(new ScriptBundle("~/bundles/zgpostpartial").Include(
                "~/Scripts/zg.postpartial.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/Site.css"));
        }
    }
}