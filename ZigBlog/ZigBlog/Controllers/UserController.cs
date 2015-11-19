
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZigBlog.Common.Database;
using ZigBlog.Controllers.Common;
using ZigBlog.Models;
using ZigBlog.Models.ViewModels;
using ZigBlog.Translations;

namespace ZigBlog.Controllers
{
    [Authorize]
    public class UserController : CustomControllerBase
    {
        [AllowAnonymous]
        public ActionResult SignIn(string returnUrl = "/")
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                throw new Exception(Translation.AccessDenied);

            return View(new UserSignInViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(UserSignInViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(viewModel.Username, viewModel.Password);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, Translation.UserSignInError);
                }
                else
                {
                    var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthManager.SignIn(new AuthenticationProperties { IsPersistent = viewModel.RememberMe }, identity);

                    return Redirect(viewModel.ReturnUrl);
                }
            }

            return View(viewModel);
        }

        [AllowAnonymous]
        public ActionResult SignUp(string returnUrl = "/")
        {
            return View(new UserSignUpViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(UserSignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = viewModel.Username, Email = viewModel.EmailAddress.ToLower(), Created = DateTime.Now };
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                    return Redirect(viewModel.ReturnUrl);
                else
                    ModelState.AddModelError(string.Empty, Translation.SomethingHappenedUserRegistration);
            }
            
            return View(viewModel);
        }

        [Authorize]
        public ActionResult SignOut(string returnUrl = "/")
        {
            AuthManager.SignOut();

            return Redirect(returnUrl);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Profile(string arg)
        {
            var user = UserManager.FindByName(arg);
            
            if (user == null)
                throw new ArgumentException(Translation.UsernameDoNotExist);

            return View(new UserProfileViewModel
            {
                User = user
            });
        }
    }
}