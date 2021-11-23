using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Validation
{
    public class BlogsException : Exception
    {
        public IEnumerable<string> errorMessages;

        public BlogsException(string message)
        {
            errorMessages = new List<string> { message};
        }

        public BlogsException(IEnumerable<string> messages)
        {
            errorMessages = new List<string>(messages);
        }
    }
}
