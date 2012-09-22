namespace Snittlistan.Test
{
    using System;
    using System.Linq.Expressions;
    using System.Web.Mvc;

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
            return ActionResultHelper.AssertResultIs<ViewResult>(result);
        }

        public static PartialViewResult AssertPartialViewRendered(this ActionResult result)
        {
            return ActionResultHelper.AssertResultIs<PartialViewResult>(result);
        }

        public static RedirectResult AssertHttpRedirect(this ActionResult result)
        {
            return ActionResultHelper.AssertResultIs<RedirectResult>(result);
        }

        public static RedirectToRouteResult AssertActionRedirect(this ActionResult result)
        {
            return ActionResultHelper.AssertResultIs<RedirectToRouteResult>(result);
        }

        public static RedirectToRouteResult ToController(this RedirectToRouteResult result, string controller)
        {
            return ActionResultHelper.WithParameter(result, "controller", (object)controller);
        }

        public static RedirectToRouteResult ToAction(this RedirectToRouteResult result, string action)
        {
            return ActionResultHelper.WithParameter(result, "action", (object)action);
        }

        public static RedirectToRouteResult ToAction<TController>(this RedirectToRouteResult result, Expression<Action<TController>> action) where TController : IController
        {
            string name = ((MethodCallExpression)action.Body).Method.Name;
            string controller = typeof(TController).Name;
            if (controller.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                controller = controller.Substring(0, controller.Length - "Controller".Length);
            return ActionResultHelper.ToAction(ActionResultHelper.ToController(result, controller), name);
        }

        public static RedirectToRouteResult WithParameter(this RedirectToRouteResult result, string paramName, object value)
        {
            if (!result.RouteValues.ContainsKey(paramName))
                throw new Exception(string.Format("Could not find a parameter named '{0}' in the result's Values collection.", (object)paramName));
            object obj = result.RouteValues[paramName];
            if (!obj.Equals(value))
                throw new Exception(string.Format("When looking for a parameter named '{0}', expected '{1}' but was '{2}'.", (object)paramName, value, obj));
            else
                return result;
        }

        public static ViewResult ForView(this ViewResult result, string viewName)
        {
            if (result.ViewName != viewName)
                throw new Exception(string.Format("Expected view name '{0}', actual was '{1}'", (object)viewName, (object)result.ViewName));
            else
                return result;
        }

        public static PartialViewResult ForView(this PartialViewResult result, string partialViewName)
        {
            if (result.ViewName != partialViewName)
                throw new Exception(string.Format("Expected partial view name '{0}', actual was '{1}'", (object)partialViewName, (object)result.ViewName));
            else
                return result;
        }

        public static RedirectResult ToUrl(this RedirectResult result, string url)
        {
            if (result.Url != url)
                throw new Exception(string.Format("Expected redirect to '{0}', actual was '{1}'", (object)url, (object)result.Url));
            return result;
        }

        private static TViewData AssertViewDataModelType<TViewData>(ViewResultBase actionResult)
        {
            object model = actionResult.ViewData.Model;
            Type type = typeof(TViewData);
            if (model == null)
                throw new Exception(string.Format("Expected view data of type '{0}', actual was NULL", (object)type.Name));
            if (!typeof(TViewData).IsAssignableFrom(model.GetType()))
                throw new Exception(string.Format("Expected view data of type '{0}', actual was '{1}'", (object)typeof(TViewData).Name, (object)model.GetType().Name));
            return (TViewData)model;
        }
    }
}
