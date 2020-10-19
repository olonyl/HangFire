namespace HangFire.Infrastructure.Adapter
{
    /// <summary>
    /// Base contract for adapter factory
    /// </summary>
    public interface ITypeAdapterFactory
    {
        /// <summary>
        /// Create a type adapter
        /// </summary>
        /// <returns>The created ITypeAdapter</returns>
        ITypeAdapter Create();
    }
}
