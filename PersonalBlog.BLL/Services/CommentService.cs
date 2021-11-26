using AutoMapper;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.BLL.Validation;
using PersonalBlog.DAL.Entities;
using PersonalBlog.DAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserWithIdentity> _userManager;

        public CommentService(IMapper mapper, IUnitOfWork unitOfWork, UserManager<UserWithIdentity> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task AddCommentAsync(string text, string userId, int articleId)
        {
            if (await _userManager.FindByIdAsync(userId) == null)
                throw new BlogsException("unexisting user");
            if (await _unitOfWork.ArticleRepository.GetByIdAsync(articleId) == null)
                throw new BlogsException("unexisting article");
            var dbcomment = new Comment
            {
                ArticleId = articleId,
                Text = text,
                IsBanned = false,
                IsDeleted = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UserWithIdentityId = userId
            };
            dbcomment.ModifiedAt = dbcomment.CreatedAt;

            await _unitOfWork.CommentRepository.AddAsync(dbcomment);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCommentBanStatusAsync(int commentId, bool isBanned)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);
            if (comment == null)
                throw new BlogsException("comment does not exists");
            comment.IsBanned = isBanned;
            comment.ModifiedAt = DateTimeOffset.UtcNow;
            _unitOfWork.CommentRepository.Update(comment);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsByArticleIdForAdminAsync(int articleId)
        {
            if (await _unitOfWork.ArticleRepository.GetByIdAsync(articleId) == null)
                throw new BlogsException("unexisting article");
            return (await _unitOfWork.CommentRepository.GetCommentsAsync(c => c.ArticleId == articleId)).Select(c => _mapper.Map<Comment, CommentModel>(c));
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsByArticleIdForUserAsync(int articleId)
        {
            if (await _unitOfWork.ArticleRepository.GetByIdAsync(articleId) == null)
                throw new BlogsException("unexisting article");
            return (await _unitOfWork.CommentRepository.GetCommentsAsync(c => c.ArticleId == articleId && !c.IsBanned)).Select(c => _mapper.Map<Comment, CommentModel>(c));
        }

        public async Task UpdateCommentTextAsync(int commentId, string userId, string text)
        {
            var dbcomment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);
            if (dbcomment == null)
                throw new BlogsException("comment with provided id does not exist");

            if (!dbcomment.UserWithIdentityId.Equals(userId))
                throw new BlogsException("unable to edit comment of other user");
            dbcomment.Text = text;
            dbcomment.ModifiedAt = DateTimeOffset.UtcNow;
            _unitOfWork.CommentRepository.Update(dbcomment);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCommentAsync(int commentId, string userId, string userRole)
        {
            var dbcomment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);

            if (userId.Equals(dbcomment.UserWithIdentityId) || userRole.Equals("admin"))
            {
                await _unitOfWork.CommentRepository.DeleteByIdAsync(commentId);
                await _unitOfWork.SaveAsync();
            }

            throw new BlogsException("current user has no permission to delete this comment");
        }

        public async Task<CommentModel> GetCommentByIdAsync(int commentId)
        {
            var dbcomment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);
            if (dbcomment == null)
                throw new BlogsException("comment with provided id does not exist");

            return _mapper.Map<Comment, CommentModel>(dbcomment);
        }
    }
}
