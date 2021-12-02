using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonalBlog.PL.Models.ViewModels
{
    /// <summary>
    /// Comment view model
    /// </summary>
    public class CommentVM
    {
        /// <summary>
        /// comment id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// comment text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// latest modification date
        /// </summary>
        public string ModifiedAt { get; set; }

        /// <summary>
        /// comment publisher full name
        /// </summary>
        public string PublisherName { get; set; }

        /// <summary>
        /// comment publisher id
        /// </summary>
        public string PublisherId { get; set; }

        /// <summary>
        /// flag, true when comment is banned
        /// </summary>
        public bool IsBanned { get; set; }
    }
}
