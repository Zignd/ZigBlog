using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Identity;
using ZigBlog.Common.Validations;
using ZigBlog.Translations;

namespace ZigBlog.Models.ViewModels
{
    public class UserSignInViewModel
    {
        [Required(ErrorMessageResourceName = "UsernameValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Username", ResourceType = typeof(Translation))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "PasswordValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Password", ResourceType = typeof(Translation))]
        public string Password { get; set; }
        
        [Display(Name = "RememberMe", ResourceType = typeof(Translation))]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class UserSignUpViewModel
    {
        [AvatarValidation(ErrorMessageResourceName = "AvatarImageUploadHelp", ErrorMessageResourceType = typeof(Translation))]
        public HttpPostedFileBase Avatar { get; set; }

        [Required(ErrorMessageResourceName = "UsernameValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [UniqueUsername(ErrorMessageResourceName = "UsernameValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Remote("UniqueUsername", "Validation", ErrorMessageResourceName = "UsernameValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Username", ResourceType = typeof(Translation))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "PasswordValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Password(true, 6, true, true, ErrorMessageResourceName = "PasswordValidationErrorInvalid", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Password", ResourceType = typeof(Translation))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "PasswordConfirmationValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessageResourceName = "PasswordConfirmationValidationErrorDiffers", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "PasswordConfirmation", ResourceType = typeof(Translation))]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessageResourceName = "EmailAddressValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [EmailAddress(ErrorMessageResourceName = "EmailAddressValidationErrorNotValid", ErrorMessageResourceType = typeof(Translation))]
        [UniqueEmail(ErrorMessageResourceName = "EmailAddressValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Remote("UniqueEmailAddress", "Validation", ErrorMessageResourceName = "EmailAddressValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Translation))]
        public string EmailAddress { get; set; }

        [ValidIf(Comparison.IsEqualTo, true, ErrorMessageResourceName = "AcceptTermsValidationErrorMustBeAccepted", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "AcceptTerms", ResourceType = typeof(Translation))]
        public bool AcceptTerms { get; set; }

        public UserRole Role { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class UserEditViewModel
    {
        [AvatarValidation(ErrorMessageResourceName = "AvatarImageUploadHelp", ErrorMessageResourceType = typeof(Translation))]
        public HttpPostedFileBase Avatar { get; set; }

        [Required(ErrorMessageResourceName = "CurrentPasswordValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "CurrentPassword", ResourceType = typeof(Translation))]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Password(true, 6, true, true, ErrorMessageResourceName = "NewPasswordValidationErrorInvalid", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "NewPassword", ResourceType = typeof(Translation))]
        public string NewPassword { get; set; }

        [Required(ErrorMessageResourceName = "NewPasswordConfirmationValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessageResourceName = "PasswordConfirmationValidationErrorDiffers", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "NewPasswordConfirmation", ResourceType = typeof(Translation))]
        public string NewPasswordConfirmation { get; set; }

        [Required(ErrorMessageResourceName = "EmailAddressValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [EmailAddress(ErrorMessageResourceName = "EmailAddressValidationErrorNotValid", ErrorMessageResourceType = typeof(Translation))]
        [UniqueEmail(ErrorMessageResourceName = "EmailAddressValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Remote("UniqueEmailAddress", "Validation", ErrorMessageResourceName = "EmailAddressValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Translation))]
        public string EmailAddress { get; set; }

        public string ReturnUrl { get; set; }
    }

    public class UserProfileViewModel
    {
        public AppUser User { get; set; }
    }

    public class UserManageViewModel
    {
        public List<AppUser> Users { get; set; }
    }

    public class UserTableRowViewModel
    {
        public UserTableRowViewModel(AppUser user)
        {
            Id = user.Id;
            UserName = user.UserName;

            var task = IdentityHelper.UserManager.IsInRoleAsync(user.Id, "Administrator");
            task.Wait();
            IsAdministrator = task.Result;

            task = IdentityHelper.UserManager.IsInRoleAsync(user.Id, "Blogger");
            task.Wait();
            IsBlogger = task.Result;

            task = IdentityHelper.UserManager.IsInRoleAsync(user.Id, "Commenter");
            task.Wait();
            IsCommenter = task.Result;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsAdministrator { get; set; }
        public bool IsBlogger { get; set; }
        public bool IsCommenter { get; set; }
    }

    public enum UserRole
    {
        Administrator,
        Blogger,
        Commenter
    }
}