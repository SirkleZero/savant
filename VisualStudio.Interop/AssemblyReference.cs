namespace VisualStudio.Interop
{
    /// <summary>
    /// Describes the state of a referenced assembly.
    /// </summary>
    public enum AssemblyReference
    {
        /// <summary>
        /// Indicates that the assembly being compared is current.
        /// </summary>
        Current,
        /// <summary>
        /// Indicates that the assembly is not referenced.
        /// </summary>
        Missing,
        /// <summary>
        /// Indicates that the reference being compared is newer than the original.
        /// </summary>
        Newer,
        /// <summary>
        /// Indicates that the reference being compared is older than the original.
        /// </summary>
        Older
    }
}
