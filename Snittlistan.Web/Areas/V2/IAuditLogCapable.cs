#nullable enable

using Raven.Client.Documents.Session;
using Snittlistan.Web.Areas.V2.Domain;

namespace Snittlistan.Web.Areas.V2;

public interface IAuditLogCapable
{
    FormattedAuditLog GetFormattedAuditLog(IDocumentSession documentSession);
}
