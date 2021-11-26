using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.ResponseModels
{
    /// <summary>
    /// Response model
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// response code
        /// </summary>
        public ResponseModelCode responseCode { get; set; }

        /// <summary>
        /// response message
        /// </summary>
        public string responseMessage { get; set; }

        /// <summary>
        /// response data
        /// If response code = OK(1), data contains requested data
        /// If response code = Error(2), data contains list of error strings
        /// </summary>
        public object data { get; set; }
    }
}
