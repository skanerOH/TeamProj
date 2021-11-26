using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.DTOs
{
    /// <summary>
    /// DTO to transfer and store on client side
    /// </summary>
    public class UserWithTokenDTO
    {
        /// <summary>
        /// Authorized user id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Authorized user full name
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Authorized user role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// JWT
        /// </summary>
        public string Token { get; set; }
    }
}
