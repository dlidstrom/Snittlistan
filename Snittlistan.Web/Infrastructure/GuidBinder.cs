using System;
using System.Web.Mvc;

namespace Snittlistan.Web.Infrastructure
{
    public class GuidBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (value != null && Guid.TryParse(value.AttemptedValue, out var guid))
                return guid;

            return Guid.Empty;
        }
    }
}