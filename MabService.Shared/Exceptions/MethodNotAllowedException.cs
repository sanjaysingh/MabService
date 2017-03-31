namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Method Not Allowed Exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class MethodNotAllowedException : ExceptionBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodNotAllowedException" /> class.
        /// </summary>
        public MethodNotAllowedException() : base(Constants.MethodNotAllowedExceptionMessage)
        {
        }
    }
}
