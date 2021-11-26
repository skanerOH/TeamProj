using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// User registration data binding model
    /// </summary>
    public class RegisterUserBM
    {
        /// <summary>
        /// User full name
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "User name length must be >=6")]
        [MaxLength(50, ErrorMessage = "User name length must be <=50")]
        public string FullName { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }
    }
}
