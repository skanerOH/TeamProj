using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.ViewModels
{
    /// <summary>
    /// Blog view model
    /// </summary>
    public class BlogVM
    {
        /// <summary>
        /// blog id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// blog title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// blog description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// latest modification date
        /// </summary>
        public string ModifiedAt { get; set; }

        /// <summary>
        /// blog`s publisher id
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// blog`s publisher full name
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// flag, true when blog is banned
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// count of articles attached to blog
        /// </summary>
        public int ArticlesCount { get; set; }
    }
}
