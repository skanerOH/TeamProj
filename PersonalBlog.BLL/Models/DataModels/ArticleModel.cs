using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models.DataModels
{
    public class ArticleModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public bool IsBanned { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public int BlogId { get; set; }

        public BlogModel Blog { get; set; }

        public ICollection<CommentModel> Comments { get; set; }

        public ICollection<TagModel> Tags { get; set; }
    }
}
