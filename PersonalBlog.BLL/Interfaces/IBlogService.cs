using PersonalBlog.BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Interfaces
{
    public interface IBlogService
    {
        public Task<IEnumerable<BlogModel>> GetAllBlogsWithDetailsAsync(string userId, string userRole);

        public Task CreateBlogAsync(BlogModel blogModel);

        public Task UpdateBlogAsync(BlogModel blogModel);

        public Task BanArticle(int blogId, string userRole);

        public Task UnbanArticle(int blogId, string userRole);

        public Task DeleteBlogAsync(int blogId, string userId);
    }
}
