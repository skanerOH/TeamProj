using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.BLL.Models.DataModels
{
    public class BlogModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsBanned { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset ModifiedAt { get; set; }

        public string UserWithIdentityId { get; set; }

        public ICollection<ArticleModel> Articles { get; set; }
    }
}
