using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// Blog data binding model
    /// </summary>
    public class BlogBM
    {
        /// <summary>
        /// blog title
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "Blog title length must be >=6")]
        [MaxLength(50, ErrorMessage = "Blog title length must be <=50")]
        public string Title { get; set; }

        /// <summary>
        /// blog description
        /// </summary>
        [MaxLength(2000, ErrorMessage = "Blog description length must be <=2000")]
        public string Description { get; set; }
    }
}
