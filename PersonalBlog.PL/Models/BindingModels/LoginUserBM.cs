using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// user login binding model
    /// </summary>
    public class LoginUserBM
    {
        /// <summary>
        /// user email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// user password
        /// </summary>
        public string Password { get; set; }
    }
}
