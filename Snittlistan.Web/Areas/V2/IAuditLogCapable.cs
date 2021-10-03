namespace Snittlistan.Web.Areas.V2
{
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;

    public interface IAuditLogCapable
    {
        FormattedAuditLog GetFormattedAuditLog(IDocumentSession documentSession);
    }
}
