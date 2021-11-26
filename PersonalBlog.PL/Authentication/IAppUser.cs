using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Authentication
{
    /// <summary>
    /// interface for AppUser
    /// </summary>
    public interface IAppUser
    {
        /// <summary>
        /// current user id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// current user role
        /// </summary>
        string Role { get; }
    }
}
