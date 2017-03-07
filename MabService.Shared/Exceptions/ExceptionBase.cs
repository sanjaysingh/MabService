using MabService.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MabService.Shared.Exceptions
{
    public class ExceptionBase : Exception
    {
        private IEnumerable<string> errors = Enumerable.Empty<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected ExceptionBase(string message):base(message)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="error">The error.</param>
        protected ExceptionBase(string message, string error):this(message, new string[] { error})
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        protected ExceptionBase(string message, IEnumerable<string> errors):base(message)
        {
            this.errors = errors;
        }

        /// <summary>
        /// To the error response.
        /// </summary>
        /// <returns></returns>
        public ErrorResponseModel ToErrorResponse()
        {
            return new ErrorResponseModel(this.GetType().Name, this.errors);
        }
    }
}
