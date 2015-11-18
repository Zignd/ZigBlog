using AspNet.Identity.MongoDB;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZigBlog.Common.Database;
using ZigBlog.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ZigBlog.Common.Identity
{
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            return new AppUserManager(new UserStore<AppUser>(ZigBlogDb.Users))
            {
                UserValidator = new DummyUserValidator(),
                PasswordValidator = new DummyPasswordValidator()
            };
        }
    }

    // Username validation will be performed with a ValidationAttribute so that I can easily manage client and server side validation in one place.
    public class DummyUserValidator : IIdentityValidator<AppUser>
    {
        public async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            return IdentityResult.Success;
        }
    }

    // Password validation will be performed with a ValidationAttribute so that I can easily manage client and server side validation in one place.
    public class DummyPasswordValidator : IIdentityValidator<string>
    {
        public async Task<IdentityResult> ValidateAsync(string item)
        {
            return IdentityResult.Success;
        }
    }
}