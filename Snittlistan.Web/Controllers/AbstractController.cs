#nullable enable

using System.Web.Mvc;
using Castle.Core.Logging;
using Snittlistan.Web.Commands;
using Snittlistan.Web.Infrastructure;
using Snittlistan.Web.Infrastructure.Database;
using Snittlistan.Web.Models;

namespace Snittlistan.Web.Controllers;

public abstract class AbstractController : Controller
{
    private readonly Lazy<CommandExecutor> commandExecutor;

    public AbstractController()
    {
        commandExecutor = new Lazy<CommandExecutor>(CreateCommandExecutor);
    }

    public CompositionRoot CompositionRoot { get; set; } = null!;

    public ILogger Logger { get; set; } = NullLogger.Instance;

    protected new CustomPrincipal User => (CustomPrincipal)HttpContext.User;

    protected async Task<TaskPublisher> GetTaskPublisher()
    {
        Tenant currentTenant = await CompositionRoot.GetCurrentTenant();
        return new TaskPublisher(
            currentTenant,
            CompositionRoot.Databases,
            CompositionRoot.CorrelationId,
            null);
    }

    protected async Task ExecuteCommand<TCommand>(TCommand command)
        where TCommand : class
    {
        await commandExecutor.Value.Execute(command);
    }

    protected override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        // load website config to make sure it always migrates
        WebsiteConfig websiteContent = CompositionRoot.DocumentSession.Load<WebsiteConfig>(WebsiteConfig.GlobalId);
        if (websiteContent == null)
        {
            CompositionRoot.DocumentSession.Store(new WebsiteConfig(new WebsiteConfig.TeamNameAndLevel[0], false, -1, 2019));
        }

        // make sure there's an admin user
        if (CompositionRoot.DocumentSession.Load<User>(Models.User.AdminId) != null)
        {
            return;
        }

        // first launch
        Response.Redirect("/v1/welcome");
        Response.End();
    }

    protected override void OnActionExecuted(ActionExecutedContext filterContext)
    {
        if (filterContext.IsChildAction || filterContext.Exception != null)
        {
            return;
        }

        // this commits the document session
        CompositionRoot.EventStoreSession.SaveChanges();

        int changesSaved = CompositionRoot.Databases.Snittlistan.SaveChanges();
        if (changesSaved > 0)
        {
            Logger.InfoFormat(
                "saved {changesSaved} to database",
                changesSaved);
        }
    }

    private CommandExecutor CreateCommandExecutor()
    {
        return new CommandExecutor(
            CompositionRoot,
            CompositionRoot.CorrelationId,
            null,
            User.CustomIdentity.Name);
    }
}
