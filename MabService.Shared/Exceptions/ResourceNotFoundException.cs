namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Resource not found exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class ResourceNotFoundException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceNotFoundException" /> class.
        /// </summary>
        public ResourceNotFoundException() : base(Constants.ResourceNotFoundMessage)
        {
        }
    }
}
