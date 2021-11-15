using PersonalBlog.DAL.Interfaces;
using PersonalBlog.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogsDBContext _blogsDBContext;

        private IBlogRepository blogRepository;
        private IArticleRepository articleRepository;
        private ITagRepository tagRepository;
        private ICommentRepository commentRepository;

        public UnitOfWork(BlogsDBContext blogsDBContext)
        {
            _blogsDBContext = blogsDBContext;
        }

        public IBlogRepository BlogRepository
        {
            get
            {
                if (blogRepository == null)
                    blogRepository = new BlogRepository(_blogsDBContext);
                return blogRepository;
            }
        }


        public IArticleRepository ArticleRepository
        {
            get
            {
                if (articleRepository == null)
                    articleRepository = new ArticleRepository(_blogsDBContext);
                return articleRepository;
            }
        }

        public ICommentRepository CommentRepository
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(_blogsDBContext);
                return commentRepository;
            }
        }

        public ITagRepository TagRepository
        {
            get
            {
                if (tagRepository == null)
                    tagRepository = new TagRepository(_blogsDBContext);
                return tagRepository;
            }
        }

        public async Task<int> SaveAsync()
        {
            return await _blogsDBContext.SaveChangesAsync();
        }
    }

}
