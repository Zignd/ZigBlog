using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZigBlog.Common.Validation;
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
        [Required(ErrorMessageResourceName = "UsernameValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [UniqueUsername(ErrorMessageResourceName = "UsernameValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Remote("UniqueUsername", "Validation", ErrorMessageResourceName = "UsernameValidationErrorAlreadyInUse", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Username", ResourceType = typeof(Translation))]
        public string Username { get; set; }

        [Required(ErrorMessageResourceName = "PasswordValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "Password", ResourceType = typeof(Translation))]
        public string Password { get; set; }

        [Required(ErrorMessageResourceName = "PasswordConfirmationValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [System.ComponentModel.DataAnnotations.Compare("Model.Password", ErrorMessageResourceName = "PasswordConfirmationValidationErrorDiffers", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "PasswordConfirmation", ResourceType = typeof(Translation))]
        public string PasswordConfirmation { get; set; }

        [Required(ErrorMessageResourceName = "EmailAddressValidationErrorRequired", ErrorMessageResourceType = typeof(Translation))]
        [RegularExpression(@"/^([\w-]+(?:\.[\w-]+)*)@((?:[\w-]+\.)*\w[\w-]{0,66})\.([a-z]{2,6}(?:\.[a-z]{2})?)$/i", ErrorMessageResourceName = "EmailAddressValidationErrorNotValid", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "EmailAddress", ResourceType = typeof(Translation))]
        public string EmailAddress { get; set; }

        [ValidIf(Comparison.IsEqualTo, true, ErrorMessageResourceName = "AcceptTermsValidationErrorMustBeAccepted", ErrorMessageResourceType = typeof(Translation))]
        [Display(Name = "AcceptTerms", ResourceType = typeof(Translation))]
        public bool AcceptTerms { get; set; }

        public string ReturnUrl { get; set; }
    }
}