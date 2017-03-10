namespace MabService.Shared.Exceptions
{
    /// <summary>
    /// Resource not found exception
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class CollectionNotFoundException : ResourceNotFoundException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionNotFoundException" /> class.
        /// </summary>
        public CollectionNotFoundException()
        {
        }
    }
}
