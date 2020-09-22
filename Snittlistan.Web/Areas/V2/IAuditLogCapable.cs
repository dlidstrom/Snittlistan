namespace Snittlistan.Web.Areas.V2
{
    using Raven.Client;
    using Snittlistan.Web.Areas.V2.Domain;

    public interface IAuditLogCapable
    {
        FormattedAuditLogEntry[] GetHistory(IDocumentSession documentSession);
    }
}
