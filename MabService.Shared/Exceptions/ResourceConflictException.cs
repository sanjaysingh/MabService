namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Resource conflict exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ResourceConflictException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceConflictException" /> class.
        /// </summary>
        public ResourceConflictException() : base(Constants.ResourceConflictMessage)
        {
        }
    }
}
