using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// change password data binding model
    /// </summary>
    public class ChangePasswordBM
    {
        /// <summary>
        /// current user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// new password
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// confirm new password
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}
