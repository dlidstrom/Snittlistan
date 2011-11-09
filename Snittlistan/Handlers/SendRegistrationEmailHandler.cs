namespace Snittlistan.Handlers
{
    using System.IO;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Snittlistan.Controllers;
    using Snittlistan.Events;
    using Snittlistan.Infrastructure;
    using Snittlistan.Services;
    using Snittlistan.ViewModels;

    public class SendRegistrationEmailHandler : IHandle<NewUserCreatedEvent>
    {
        private IEmailService emailService;

        public SendRegistrationEmailHandler(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        public void Handle(NewUserCreatedEvent @event)
        {
            string recipient = @event.User.Email;
            string subject = "Välkommen till Snittlistan!";

            string body = RenderBody(new RegistrationMailViewModel { ActivationKey = @event.User.ActivationKey });

            emailService.SendMail(recipient, subject, body);
        }

        private static string RenderBody(object viewModel)
        {
            var routeData = new RouteData();
            routeData.Values.Add("controller", "MailTemplates");
            var controllerContext = new ControllerContext(new MailHttpContext(), routeData, new MailController());
            var viewEngineResult = ViewEngines.Engines.FindView(controllerContext, "Registration", "_Layout");
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