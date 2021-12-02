using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// comment data binding model
    /// </summary>
    public class CommentBM
    {
        /// <summary>
        /// comment text
        /// </summary>
        [Required]
        [MinLength(3, ErrorMessage = "Comment text length must be >=3")]
        [MaxLength(1000, ErrorMessage = "Comment text length must be <=1000")]
        public string Text { get; set; }
    }
}
