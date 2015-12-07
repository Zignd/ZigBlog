
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
using ZigBlog.Common.Filters;
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
        [HandleError]
        public ActionResult SignIn()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                throw new Exception(Translation.AccessDenied);
            
            return View(new UserSignInViewModel
            {
                ReturnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.PathAndQuery : "/"
            });
        }

        [AllowAnonymous]
        [HandleError]
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
        [HandleError]
        public ActionResult SignUp()
        {
            return View(new UserSignUpViewModel
            {
                ReturnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.PathAndQuery : "/"
            });
        }

        [AllowAnonymous]
        [HandleError]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignUp(UserSignUpViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // TODO: Improve image uploading following those tips: http://stackoverflow.com/a/4535684/1324082

                var path = Path.Combine(Server.MapPath("~/App_Data/Avatars"), $"{viewModel.Username}.jpg");
                viewModel.Avatar.SaveAs(path);

                var user = new AppUser { UserName = viewModel.Username, Email = viewModel.EmailAddress.ToLower(), Created = DateTime.Now };
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                if (result.Succeeded)
                {
                    if (User.IsInRole("Administrator"))
                        await UserManager.AddToRoleAsync(user.Id, viewModel.Role.ToString());
                    else
                        await UserManager.AddToRoleAsync(user.Id, "Commenter");

                    return Redirect(viewModel.ReturnUrl);
                }   
                else
                {
                    ModelState.AddModelError(string.Empty, Translation.SomethingHappenedUserRegistration);
                }
            }
            
            return View(viewModel);
        }

        public ActionResult SignOut()
        {
            AuthManager.SignOut();

            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        [AllowAnonymous]
        [HandleError]
        public async Task<ActionResult> Profile(string userName)
        {
            var user = UserManager.FindByName(userName);
            
            if (user == null)
                throw new Exception(Translation.UsernameDoNotExist);

            return View(new UserProfileViewModel
            {
                User = user
            });
        }

        [AllowAnonymous]
        [HandleError]
        public ActionResult Avatar(string userName)
        {
            var path = Server.MapPath("~/App_Data/Avatars");
            var fullPath = Path.Combine(path, $"{userName}.jpg");

            if (!System.IO.File.Exists(fullPath))
                throw new Exception(Translation.ThisAvatarCouldNotBeFoundException);

            return File(fullPath, "image/jpeg");
        }

        [Authorize(Roles = "Administrator")]
        [HandleError]
        public async Task<ActionResult> Manage()
        {
            var users = await ZigBlogDb.Users.Find(_ => true).ToListAsync();
            return View(new UserManageViewModel { Users = users });
        }

        [Authorize(Roles = "Administrator")]
        [HandleJsonError]
        [HttpPost]
        public async Task UpdateRole(string userName, string role, bool isInRole)
        {
            var user = await UserManager.FindByNameAsync(userName);

            if (user == null)
                throw new Exception(Translation.ThisUserCouldNotBeFoundException);

            if (!await RoleManager.RoleExistsAsync(role))
                throw new Exception(Translation.ThisRoleDoesNotExist);

            if (isInRole)
                await UserManager.AddToRoleAsync(user.Id, role);
            else
                await UserManager.RemoveFromRoleAsync(user.Id, role);
        }
    }
}