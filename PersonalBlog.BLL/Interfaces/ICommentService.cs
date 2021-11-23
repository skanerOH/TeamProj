using BLL.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICommentService
    {
        public Task<IEnumerable<CommentModel>> GetCommentsByArticleIdForUserAsync(int articleId);

        public Task<IEnumerable<CommentModel>> GetCommentsByArticleIdForAdminAsync(int articleId);

        public Task<IEnumerable<CommentModel>> GetCommentsByArticleIdAsync(int articleId, string userId, string userRole);

        public Task AddCommentAsync(string text, string userId, int articleId);

        public Task UpdateCommentBanStatusAsync(int commentId, bool isBanned);

        public Task UpdateCommentTextAsync(int commentId, string userId, string text);

        public Task DeleteCommentAsync(int commentId, string userId, string userRole);

        public Task<CommentModel> GetCommentByIdAsync(int commentId);
    }
}
