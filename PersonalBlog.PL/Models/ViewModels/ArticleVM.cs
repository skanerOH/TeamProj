using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.ViewModels
{
    /// <summary>
    /// Article view model
    /// </summary>
    public class ArticleVM
    {
        /// <summary>
        /// article id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// article title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// article text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// count of unbanned comments on this post
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// latest modification date
        /// </summary>
        public string ModifiedAt { get; set; }

        /// <summary>
        /// blog id
        /// </summary>
        public int BlogId { get; set; }

        /// <summary>
        /// blog title
        /// </summary>
        public string BlogTitle { get; set; }

        /// <summary>
        /// id of article publisher
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// flag, true when article is banned
        /// </summary>
        public bool IsBanned { get; set; }

        /// <summary>
        /// list of tags attached to article
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
    }
}
