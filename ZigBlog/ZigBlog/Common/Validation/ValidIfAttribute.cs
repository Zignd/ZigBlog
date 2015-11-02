using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ZigBlog.Common.Validation
{
    public class ValidIfAttribute : ValidationAttribute, IClientValidatable
    {
        public Comparison Comparison { get; set; }
        public object Value { get; set; }

        public ValidIfAttribute(Comparison comparison, object value)
        {
            Comparison = comparison;
            Value = value;
        }

        public override bool IsValid(object value)
        {
            return Comparison == Comparison.IsEqualTo ? Value.Equals(value) : !Value.Equals(value);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule();
            rule.ErrorMessage = FormatErrorMessage(metadata.GetDisplayName());
            rule.ValidationParameters.Add("comparison", Comparison.ToString());
            rule.ValidationParameters.Add("value", Value);
            rule.ValidationType = "validif";
            yield return rule;
        }
    }

    public enum Comparison
    {
        IsEqualTo,
        IsNotEqualTo
    }
}