using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        IBlogRepository BlogRepository { get; }

        IArticleRepository ArticleRepository { get; }

        ICommentRepository CommentRepository { get; }

        ITagRepository TagRepository { get; }

        Task<int> SaveAsync();
    }

}
