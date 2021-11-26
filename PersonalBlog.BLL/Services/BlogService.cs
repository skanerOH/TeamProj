using AutoMapper;
using PersonalBlog.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.Interfaces;
using System.Threading.Tasks;
using PersonalBlog.BLL.Models.DataModels;
using PersonalBlog.DAL.Entities;
using PersonalBlog.BLL.Validation;

namespace PersonalBlog.BLL.Services
{
    public class BlogService : IBlogService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task BanArticle(int blogId, string userRole)
        {
            var dbblog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);

            if (dbblog == null)
                throw new BlogsException("unexisting blog");

            if (!userRole.Equals("admin"))
                throw new BlogsException("only admins are permitted to ban");

            dbblog.IsBanned = true;

            _unitOfWork.BlogRepository.Update(dbblog);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateBlogAsync(BlogModel blogModel)
        {
            var dbblog = _mapper.Map<BlogModel, Blog>(blogModel);

            if (dbblog == null)
                throw new BlogsException("unable to create blog with invalid parameters");

            dbblog.CreatedAt = DateTimeOffset.UtcNow;
            dbblog.ModifiedAt = dbblog.CreatedAt;
            dbblog.IsDeleted = false;
            dbblog.IsBanned = false;

            await _unitOfWork.BlogRepository.AddAsync(dbblog);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteBlogAsync(int blogId, string userId)
        {
            var dbblog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);

            if (dbblog == null)
                throw new BlogsException("unexisting blog");

            if (!dbblog.UserWithIdentityId.Equals(userId))
                throw new BlogsException("only blog creator is permitted to delete it");

            await _unitOfWork.BlogRepository.DeleteByIdAsync(blogId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BlogModel>> GetAllBlogsWithDetailsAsync(string userId, string userRole)
        {
            IEnumerable<Blog> dbBlogs;
            if (userRole.Equals("admin"))
            {
                dbBlogs = await _unitOfWork.BlogRepository.GetAllBlogsWithDetailsAsync(b => b != null);
            }
            else
            {
                dbBlogs = await _unitOfWork.BlogRepository.GetAllBlogsWithDetailsAsync(b => !b.IsBanned || b.UserWithIdentityId.Equals(userId));
            }

            return _mapper.Map<IEnumerable<Blog>, IEnumerable<BlogModel>>(dbBlogs);
        }

        public async Task UnbanArticle(int blogId, string userRole)
        {
            var dbblog = await _unitOfWork.BlogRepository.GetByIdAsync(blogId);

            if (dbblog == null)
                throw new BlogsException("unexisting blog");

            if (!userRole.Equals("admin"))
                throw new BlogsException("only admins are permitted to unban");

            dbblog.IsBanned = false;

            _unitOfWork.BlogRepository.Update(dbblog);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateBlogAsync(BlogModel blogModel)
        {
            var dbblog = await _unitOfWork.BlogRepository.GetByIdAsync(blogModel.Id);

            if (dbblog == null)
                throw new BlogsException("unexisting blog");

            if (!dbblog.UserWithIdentityId.Equals(blogModel.UserWithIdentityId))
                throw new BlogsException("only blog creator is permitted to edit it");

            dbblog.Title = blogModel.Title;
            dbblog.Description = blogModel.Description;
            dbblog.ModifiedAt = DateTimeOffset.UtcNow;

            _unitOfWork.BlogRepository.Update(dbblog);
            await _unitOfWork.SaveAsync();
        }
    }
}
