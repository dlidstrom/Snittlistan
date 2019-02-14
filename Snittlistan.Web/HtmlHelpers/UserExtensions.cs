namespace Snittlistan.Web.HtmlHelpers
{
    using Infrastructure;

    public static class UserExtensions
    {
        public static bool IsInRole2(this CustomPrincipal user, string role)
        {
            return user?.IsInRole(role) ?? false;
        }
    }
}