using PersonalBlog.PL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// Article data binding model
    /// </summary>
    public class ArticleBM
    {
        /// <summary>
        /// article title
        /// </summary>
        [Required]
        [MinLength(6, ErrorMessage = "Article title length must be >=6")]
        [MaxLength(100, ErrorMessage = "Article title length must be <=100")]
        public string Title { get; set; }

        /// <summary>
        /// article text
        /// </summary>
        [MaxLength(8000, ErrorMessage = "Article text length must be <=8000")]
        public string Text { get; set; }

        /// <summary>
        /// list of article tags
        /// </summary>
        [MinMaxTagCollection(3, 100)]
        public IEnumerable<string> Tags { get; set; }
    }
}
