using PersonalBlog.PL.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.BindingModels
{
    /// <summary>
    /// article search filter data
    /// </summary>
    public class ArticleSearchFilterBM
    {
        /// <summary>
        /// string to search in article content
        /// </summary>
        [MaxLength(100, ErrorMessage = "Length of search string must be <100")]
        public string S { get; set; } = "";

        /// <summary>
        /// list of taken ids
        /// </summary>
        [MinMaxTagCollection(3, 100)]
        public IEnumerable<string> T { get; set; } = new List<string>();

        /// <summary>
        /// list of filter tags
        /// </summary>
        public IEnumerable<int> I { get; set; } = new List<int>();
    }
}
