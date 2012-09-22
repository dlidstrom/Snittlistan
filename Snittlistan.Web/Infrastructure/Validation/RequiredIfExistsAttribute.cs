namespace Snittlistan.Web.Infrastructure.Validation
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class RequiredIfExistsAttribute : ValidationAttribute, IClientValidatable
    {
        private RequiredAttribute innerAttribute = new RequiredAttribute();

        public RequiredIfExistsAttribute(string dependentProperty)
        {
            this.DependentProperty = dependentProperty;
        }

        public string DependentProperty { get; set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = this.FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = "requiredifexists",
            };

            string depProp = this.BuildDependentPropertyId(metadata, context as ViewContext);

            rule.ValidationParameters.Add("dependentproperty", depProp);

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this.DependentProperty);

            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if (dependentvalue != null)
                {
                    // match => means we should try validating this field
                    if (!this.innerAttribute.IsValid(value))
                    {
                        // validation failed - return an error
                        return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }

            return ValidationResult.Success;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            // build the ID of the property
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this.DependentProperty);

            // unfortunately this will have the name of the current field appended to the beginning,
            // because the TemplateInfo's context has had this fieldname appended to it. Instead, we
            // want to get the context as though it was one level higher (i.e. outside the current property,
            // which is the containing object (our Person), and hence the same level as the dependent property.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
            {
                // strip it off again
                depProp = depProp.Substring(thisField.Length);
            }

            return depProp;
        }
    }
}