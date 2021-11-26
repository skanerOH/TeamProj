using AutoMapper;
using PersonalBlog.BLL.Interfaces;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.BLL.Validation;
using PersonalBlog.DAL.Entities;
using PersonalBlog.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.BLL.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task BanArticleByIdAsync(int articleId, string userId, string userRole)
        {
            var dbarticle = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);

            if (!userRole.Equals("admin"))
                throw new BlogsException("only admins are permitted to ban");

            if (dbarticle == null)
                throw new BlogsException("unexisting article");

            dbarticle.IsBanned = true;

            _unitOfWork.ArticleRepository.Update(dbarticle);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateArticleAsync(int blogId, string userId, ArticleModel articleModel)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);

            if (blog == null)
                throw new BlogsException("unexisting blog");

            if (!blog.UserWithIdentityId.Equals(userId))
                throw new BlogsException("user can create articles only in his blog");

            var article = _mapper.Map<ArticleModel, Article>(articleModel);
            article.BlogId = blogId;
            article.CreatedAt = DateTimeOffset.UtcNow;
            article.ModifiedAt = article.CreatedAt;
            article.IsBanned = false;
            article.IsDeleted = false;
            var tags = new List<Tag>(article.Tags);
            article.Tags.Clear();

            await _unitOfWork.ArticleRepository.AddAsync(article);

            foreach (var t in tags)
            {
                article.Tags.Add(t);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteArticleAsync(int articleId, string userId)
        {
            var dbarticle = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);

            if (dbarticle == null)
                throw new BlogsException("unexisting article");

            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(dbarticle.BlogId);

            if (blog == null)
                throw new BlogsException("unexisting blog");

            if (!blog.UserWithIdentityId.Equals(userId))
                throw new BlogsException("only article creator permitted to delete it");

            await _unitOfWork.ArticleRepository.DeleteByIdAsync(articleId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<ArticleModel> GetArticleById(int articleId)
        {
            var dbarticle = await _unitOfWork.ArticleRepository.GetArticleWithDatailsByIdAsync(articleId);

            if (dbarticle == null)
                throw new BlogsException("unexisting article");

            return _mapper.Map<Article, ArticleModel>(dbarticle);
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesByFiltersAsync(string textSearchStr, IEnumerable<string> tags, IEnumerable<int> takenIds, int countToTake, string userRole)
        {
            if (userRole.Equals("admin"))
            {
                return _mapper.Map<IEnumerable<Article>, IEnumerable<ArticleModel>>(
                    await _unitOfWork.ArticleRepository.SearchArticleWithDetailsByFiltersAndTakeThatNotTakenAsync(textSearchStr, tags, takenIds, countToTake, (a => a != null)));
            }
            else
            {
                return _mapper.Map<IEnumerable<Article>, IEnumerable<ArticleModel>>(
                    await _unitOfWork.ArticleRepository.SearchArticleWithDetailsByFiltersAndTakeThatNotTakenAsync(textSearchStr, tags, takenIds, countToTake, (a => !a.IsBanned)));
            }
        }

        public async Task<IEnumerable<ArticleModel>> GetArticlesWithDetailsByBlogIdAsync(int blogId, string userId, string userRole)
        {
            if (await _unitOfWork.BlogRepository.GetByIdAsync(blogId) == null)
                throw new BlogsException("blog with provided id does not exist");

            IEnumerable<Article> dbarticles;
            if (userRole.Equals("admin"))
            {
                dbarticles = await _unitOfWork.ArticleRepository.GetArticlesWithDetailsAsync(a => a.BlogId == blogId);
            }
            else
            {
                dbarticles = await _unitOfWork.ArticleRepository.GetArticlesWithDetailsAsync(a => a.BlogId == blogId && (a.Blog.UserWithIdentityId.CompareTo(userId) == 0 || !a.IsBanned));
            }
            return _mapper.Map<IEnumerable<Article>, IEnumerable<ArticleModel>>(dbarticles);
        }

        public async Task UnbanArticleByIdAsync(int articleId, string userId, string userRole)
        {
            var dbarticle = await _unitOfWork.ArticleRepository.GetByIdAsync(articleId);

            if (!userRole.Equals("admin"))
                throw new BlogsException("only admins are permitted to unban");

            if (dbarticle == null)
                throw new BlogsException("unexisting article");

            dbarticle.IsBanned = false;

            _unitOfWork.ArticleRepository.Update(dbarticle);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateArticleAsync(int blogId, string userId, ArticleModel articleModel)
        {
            var blog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);

            if (blog == null)
                throw new BlogsException("unexisting blog");

            if (blog.UserWithIdentityId.CompareTo(userId) != 0)
                throw new BlogsException("user can edit articles only in his blog");

            var dbarticle = await _unitOfWork.ArticleRepository.GetArticleWithDatailsByIdAsync(articleModel.Id);

            if (dbarticle == null)
                throw new BlogsException("article with provided id does not exist");

            dbarticle.Title = articleModel.Title;
            dbarticle.Text = articleModel.Text;
            dbarticle.ModifiedAt = DateTimeOffset.UtcNow;
            var modelTags = _mapper.Map<ICollection<TagModel>, ICollection<Tag>>(articleModel.Tags);
            var tagsToAdd = modelTags.Where(tag => !dbarticle.Tags.Any(t => t.Id == tag.Id)).ToList();
            var tagsToDelete = dbarticle.Tags.Where(tag => !modelTags.Any(t => t.Id == tag.Id)).ToList();
            foreach (var tag in tagsToAdd)
            {
                dbarticle.Tags.Add(tag);
            }
            foreach (var tag in tagsToDelete)
            {
                dbarticle.Tags.Remove(tag);
            }

            _unitOfWork.ArticleRepository.Update(dbarticle);
            await _unitOfWork.SaveAsync();
        }
    }
}
