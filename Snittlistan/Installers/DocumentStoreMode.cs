namespace Snittlistan.Installers
{
    public enum DocumentStoreMode
    {
        /// <summary>
        /// Run in-memory. Suitable for testing.
        /// </summary>
        InMemory,

        /// <summary>
        /// Run embedded. Used when in production.
        /// </summary>
        Embeddable,

        /// <summary>
        /// Run with standalone server. Used when debugging.
        /// </summary>
        Server
    }
}