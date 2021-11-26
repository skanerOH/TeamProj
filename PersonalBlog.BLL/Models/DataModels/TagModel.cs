using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.BLL.Models.DataModels
{
    public class TagModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public ICollection<ArticleModel> Articles { get; set; }
    }
}
