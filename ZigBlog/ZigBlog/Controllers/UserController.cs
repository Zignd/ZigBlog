
using MongoDB.Driver;
using System;
using System.Collections.Generic;
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
    public class UserController : CustomControllerBase
    {
        [HttpGet]
        public ActionResult SignIn(string returnUrl = "/")
        {
            return View(new UserSignInViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(UserSignInViewModel viewModel, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                var filter = Builders<User>.Filter.Eq(u => u.UsernameUpper, viewModel.Username.ToUpper()) & Builders<User>.Filter.Eq(u => u.Password, viewModel.Password);
                var user = await ZigBlogDb.Users.Find(filter).SingleOrDefaultAsync();

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, Translation.UserSignInError);
                    return View();
                }

                FormsAuthentication.SetAuthCookie(user.Username, viewModel.RememberMe);

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                return View("Error", new SharedErrorViewModel(ex));
            }
        }

        [HttpGet]
        public ActionResult SignUp(string returnUrl = "/")
        {
            return View(new UserSignUpViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(UserSignUpViewModel viewModel, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            // TODO: Send an email to the user with an account activation url on it. The user would need to access this url in order to activate its account and make use of it.

            var user = new User
            {
                Username = viewModel.Username,
                Password = viewModel.Password,
                Role = UserRole.Commenter,
                EmailAddress = viewModel.EmailAddress,
                IsActivated = true,
                Created = DateTime.Now
            };

            try
            {
                await ZigBlogDb.Users.InsertOneAsync(user);

                return Redirect(returnUrl);
            }
            catch (Exception ex)
            {
                return View("Error", new SharedErrorViewModel(ex));
            }
        }

        public ActionResult SignOut(string returnUrl = "/")
        {
            FormsAuthentication.SignOut();

            return Redirect(returnUrl);
        }
    }
}