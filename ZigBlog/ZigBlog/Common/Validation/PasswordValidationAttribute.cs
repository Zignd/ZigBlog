using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZigBlog.Common.Validation
{
    public class PasswordValidationAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Require a digit ('0' - '9').
        /// </summary>
        public bool RequireDigit { get; set; }
        
        /// <summary>
        /// Minimum required length. A 0 value disables this rule.
        /// </summary>
        public int RequiredLength { get; set; }

        /// <summary>
        /// Require a lower case letter ('a' - 'z').
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// Require a non letter or digit character
        /// </summary>
        public bool RequireNonLetterOrDigit { get; set; }

        /// <summary>
        /// Require an upper case letter ('A' - 'Z')
        /// </summary>
        public bool RequireUppercase { get; set; }

        public PasswordValidationAttribute(bool requireDigit, int requiredLength, bool requireLowercase, bool requireNonLetterOrDigit, bool requireUppercase)
        {
            RequireDigit = requireDigit;
            RequiredLength = requiredLength;
            RequireLowercase = requireLowercase;
            RequireNonLetterOrDigit = requireNonLetterOrDigit;
            RequireUppercase = requireUppercase;
        }

        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            if (!(value is string))
                throw new ArgumentException("Password validation will only work with string values");

            var castedValue = (string)value;

            if (RequireDigit && !castedValue.Any(x => char.IsDigit(x)))
                return false;

            if (RequiredLength != 0 && castedValue.Length < RequiredLength)
                return false;

            if (RequireLowercase && !castedValue.Any(x => char.IsLower(x)))
                return false;

            if (RequireNonLetterOrDigit && !castedValue.Any(x => !char.IsLetter(x) || char.IsDigit(x)))
                return false;

            if (RequireUppercase && !castedValue.Any(x => char.IsLetter(x) && char.IsUpper(x)))
                return false;

            return true;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("requiredigit", RequireDigit);
            rule.ValidationParameters.Add("requiredlength", RequiredLength);
            rule.ValidationParameters.Add("requirelowercase", RequireLowercase);
            rule.ValidationParameters.Add("requirenonletterordigit", RequireNonLetterOrDigit);
            rule.ValidationParameters.Add("requireuppercase", RequireUppercase);
            rule.ValidationType = "passwordvalidation";
            yield return rule;
        }
    }
}