﻿using System.ComponentModel.DataAnnotations;

namespace Medyx_EMR_BCA.ApiAssets.AttributeCustom
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private string PropertyName { get; set; }
        private object DesiredValue { get; set; }
        private readonly RequiredAttribute _innerAttribute;

        public RequiredIfAttribute(string propertyName, object desiredvalue)
        {
            PropertyName = propertyName;
            DesiredValue = desiredvalue;
            _innerAttribute = new RequiredAttribute();
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var dependentValue = context.ObjectInstance.GetType().GetProperty(PropertyName)?.GetValue(context.ObjectInstance, null);
            if (dependentValue?.ToString() == DesiredValue?.ToString())
            {
                if (!_innerAttribute.IsValid(value))
                {
                    return new ValidationResult(FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                }
            }
            return ValidationResult.Success;
        }
    }
}
