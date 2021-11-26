using PersonalBlog.BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Interfaces
{
    public interface IArticleService
    {
        public Task<IEnumerable<ArticleModel>> GetArticlesWithDetailsByBlogIdAsync(int blogId, string userId, string userRole);

        public Task CreateArticleAsync(int blogId, string userId, ArticleModel articleModel);

        public Task UpdateArticleAsync(int blogId, string userId, ArticleModel articleModel);

        public Task BanArticleByIdAsync(int articleId, string userId, string userRole);

        public Task UnbanArticleByIdAsync(int articleId, string userId, string userRole);

        public Task DeleteArticleAsync(int articleId, string userId);

        public Task<ArticleModel> GetArticleById(int articleId);

        public Task<IEnumerable<ArticleModel>> GetArticlesByFiltersAsync(string textSearchStr, IEnumerable<string> tags, IEnumerable<int> takenIds, int countToTake, string userRole);
    }
}
