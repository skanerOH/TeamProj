using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Validation
{
    /// <summary>
    /// Custom input validation exception
    /// </summary>
    public class ModelStateException : Exception
    {
        /// <summary>
        /// error messages
        /// </summary>
        public IEnumerable<string> errorMessages;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">single exception message</param>
        public ModelStateException(string message)
        {
            errorMessages = new List<string> { message };
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="messages">list of exception messages</param>
        public ModelStateException(IEnumerable<string> messages)
        {
            errorMessages = new List<string>(messages);
        }
    }
}
