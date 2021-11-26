using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.BLL.Models.DataModels
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public bool IsBanned { get; set; }

        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public string UserWithIdentityId { get; set; }

        public int ArticleId { get; set; }
    }
}
