using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.BLL.Models.DataModels
{
    public class UserModel
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Role { get; set; }

        public ICollection<CommentModel> CommentModels { get; set; }

        public ICollection<BlogModel> BlogModels { get; set; }
    }
}
