using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Snittlistan.Test
{
    public static class ActionResultHelper
    {
        public static T AssertResultIs<T>(this ActionResult result) where T : ActionResult
        {
            T obj = result as T;
            if (obj == null)
                throw new Exception(string.Format("Expected result to be of type {0}. It is actually of type {1}.", (object)typeof(T).Name, (object)result.GetType().Name));
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
                throw new Exception(string.Format("Could not find a parameter named '{0}' in the result's Values collection.", paramName));
            object obj = result.RouteValues[paramName];
            if (!obj.Equals(value))
                throw new Exception(string.Format("When looking for a parameter named '{0}', expected '{1}' but was '{2}'.", paramName, value, obj));
            return result;
        }

        public static ViewResult ForView(this ViewResult result, string viewName)
        {
            if (result.ViewName != viewName)
                throw new Exception(string.Format("Expected view name '{0}', actual was '{1}'", viewName, result.ViewName));
            return result;
        }

        public static PartialViewResult ForView(this PartialViewResult result, string partialViewName)
        {
            if (result.ViewName != partialViewName)
                throw new Exception(string.Format("Expected partial view name '{0}', actual was '{1}'", partialViewName, result.ViewName));
            return result;
        }

        public static RedirectResult ToUrl(this RedirectResult result, string url)
        {
            if (result.Url != url)
                throw new Exception(string.Format("Expected redirect to '{0}', actual was '{1}'", url, result.Url));
            return result;
        }

        private static TViewData AssertViewDataModelType<TViewData>(ViewResultBase actionResult)
        {
            object model = actionResult.ViewData.Model;
            Type type = typeof(TViewData);
            if (model == null)
                throw new Exception(string.Format("Expected view data of type '{0}', actual was NULL", type.Name));
            if (!(model is TViewData))
                throw new Exception(string.Format("Expected view data of type '{0}', actual was '{1}'", typeof(TViewData).Name, model.GetType().Name));
            return (TViewData)model;
        }
    }
}
