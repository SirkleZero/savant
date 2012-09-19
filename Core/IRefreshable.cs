namespace Savant.Core
{
    /// <summary>
    /// Provides functionality for objects that require internal data to be refreshed.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Refreshes the internal state of the object.
        /// </summary>
        void Refresh();
    }
}
