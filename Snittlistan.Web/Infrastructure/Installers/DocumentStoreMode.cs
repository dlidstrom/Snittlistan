namespace Snittlistan.Web.Infrastructure.Installers;

public enum DocumentStoreMode
{
    /// <summary>
    /// Run in-memory. Suitable for testing.
    /// </summary>
    InMemory,

    /// <summary>
    /// Run with standalone server. Used when debugging.
    /// </summary>
    Server
}
