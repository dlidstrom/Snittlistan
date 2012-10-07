namespace Snittlistan.Web.Handlers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;

    using Snittlistan.Web.Areas.V1.Controllers;
    using Snittlistan.Web.Areas.V1.ViewModels.Account;
    using Snittlistan.Web.Controllers;
    using Snittlistan.Web.Events;
    using Snittlistan.Web.Infrastructure;
    using Snittlistan.Web.Services;

    public class SendRegistrationEmailHandler : IHandle<NewUserCreatedEvent>
    {
        private readonly IEmailService emailService;

        public SendRegistrationEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(NewUserCreatedEvent @event)
        {
            string recipient = @event.User.Email;
            const string Subject = "Välkommen till Snittlistan!";

            string body = RenderBody(new RegistrationMailViewModel { ActivationKey = @event.User.ActivationKey });

            this.emailService.SendMail(recipient, Subject, body);
        }

        private static string RenderBody(object viewModel)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "MailTemplates");
            var controllerContext = new ControllerContext(new MailHttpContext(), routeData, new MailController());
// ReSharper disable Mvc.ViewNotResolved
// ReSharper disable Mvc.MasterpageNotResolved
            var viewEngineResult = ViewEngines.Engines.FindView(controllerContext, "Registration", "_Layout");
// ReSharper restore Mvc.MasterpageNotResolved
// ReSharper restore Mvc.ViewNotResolved
            var stringWriter = new StringWriter();
            viewEngineResult.View.Render(
                new ViewContext(
                    controllerContext,
                    viewEngineResult.View,
                    new ViewDataDictionary(viewModel),
                    new TempDataDictionary(),
                    stringWriter),
                stringWriter);

            return stringWriter.GetStringBuilder().ToString();
        }
    }
}