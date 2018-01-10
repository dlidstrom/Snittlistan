using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Snittlistan.Test
{
    public static class ActionResultHelper
    {
        public static T AssertResultIs<T>(this ActionResult result) where T : ActionResult
        {
            if (!(result is T obj))
                throw new Exception($"Expected result to be of type {(object)typeof(T).Name}. It is actually of type {(object)result.GetType().Name}.");
            return obj;
        }

        public static ViewResult AssertViewRendered(this ActionResult result)
        {
            return result.AssertResultIs<ViewResult>();
        }

        public static PartialViewResult AssertPartialViewRendered(this ActionResult result)
        {
            return result.AssertResultIs<PartialViewResult>();
        }

        public static RedirectResult AssertHttpRedirect(this ActionResult result)
        {
            return result.AssertResultIs<RedirectResult>();
        }

        public static RedirectToRouteResult AssertActionRedirect(this ActionResult result)
        {
            return result.AssertResultIs<RedirectToRouteResult>();
        }

        public static RedirectToRouteResult ToController(this RedirectToRouteResult result, string controller)
        {
            return result.WithParameter("controller", controller);
        }

        public static RedirectToRouteResult ToAction(this RedirectToRouteResult result, string action)
        {
            return WithParameter(result, "action", action);
        }

        public static RedirectToRouteResult ToAction<TController>(this RedirectToRouteResult result, Expression<Action<TController>> action) where TController : IController
        {
            string name = ((MethodCallExpression)action.Body).Method.Name;
            string controller = typeof(TController).Name;
            if (controller.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                controller = controller.Substring(0, controller.Length - "Controller".Length);
            return ToAction(ToController(result, controller), name);
        }

        public static RedirectToRouteResult WithParameter(this RedirectToRouteResult result, string paramName, object value)
        {
            if (!result.RouteValues.ContainsKey(paramName))
                throw new Exception($"Could not find a parameter named '{paramName}' in the result's Values collection.");
            object obj = result.RouteValues[paramName];
            if (!obj.Equals(value))
                throw new Exception($"When looking for a parameter named '{paramName}', expected '{value}' but was '{obj}'.");
            return result;
        }

        public static ViewResult ForView(this ViewResult result, string viewName)
        {
            if (result.ViewName != viewName)
                throw new Exception($"Expected view name '{viewName}', actual was '{result.ViewName}'");
            return result;
        }

        public static PartialViewResult ForView(this PartialViewResult result, string partialViewName)
        {
            if (result.ViewName != partialViewName)
                throw new Exception($"Expected partial view name '{partialViewName}', actual was '{result.ViewName}'");
            return result;
        }

        public static RedirectResult ToUrl(this RedirectResult result, string url)
        {
            if (result.Url != url)
                throw new Exception($"Expected redirect to '{url}', actual was '{result.Url}'");
            return result;
        }

        private static TViewData AssertViewDataModelType<TViewData>(ViewResultBase actionResult)
        {
            object model = actionResult.ViewData.Model;
            Type type = typeof(TViewData);
            if (model == null)
                throw new Exception($"Expected view data of type '{type.Name}', actual was NULL");
            if (!(model is TViewData))
                throw new Exception($"Expected view data of type '{typeof(TViewData).Name}', actual was '{model.GetType().Name}'");
            return (TViewData)model;
        }
    }
}
