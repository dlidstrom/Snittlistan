#nullable enable

namespace Snittlistan.Web.Infrastructure.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Web.Mvc;

    public class RequiredIfExistsAttribute : ValidationAttribute, IClientValidatable
    {
        private readonly RequiredAttribute innerAttribute = new();

        public RequiredIfExistsAttribute(string dependentProperty)
        {
            DependentProperty = dependentProperty;
        }

        public string DependentProperty { get; set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRule rule = new()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredifexists",
            };

            string depProp = BuildDependentPropertyId(metadata, (ViewContext)context);

            rule.ValidationParameters.Add("dependentproperty", depProp);

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            Type containerType = validationContext.ObjectInstance.GetType();
            PropertyInfo field = containerType.GetProperty(DependentProperty);

            if (field != null)
            {
                // get the value of the dependent property
                object dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if (dependentvalue != null)
                {
                    // match => means we should try validating this field
                    if (!innerAttribute.IsValid(value))
                    {
                        // validation failed - return an error
                        return new ValidationResult(ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(DependentProperty);

            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            string thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
            {
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            }

            return depProp;
        }
    }
}
