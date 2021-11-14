using PersonalBlog.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalBlog.DAL.Interfaces
{
    public interface IBlogRepository : IRepository<Blog>
    {
        public Task<IEnumerable<Blog>> GetAllBlogsWithDetailsAsync(Expression<Func<Blog, bool>> expression);

        public Task<Blog> GetBlogWithDetailsByIdAsync(int id);
    }
}
