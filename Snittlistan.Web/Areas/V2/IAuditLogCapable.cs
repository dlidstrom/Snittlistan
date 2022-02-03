
using Raven.Client;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2;
public interface IAuditLogCapable
{
    FormattedAuditLog GetFormattedAuditLog(IDocumentSession documentSession);
}
